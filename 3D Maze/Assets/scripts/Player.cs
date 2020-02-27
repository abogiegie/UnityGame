using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private GameObject gameManager;
    Vector3 birthPlace;
    public GameObject bullet;

    Rigidbody mRigidbody;

    private List<Image> HPUI = new List<Image>();

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        GameObject HP_UI = GameObject.Find("HP");
        mRigidbody = gameObject.GetComponent<Rigidbody>();
        foreach(Image image in HP_UI.GetComponentsInChildren<Image>())
        {
            HPUI.Add(image);
        }
    }

    private float mouseSpeed = 100.0f;
    private const float speed = 1.5f;

    private int hp = 5;
    private int bulletCount = -1;//-1代表无限

    void Start()
    {
        MapMaker mapMaker = gameManager.GetComponent<MapMaker>();
        float center = mapMaker.GetCenter();
        transform.position = mapMaker.startPlace;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        

        Cursor.lockState = CursorLockMode.Locked;//鼠标锁定并消失
        Cursor.lockState = CursorLockMode.Confined;//鼠标限制在游戏中
        Cursor.visible = false;
    }


    void Update()
    {
        if (isAlive)
        {
            Move();
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.GetComponent<GameOver>().ShowMenu();
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Cursor.visible = !Cursor.visible;
        }
    }

    private void Move()
    {
        mRigidbody.velocity *= 0.9f;
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            mRigidbody.velocity = (transform.localToWorldMatrix * Vector3.forward * Input.GetAxis("Vertical")).normalized * speed;
            mRigidbody.velocity += (Vector3)(transform.localToWorldMatrix * Vector3.right * Input.GetAxis("Horizontal")).normalized * speed;
            //transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * speed, Space.Self);
            //transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * speed, Space.Self);
            mRigidbody.velocity = mRigidbody.velocity.normalized * speed;
        }
        if (!Cursor.visible)
        {
            float angleHorizontal = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            float angleVertical = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, angleHorizontal, Space.Self);
            transform.Rotate(Vector3.left, angleVertical, Space.Self);
            transform.Rotate(Vector3.forward, -transform.localEulerAngles.z, Space.Self);
        }
    }

    private bool isAlive = true;

    public void GetDamage()
    {
        hp--;
        if(hp >= 0)
            HPUI[hp].color = Color.gray;
        if (hp <= 0)
        {
            isAlive = false;
            gameManager.GetComponent<GameOver>().Defeat();
        }
    }

    private const float attackRate = 0.5f;
    private float rate = attackRate;

    private void Attack()
    {
        if (rate < attackRate)
            rate += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            Debug.Log("isOnGameObject" + EventSystem.current.IsPointerOverGameObject());
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (rate >= attackRate)
                {
                    GameObject gameObject = Instantiate(bullet, transform.position, transform.rotation);
                    if (bulletCount != -1)
                    {
                        bulletCount--;
                    }
                    rate = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Treasure")
        {
            gameManager.GetComponent<GameOver>().Win();
        }
    }
}
