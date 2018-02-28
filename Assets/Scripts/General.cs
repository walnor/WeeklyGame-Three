using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour {

    public static
    void colorCorrection(GameObject gb, bool isLight)
    {
        if (isLight)
        {
            gb.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            float c = (1.0f / 255.0f) * 45.0f;

            gb.GetComponent<SpriteRenderer>().color = new Color(c, c, c);
        }
    }

    public static GameObject GetNearestGameObject(GameObject sourceGameObject, string tag, float maxDistance = float.MaxValue)
    {
        GameObject nearest = null;

        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag(tag);

        if (gameObjects.Length > 0)
        {
            float nearestDistance = float.MaxValue;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != sourceGameObject)
                {
                    float distance = (sourceGameObject.transform.position - gameObjects[i].transform.position).magnitude;
                    if (distance < nearestDistance)
                    {
                        nearest = gameObjects[i];
                        nearestDistance = distance;
                    }
                }
            }

            if (nearestDistance > maxDistance) nearest = null;
        }

        return nearest;
    }


    public static GameObject[] GetGameObjects(GameObject sourceGameObject, string tag, int MaxToGet, float maxDistance = float.MaxValue)
    {
        List<GameObject> returnGameObjects = new List<GameObject>();

        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != sourceGameObject)
            {
                float distance = (sourceGameObject.transform.position - gameObjects[i].transform.position).magnitude;
                if (distance < maxDistance)
                {
                    returnGameObjects.Add(gameObjects[i]);
                }
            }

            if (gameObjects.Length >= MaxToGet)
                break;
        }
        return returnGameObjects.ToArray();
    }
}
