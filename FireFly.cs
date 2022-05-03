using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour
{
    GameObject sun;


    public void Start()
    {
        InvokeRepeating("CheckIfNight", 1.0f, 0.3f);
        sun = GameObject.FindGameObjectWithTag("sun");
    }


    public void CheckIfNight()
    {
        if (sun.transform.rotation.eulerAngles.x < 17 || sun.transform.rotation.eulerAngles.x > 270)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
