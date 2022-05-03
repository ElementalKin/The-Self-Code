using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float panSpeed;
    public float PanBorderThickness = 10f;
    public float negxLimit;
    public float posxLimit;

    public float negzLimit;
    public float poszLimit;



    public float minY = 5f;
    public float maxY = 40f;

    public float scrollspeed = 20f;
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if(Input.GetKey("w")/* || Input.mousePosition.y >= Screen.height - PanBorderThickness*/)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") /*|| Input.mousePosition.y <= PanBorderThickness*/)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") /*|| Input.mousePosition.x >= Screen.width - PanBorderThickness*/)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") /*|| Input.mousePosition.x <= PanBorderThickness*/)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollspeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, negxLimit, posxLimit);

        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pos.z = Mathf.Clamp(pos.z, negzLimit, poszLimit);
        transform.position = pos;
    }
}
