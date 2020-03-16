using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSett : MonoBehaviour
{
    [Range(0,60)]
    public int TargetFPS = 30;

    private void Awake()
    {
        Application.targetFrameRate = TargetFPS;
        Debug.Log("Test Unity Crash 42");

    }

    
}
