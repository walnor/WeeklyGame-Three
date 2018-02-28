using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTracker : MonoBehaviour {

    [SerializeField] Text m_text;
    [SerializeField] Home m_Home;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_Home)
        {
            Destroy(gameObject);
        }
        m_text.text = m_Home.m_Life + "/" + m_Home.m_MaxLife;
		
	}
}
