using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    bool m_isIncreasing = true;
    bool m_dualTakenFix = false;

    float m_lerpAmount = 0.0f;

    [SerializeField] Color m_LightColor;
    [SerializeField] Color m_DarkColor;

    SpriteRenderer m_spRender;

    public Tear m_Owner;

    // Use this for initialization
    void Start () {
        m_spRender = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (m_isIncreasing)
        {
            m_lerpAmount += Time.deltaTime;
            if (m_lerpAmount >= 1.0f)
                m_isIncreasing = !m_isIncreasing;
        }
        else
        {
            m_lerpAmount -= Time.deltaTime;
            if (m_lerpAmount <= 0.0f)
                m_isIncreasing = !m_isIncreasing;
        }

        m_spRender.color = Color.Lerp(m_LightColor, m_DarkColor, m_lerpAmount);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "PhotonLight" || other.tag == "PhotonDark")
        {
            Photon ph = other.GetComponent<Photon>();
            if (!ph.m_heldPower && !m_dualTakenFix)
            {
                m_dualTakenFix = true;
                ph.ObtainPower(this);
                m_Owner.Taken();
                Destroy(gameObject);
            }
        }
    }
}
