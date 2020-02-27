using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstEnemy : MonoBehaviour
{
    private float speed = 0;
    private Rigidbody enemyRigibody;
    private Vector3 doorPosition = new Vector3(40, 0, 20);
    private Slider sliderHP;
    private int damage = 5;//怪物伤害
    private bool haveTalk = false;//判断是否开始对话
    public float attackTime = 0;
    private float attackRate = 1;

    public GameObject ironOre;
    private GameObject gameControl;

    private void Awake()
    {
         gameControl = GameObject.FindWithTag("GameController");
         //gameControl = GameObject.FindWithTag("IronOre");
    }

    void Start()
    {
        enemyRigibody = this.GetComponent<Rigidbody>();
        sliderHP = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if (transform.position.y > 1)
        {
            enemyRigibody.velocity = new Vector3(enemyRigibody.velocity.x, -1, enemyRigibody.velocity.z);
        }
    }

    private void Attack(GameObject attackTarget)
    {
        if (attackTarget.tag == "Door")
        {
            this.GetComponentInChildren<MushroomMon_Ani_Test>().AttackAni();
        }
        else if(attackTarget.tag == "Player")
        {
            attackTarget.GetComponent<PlayerControl>().GetDamage(damage);
            this.GetComponentInChildren<MushroomMon_Ani_Test>().AttackAni();
        }
        this.GetComponentInChildren<MushroomMon_Ani_Test>().IdleAni();
    }

    private void Talk(string talkContent)
    {
        Text talkText = gameControl.GetComponent<GameControl>().talk.GetComponentInChildren<Text>();
        gameControl.GetComponent<GameControl>().OpenTalk();
        talkText.text = talkContent;
    }

    public void Die()
    {
        GameObject.Instantiate(ironOre, transform.position, transform.rotation);
        Destroy(this.gameObject, 2);
        this.GetComponentInChildren<MushroomMon_Ani_Test>().DeathAni();
        Talk("宁肯普：终于打倒了这个怪物，他身上好像掉落了什么东西，靠近物体点击鼠标右键可以拾取物品");
        gameControl.GetComponent<PartControl>().word = new string[]
        {
            "卫兵:欢迎来到我们村子，不过看你的样子不太像本地人，如果有什么需要帮助的话就去找我们村长吧，他现在在村子里面。",
            "宁肯普：好的，正好我现在有点小麻烦"
        };
    }

    public void SetSpeed(int s)
    {
        speed = s;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Meat")
        {
            gameControl.GetComponent<GameControl>().Prompt("它已经被你扔的那块肉吸引住了，不过这块肉好像并不能吸引他太久，赶快跑到西边的村子，他们可能会帮助你对付这怪物");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Meat")
        {
            Vector3 temp = (other.transform.position - this.transform.position).normalized * speed;
            temp.y = enemyRigibody.velocity.y;
            enemyRigibody.velocity = temp;
            SetSpeed(3);
            if (speed > 0.2f)
                this.transform.LookAt(this.transform.position + temp);
            if ((other.transform.position - this.transform.position).magnitude <= 1)
            {
                Destroy(other.gameObject, 2);
                SetSpeed(5);
            }
        }
        else if (other.transform.tag == "Door")
        {
            Vector3 temp = (other.transform.position - this.transform.position).normalized * speed;
            temp.y = enemyRigibody.velocity.y;
            enemyRigibody.velocity = temp;
            if (speed > 0.2f)
                this.transform.LookAt(this.transform.position + temp);
        }
        else if(other.transform.tag == "Player")
        {
            Vector3 temp = (other.transform.position - this.transform.position).normalized * speed;
            temp.y = enemyRigibody.velocity.y;
            enemyRigibody.velocity = temp;
            if (speed > 0.2f)
                this.transform.LookAt(this.transform.position + temp);
            if (haveTalk)
            {
                if ((transform.position - other.transform.position).magnitude > 10)
                {
                    SetSpeed(6);
                }
                else
                {
                    SetSpeed(5);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.transform.tag == "Door")
        {
            attackTime += Time.deltaTime;
            if(attackTime > attackRate)
            {
                Attack(collision.gameObject);
                attackTime = 0;
            }
        }
        else if (collision.transform.tag == "Player")
        {
            attackTime += Time.deltaTime;
            if (attackTime > attackRate)
            {
                Attack(collision.gameObject);
                attackTime = 0;
            }
            if(haveTalk == false)
            {
                gameControl.GetComponent<GameControl>().Prompt("你快要被怪物追上了，快按快捷键B打开背包，扔下背包里面的肉来吸引他的注意力");
                SetSpeed(5);
                haveTalk = true;
            }
        }
    }
}
