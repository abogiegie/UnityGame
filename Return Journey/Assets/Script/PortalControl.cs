using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalControl : MonoBehaviour
{
    GameObject gameControl;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameControl.GetComponent<PartControl>().SetPartOver();
        }
    }
}
