using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] Transform m_Pos = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Hello");

        Photon other = collision.gameObject.GetComponent<Photon>();

        if (other)
        {
            other.Wall(m_Pos);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Photon other = collision.gameObject.GetComponent<Photon>();

        if (other)
        {
            other.Wall(m_Pos);
        }

    }
}
