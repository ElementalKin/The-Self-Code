using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotX, rotY, rotZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotX * Time.deltaTime, rotY * Time.deltaTime, rotZ * Time.deltaTime);
    }
}
