using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour {

    [SerializeField] GameObject m_Photon;
    [SerializeField] GameObject[] SpawnPoints;

    public bool m_isLight = true;

    [SerializeField] float m_spawnRate = 1.0f;

    public int m_count = 0;
    public int m_MaxNum = 100;

    float m_timer = 0.0f;

    public int m_Life = 100;
    public int m_MaxLife = 100;
	// Use this for initialization
	void Start () {
        General.colorCorrection(gameObject, m_isLight);
    }

    // Update is called once per frame
    void Update ()
    {
        m_timer += Time.deltaTime;

        float absMaxCount = m_MaxNum * 2;

        float spawnTimerAdj = 9.0f * (1.0f - ((absMaxCount - m_count) / absMaxCount));

        if (m_timer >= (m_spawnRate + spawnTimerAdj))
        {
            m_timer = 0.0f;

            if (m_count < m_MaxNum)
            {
                SpawnPhoton();
                m_count++;
            }
        }		
	}

    private void SpawnPhoton()
    {
        GameObject gb = Instantiate(m_Photon);
        gb.GetComponent<Photon>().m_isLight = m_isLight;
        gb.GetComponent<Photon>().m_home = this;

        if (m_isLight)
        {
            gb.tag = "PhotonLight";
        }
        else
        {
            gb.tag = "PhotonDark";
        }

        int ranIndex = (int)UnityEngine.Random.Range(0.0f, SpawnPoints.Length);
        if (ranIndex == SpawnPoints.Length)
        {
            ranIndex--;
        }

        gb.transform.position = SpawnPoints[ranIndex].transform.position;
    }

    public void EatPower()
    {
        if (m_count <= (m_MaxNum * 2))
        {
            SpawnPhoton();
            m_count++;
        }

        if (m_Life < m_MaxLife && UnityEngine.Random.Range(0.0f, 100.0f) > 98.0f)
        {
            m_Life++;
        }
        //to do Eat power from photon
    }
    public void hit()
    {
        m_Life--;

        if (m_Life <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0.2f);

        Destroy(this);
    }
}
