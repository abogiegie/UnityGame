using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject target;
    public Vector3 cameraMove;
    public bool canMouseMove = true;
    public float mouseSpeed = 50.0f;
    public bool cursorLock = false;

    public List<Material> skyMaterial;
    private Skybox skybox;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        transform.rotation = target.transform.rotation;
        skybox = transform.GetComponent<Skybox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMouseMove)//在与村名交互或者对话时不能通过鼠标来移动视角
        {
            float angle = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);
        }
        transform.position = target.transform.position;//始终让玩家model显示在相机固定位置
        transform.Translate(cameraMove, Space.Self);
    }

    public void ChangeSkybox(SkyType type)//改变天空盒
    {
        RenderSettings.skybox = skyMaterial[(int)type];
    }
}
public enum SkyType
{
    Sunny,
    Cloudy,
    MoonNight,
    StarryNight
}