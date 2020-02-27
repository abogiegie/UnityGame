using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeedChange : MonoBehaviour
{
    GameObject mainCamera = null;

    public void ChangeMouseSpeed()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindWithTag("MainCamera");
        }
        mainCamera.GetComponent<Camera>().mouseSpeed = this.GetComponent<Slider>().value * 100;
        Debug.Log(this.GetComponent<Slider>().value);
    }
}
