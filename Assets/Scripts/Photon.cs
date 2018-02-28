using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour {
    static Select m_Magnet;
    static DarkAI m_DAI;

    bool m_darkAttack = false;

    [SerializeField] float m_speed = 0.2f;
    [SerializeField] float m_reaction = 15.0f;

    GameObject m_Job = null;

    public bool m_heldPower = false;

    public bool m_isLight = true;
    public GameObject m_target = null;
    public Home m_home = null;

    Vector2 m_direction = Vector2.zero;

    bool m_Wall = false;
    Transform m_WallGo = null;
    float m_wallTime = 0.0f;

    bool m_DualHitFix = false;
    bool m_DualKillFix = false;
    // Use this for initialization
    void Start ()
    {
        if (!m_Magnet)
            m_Magnet = FindObjectOfType<Select>();

        if (!m_DAI)
            m_DAI = FindObjectOfType<DarkAI>();

        General.colorCorrection(gameObject, m_isLight);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_isLight)
        {
            DarkAIAttackCheck();
        }

        if (m_Wall)
        {
            m_wallTime += Time.deltaTime;
            m_direction = m_WallGo.position - transform.position;

            if (m_wallTime >= 0.5f)
                m_Wall = false;
        }
        else
        if (m_heldPower && m_home)
        {
            m_direction = m_home.gameObject.transform.position - transform.position;
        }
        else
        if (!m_darkAttack)
        {
            Vector2 newDir = Wander(2.0f) + Cohesion(5.0f, 1.0f) + Separation(1.0f, 5.0f)
                + Chase(3.0f, 3.0f) + FollowMagnet(2.25f, 10.0f) + FollowJob(5.0f);
            newDir.Normalize();

            m_direction = Vector2.Lerp(m_direction, newDir, Time.deltaTime * m_reaction);
        }
        else
        {
            m_direction = Vector2.Lerp(m_direction, DarkAttack(), Time.deltaTime * m_reaction);
        }

        m_direction.Normalize();
        
        float angle = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_reaction);


        Vector3 dir = m_direction * m_speed * Time.deltaTime;
        transform.position += dir;
    }

    Vector2 Wander(float power)
    {
        Vector2 ranDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        ranDir.Normalize();
        return ranDir * power;
    }


    Vector2 Cohesion(float radius, float power)
    {
        int MaxToCheck = 5;
        int Numchecked = 0;
        Vector3 velocity = Vector3.zero;

        GameObject[] gameObjects = General.GetGameObjects(gameObject, gameObject.tag, MaxToCheck, radius);
        if (gameObjects.Length > 0)
        {
            Vector3 center = Vector3.zero;

            foreach (GameObject obj in gameObjects)
            {
                center += obj.transform.position;
                Numchecked++;
                if (Numchecked > MaxToCheck)
                    break;
            }

            center = center / Numchecked;

            Vector3 direction = center - transform.position;
            direction.z = 0.0f;
            velocity = direction.normalized * power;
        }
        velocity.z = 0.0f;

        return velocity;
    }

    Vector2 Separation(float radius, float power)
    {
        int MaxToCheck = 5;
        int Numchecked = 0;
        Vector3 velocity = Vector3.zero;

        GameObject[] gameObjects = General.GetGameObjects(gameObject, gameObject.tag, MaxToCheck, radius);
        if (gameObjects.Length > 0)
        {
            foreach (GameObject obj in gameObjects)
            {
                Vector3 direction = transform.position - obj.transform.position;

                float strength = direction.magnitude / radius;
                strength = 1.0f - strength;
                direction.Normalize();

                velocity += direction * strength;
                Numchecked++;
                if (Numchecked > MaxToCheck)
                    break;
            }
            velocity = velocity / Numchecked;
            velocity *= power;
        }
        velocity.z = 0.0f;

        return velocity;
    }

    Vector2 Chase(float radius, float power)
    {
        Vector3 toReturn = Vector3.zero;

        if (m_target != null)
        {
            if ((m_target.transform.position - transform.position).magnitude > radius)
            {
                m_target = null;
            }
        }
        else
        {

            string findTag = m_isLight ? "PhotonDark" : "PhotonLight";

            m_target = General.GetNearestGameObject(gameObject, findTag, radius);
        }

        if (m_target != null)
        {
            toReturn = m_target.transform.position - transform.position;
            toReturn.Normalize();
            toReturn *= power;
        }

        return toReturn;
    }

    Vector2 FollowMagnet(float radius, float power)
    {
        Vector3 toReturn = Vector3.zero;

        if (m_isLight && m_Magnet.isActive)
        {
            toReturn = m_Magnet.transform.position - transform.position;

            float Amp = (radius - toReturn.magnitude);
            if (Amp < 0.0f)
                Amp = 0.0f;

            toReturn *= Amp;

            if (toReturn.magnitude > 0.5f)
            {
                m_Job = null;
            }
        }
        return toReturn * power;
    }

    Vector2 FollowJob(float power)
    {
        Vector3 toReturn = Vector3.zero;

        if (m_Job)
        {
            toReturn = m_Job.transform.position - transform.position;
        }

        return toReturn * power;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_DualKillFix)
            return;
        m_DualKillFix = true;
        string findTag = m_isLight ? "PhotonDark" : "PhotonLight";
        if (!m_DualHitFix && collision.gameObject.tag == findTag)
        {
            collision.gameObject.GetComponent<Photon>().hit();
            hit();
        }
    }

    private void LateUpdate()
    {
        m_DualKillFix = false;
    }

    public void hit()
    {
        if (m_DualHitFix)
        {
            Destroy(gameObject);
            return;
        }
        m_DualHitFix = true;
        if (m_home)
            m_home.m_count--;
        Destroy(gameObject);
    }

    public void Wall(Transform pos)
    {
        m_WallGo = pos;
        m_Wall = true;
        m_wallTime = 0.0f;

    }

    public void ObtainPower(Power p)
    {
        m_heldPower = true;

        m_Job = p.m_Owner.gameObject;
    }

    private void DarkAIAttackCheck()
    {
        if (m_DAI.Attack && Random.Range(0.0f, 1.0f) < 0.1f)
        {
            m_darkAttack = true;
            m_DAI.ImGoing();
        }
    }

    private Vector2 DarkAttack()
    {
        Vector3 toReturn;
        toReturn = m_DAI.m_Target.transform.position - transform.position;

        return toReturn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Home other = collision.gameObject.GetComponent<Home>();

        if (other)
        {
            if (other.m_isLight == m_isLight)
            {
                if (m_heldPower)
                {
                    m_heldPower = false;
                    other.EatPower();
                }
            }
            else
            {
                other.hit();
                m_home.m_count--;
                Destroy(gameObject);
            }
        }
    }

}
