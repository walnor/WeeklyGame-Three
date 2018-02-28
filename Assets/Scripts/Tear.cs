using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour {

    [SerializeField] GameObject m_power;

    int m_count = 0;
    [SerializeField] int m_MaxCount = 10;
    [SerializeField] float m_SpawnRate = 1.0f;

    float m_timer = 0.0f;
    float range = 0.4f;
    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_SpawnRate)
        {
            if(m_count < m_MaxCount)
            {
                GameObject gb = Instantiate(m_power);


                float x = Random.Range(-range, range);
                float y = Random.Range(-range, range);

                gb.transform.position = transform.position + new Vector3(x, y, 0.0f);
                gb.GetComponent<Power>().m_Owner = this;
                m_count++;
            }
            m_timer = 0.0f;
        }
    }

    public void Taken()
    {
        m_count--;
    }
}
