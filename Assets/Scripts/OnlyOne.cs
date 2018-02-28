using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOne : MonoBehaviour {
    [SerializeField] int ID = 0;

    private void Start()
    {
        GameObject[] other = GameObject.FindGameObjectsWithTag("onlyOne");

        foreach (GameObject obj in other)
        {
            if (obj != gameObject)
            {
                if (obj.GetComponent<OnlyOne>().ID == ID)
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }
        
    }
}
