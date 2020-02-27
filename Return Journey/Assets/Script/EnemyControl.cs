using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    private bool isAlive = true;
    public int HP;
    private int maxHP;
    public float speed;//移动速度
    public int damage;//攻击力
    private bool foundPlayer = false;
    private bool foundVillager = false;
    private bool getTarget = false;//进攻的敌人有没有到达目的地
    public EnemyType type = EnemyType.intruder;

    public float attackRate;
    private float time = 0;
    private float time1 = 0;//回血时间
    public float backFlowHPRate = 1;
    private Vector3 vel;
    private int i = 0;

    private Rigidbody enemyRigibody;

    private Slider sliderHP;
    public GameObject canvas;
    public Vector3 tarPos;

    public GameObject ironOre;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;
        sliderHP = this.GetComponentInChildren<Slider>();
        enemyRigibody = this.GetComponent<Rigidbody>();
        canvas = this.GetComponentInChildren<Canvas>().gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (foundPlayer == false)
        {
            if (type == EnemyType.patrol)//巡逻兵
            {
                i++;
                if (i > 50)//50帧改变移动方向一次
                {
                    vel = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1);
                    vel.Normalize();
                    vel *= speed;
                    i = 0;
                }
                enemyRigibody.velocity = new Vector3(vel.x, enemyRigibody.velocity.y, vel.z);
                transform.LookAt(transform.position + enemyRigibody.velocity * 1000);
            }
            else if (type == EnemyType.intruder)//入侵者
            {
                if (getTarget == false && (tarPos - this.transform.position).magnitude > 2)
                {
                    enemyRigibody.velocity = (tarPos - this.transform.position).normalized * speed;
                    transform.LookAt(transform.position + enemyRigibody.velocity * 1000);
                    if (getTarget)
                    {
                        type = EnemyType.patrol;
                    }
                }
                else
                {
                    getTarget = true;
                    this.type = EnemyType.patrol;
                }
            }
            this.transform.LookAt(transform.position + vel);
        }
        if (HP < maxHP)
        {
            time1 += Time.deltaTime;
            if (time1 > backFlowHPRate)
            {
                HP += 3;
                ChangeSlider();
                time1--;
            }
        }
        if (time <= attackRate)
        {
            time += Time.deltaTime;
        }
    }

    public void ChangeHP(int hp)
    {
        HP = hp;
        maxHP = HP;
    }

    public void AttackVillage()
    {
        type = EnemyType.intruder;
        getTarget = false;
    }

    private void Die()
    {
        HideCanvas();
        if(this.name != "Boss")
        {
            this.GetComponentInChildren<MushroomMon_Ani_Test>().DeathAni();
        }
        else
        {
            this.GetComponentInChildren<Animation_Test>().DeathAni();
        }
        if (this.name != "Boss")
        {
            if (Random.value > 0.5)//50%几率掉落铁矿，在第三关会掉落苹果
            {
                GameObject.Instantiate(ironOre, this.transform.position, this.transform.rotation);
            }
            if (this.transform.parent.name == "EnemyMaker")
            {
                this.transform.parent.GetComponent<EnemyMaker>().DeleteEnemyInList(this.gameObject);
            }
        }
        else
        {
            this.GetComponent<BossControl>().DestroyWall();
        }
        Destroy(this.gameObject,2);
    }
    public void GetDamage(int damage)
    {
        if(this.name != "Boss")
        {
            this.GetComponentInChildren<MushroomMon_Ani_Test>().DamageAni();
        }
        else
        {
            this.GetComponentInChildren<Animation_Test>().DamageAni();
        }

        this.HP -= damage;
        ChangeSlider();
        if (HP <= 0)
        {
            if (isAlive)
            {
                isAlive = false;
                Die();
            }
        }
    }
    private void ChangeSlider()
    {
        sliderHP.value = (float)HP / (float)maxHP;
    }

    private void Attack(GameObject target)
    {
        if (time > attackRate)
        {
            if(this.name != "Boss")
            {
                this.GetComponentInChildren<MushroomMon_Ani_Test>().AttackAni();
            }
            else
            {
                this.GetComponentInChildren<Animation_Test>().AttackAni();
            }
            if (target.transform.tag == "Player")
            {
                target.gameObject.GetComponent<PlayerControl>().GetDamage(damage);
            }
            else if(target.transform.tag == "Door")
            {
                target.gameObject.GetComponent<DoorControl>().Fix(-damage);//反向修理
            }
            time -= attackRate;
        }
    }

    private void HideCanvas()
    {
        if (foundPlayer == false && foundVillager == false)
        {
            canvas.SetActive(false);
        }
        else if (isAlive == false)//已经死了
        {
            canvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            foundPlayer = true;
            canvas.SetActive(true);
        }
        if (other.transform.tag == "Door")
        {
            foundVillager = true;
            canvas.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (((float)HP / (float)maxHP) > 0.2)
            {
                Vector3 temp = (other.transform.position - transform.position).normalized * speed;
                temp.y = enemyRigibody.velocity.y;
                enemyRigibody.velocity = temp;
                transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));
            }
            else if(this.name != "Boss")
            {
                Vector3 temp = (other.transform.position - transform.position).normalized * speed * -1;
                temp.y = enemyRigibody.velocity.y;
                enemyRigibody.velocity = temp;
                transform.LookAt(new Vector3(other.transform.position.x * -1, transform.position.y, other.transform.position.z * -1));
                this.GetComponentInChildren<MushroomMon_Ani_Test>().RunAni();
            }
            else 
            {
                Vector3 temp = (other.transform.position - transform.position).normalized * (speed + 1);
                temp.y = enemyRigibody.velocity.y;
                enemyRigibody.velocity = temp;
                transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));
            }
        }
        else if (foundPlayer == false && other.transform.tag == "Door")
        {
            if (((float)HP / (float)maxHP) > 0.2)
            {
                Vector3 temp = (other.transform.position - transform.position).normalized * speed;
                temp.y = enemyRigibody.velocity.y;
                enemyRigibody.velocity = temp;
                transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));
            }
            else if (this.name != "Boss")
            {
                Vector3 temp = (other.transform.position - transform.position).normalized * speed * -1;
                temp.y = enemyRigibody.velocity.y;
                enemyRigibody.velocity = temp;
                transform.LookAt(new Vector3(other.transform.position.x * -1, transform.position.y, other.transform.position.z * -1));
                this.GetComponentInChildren<MushroomMon_Ani_Test>().RunAni();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            foundPlayer = false;
            HideCanvas();
        }
        if (other.transform.tag == "Door")
        {
            foundVillager = false;
            HideCanvas();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Attack(collision.gameObject);
        }
        else if(collision.transform.tag == "Door")
        {
            Attack(collision.gameObject);
        }
    }
}
public enum EnemyType
{
    patrol,
    intruder
}