using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public GameObject sun;


    public void timeChange()
    {
        sun.transform.Rotate(new Vector3(15, 0, 0));
    }

    void Update()
    {
        
    }
}
