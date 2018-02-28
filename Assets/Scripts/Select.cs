using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

    Vector3 MousePosition;

    public bool isActive = true;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            isActive = true;
        }
        else
            isActive = false;

        MousePosition = Input.mousePosition;
        MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);
        MousePosition.z = 0.0f;

        transform.position = MousePosition;

        if (isActive)
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
		
	}
}
