using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenProgressBar : MonoBehaviour
{
    Image Image;

    private void Awake()
    {
        Image = transform.GetComponent<Image>();
    }

    private void Update()
    {
        Image.fillAmount = Loader.GetLoadingProgress();  
    }
}
