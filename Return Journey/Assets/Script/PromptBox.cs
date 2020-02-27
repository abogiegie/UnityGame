using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptBox : MonoBehaviour
{
    public float liveTime;//存在时间
    private float time;
    private Color color;

    public GameObject canvas;

    private void Awake()
    {
        color = this.GetComponent<Image>().color;
    }

    public void SetText(string content)
    {
        this.GetComponentInChildren<Text>().text = content;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Color tempColor = color;
        tempColor.a = color.a * (1 - (time / liveTime));
        this.GetComponent<Image>().color = tempColor;
        if (time >= liveTime)
        {

            Destroy(this.gameObject);
        }
    }
}
