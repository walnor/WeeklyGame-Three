using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkAI : MonoBehaviour {

    [SerializeField] int AttackWaveMax = 25;

    [SerializeField] float AttackTimeout = 10.0f;

    [SerializeField] [Range(0.0f,1.0f)] float AttackChance = 0.5f;

    int CurrentAttackWaveCount = 0;

    public bool Attack = false;

    public GameObject m_Target;

    float m_timer = 0.0f;
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= AttackTimeout)
        {
            m_timer = 0.0f;

            if (!Attack && Random.Range(0.0f, 1.0f) < AttackChance)
            {
                Attack = true;
                print("ATTACK!");
                if (Random.Range(0.0f, 1.0f) < AttackChance/2)
                {
                    print("Plus Ultra");
                    CurrentAttackWaveCount -= AttackWaveMax * 5;
                }
            }
        }		
	}

    public void ImGoing()
    {
        CurrentAttackWaveCount++;

        if (CurrentAttackWaveCount >= AttackWaveMax)
        {
            Attack = false;
            CurrentAttackWaveCount = 0;
        }
    }
}