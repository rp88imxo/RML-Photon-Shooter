using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float intensity;
    public float smooth;

    Quaternion originRotation;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.localRotation;
    }

    
    void updateSway()
    {

        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        Quaternion txRot = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
        Quaternion tyRot = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);

        Quaternion targetRotation = originRotation * (txRot * tyRot);

        transform.localRotation = 
            Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);


    }
    // Update is called once per frame
    void Update()
    {
        updateSway();
    }
}
