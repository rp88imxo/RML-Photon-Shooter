using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputOther : MonoBehaviour
{
    public static void ToggleCamera(Camera camera)
    {
        if (camera)
        {
            if (camera.gameObject.activeSelf)
            {
                camera.gameObject.SetActive(false);
            }
            else
            {
                camera.gameObject.SetActive(true);
            }
        }
       
    }
    
}
