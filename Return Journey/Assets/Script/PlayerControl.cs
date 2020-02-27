using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    private bool isAlive = true;
    public int HP = 100;
    public int power = 100;
    public float speed = 5;
    public float jumpSpeed = 200;
    private int jumpTimes = 0;
    private int maxHP;//用于控制血条显示
    private int maxPower;//用于控制能量条显示
    private bool onFloor = true;//判断玩家是否在地面上，用于控制动画
    public bool isAttack = false;//判断玩家是否正在攻击，用于控制动画
    public bool isOnFire = false;//判断玩家是否点火，用于控制能量损失的量
    private EquipmentType equipmentType = EquipmentType.None;
    private float time = 0;
    private float HP_Rate = 6;


    public GameObject fire;//火把prefab
    private GameObject talkTarget = null;//说话对象，如果是村民则调用村民的话，如果没有则会调用partcontrol中的word
    public GameObject rightHand;//武器焊点
    public GameObject gameControl;
    private new Renderer renderer;
    private new Rigidbody rigidbody;//角色刚体，用来控制玩家速度
    private Animator animator;//控制动画播放
    private Collision collision;
    public Vector3 startPosition;//玩家初始位置，能量为0后会传送到这个位置
    private Vector3 lookAt = new Vector3(0, 0, 1);//用于储存或改变player模型的朝向
    public GameObject bulletPrefab;//玩家发射的弓箭的prefab
    private Slider hpSlider;//血条
    private Slider powerSlider;//能量条
    //各种武器的对象，不用时隐藏起来
    public GameObject sword;
    public GameObject stoneSword;
    public GameObject arch;
    public GameObject smallSword;

    private void Awake()
    {
        camera = GameObject.FindWithTag("MainCamera");
        gameControl = GameObject.FindWithTag("GameController");
        foreach (GameObject ui in GameObject.FindGameObjectsWithTag("UI"))
        {
            if(ui.name== "PlayerInformation")
            {
                hpSlider = ui.GetComponentsInChildren<Slider>()[0];
                powerSlider = ui.GetComponentsInChildren<Slider>()[1];
            }
        }
    }

    private void Start()
    {
        sword.SetActive(false);
        stoneSword.SetActive(false);
        arch.SetActive(false);
        smallSword.SetActive(false);
        maxHP = HP;
        maxPower = power;
        startPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isAlive == false)//玩家死亡则不会动
        {
            return;
        }
        if(fireLight == null)//玩家的火把是否存在
        {
            isOnFire = false;
        }
        Move();
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)//鼠标不在UI上
            {
                if (equipmentType != EquipmentType.None)//如果手中有武器
                {
                    isAttack = true;
                }
            }
        }
        if (transform.position.y < -500)//不小心掉落出地图会这样，防止意外
        {
            GetDamage(100);
        }
        SetAnimator();//动画转换控制
        if (isAttack)//用来判断动画是否仍在进行
        {
            rate += Time.deltaTime;
            Attack();
        }
        time += Time.deltaTime;
        if (time > HP_Rate)//能量损失以及生命恢复
        {
            if(gameControl.GetComponent<PartControl>().part < 2)
            {
                if (gameControl.GetComponent<GameControl>().isNight && !isOnFire)//在晚上没有火把，能量损失增加
                {
                    PowerLost(6);
                }
                else
                {
                    PowerLost(1);
                }
            }
            else
            {
                PowerLost(1);
            }
            if ((float)power / (float)maxPower >= 0.6f)
            {
                if ((float)power / (float)maxPower >= 0.8f)
                    AddHP(4);
                else
                {
                    AddHP(2);
                }
            }
            else if ((float)power / (float)maxPower < 0.2f)
            {
                GetDamage(1);
            }
            time -= HP_Rate;
        }
    }

    private GameObject fireLight;//存储实例化的火把

    public void Fire()
    {
        isOnFire = !isOnFire;
        if (isOnFire)
        {
            fireLight = GameObject.Instantiate(fire, model.transform);//把火把实例化为玩家模型的子物体
            Destroy(fireLight, 90);
        }
        else
        {
            Destroy(fireLight);
        }
    }

    public void ChangeEquip(EquipmentType type)//武器的转换
    {
        switch (type)
        {
            case EquipmentType.None:
                sword.SetActive(false);
                stoneSword.SetActive(false);
                arch.SetActive(false);
                smallSword.SetActive(false);
                equipmentType = EquipmentType.None;
                break;
            case EquipmentType.Sword:
                sword.SetActive(true);
                stoneSword.SetActive(false);
                arch.SetActive(false);
                smallSword.SetActive(false);
                equipmentType = EquipmentType.Sword;
                break;
            case EquipmentType.StoneSword:
                sword.SetActive(false);
                stoneSword.SetActive(true);
                arch.SetActive(false);
                smallSword.SetActive(false);
                equipmentType = EquipmentType.StoneSword;
                break;
            case EquipmentType.Arch:
                sword.SetActive(false);
                stoneSword.SetActive(false);
                arch.SetActive(true);
                smallSword.SetActive(false);
                equipmentType = EquipmentType.Arch;
                break;
            case EquipmentType.SmallSword:
                sword.SetActive(false);
                stoneSword.SetActive(false);
                arch.SetActive(false);
                smallSword.SetActive(true);
                equipmentType = EquipmentType.SmallSword;
                break;
        }
    }

    public EquipmentType GetEquipmentType()//返回玩家所拿着的武器的类型
    {
        return equipmentType;
    }

    private void SetAnimator()
    {
        Vector3 velH = rigidbody.velocity;//水平速度
        velH.y = 0;
        if (isAttack)
        {
            animator.SetTrigger("Default");
        }
        else if (rigidbody.velocity.y  > 0.02f && onFloor == false)//如果在上升
        {
            animator.SetTrigger("Jump1");
        }
        else if(rigidbody.velocity.y < -0.02f && onFloor == false)//如果在下降
        {
            animator.SetTrigger("Falling");
        }
        else if (velH.magnitude >= 0.02f)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    public new GameObject camera;//相机
    public GameObject model;//玩家人物的模型

    private Vector3 SelfToWorld(Vector3 vector)//从自身坐标系转换为世界坐标系
    {
        Vector3 changedVector = new Vector3(0,vector.y,0);
        float angle = camera.transform.eulerAngles.y;
        angle = Mathf.PI / 180 * angle;
        changedVector.x = vector.x * Mathf.Cos(angle) + vector.z * Mathf.Sin(angle);
        changedVector.z = -vector.x * Mathf.Sin(angle) + vector.z * Mathf.Cos(angle);
        return changedVector;
    }

    private void Move()//玩家移动，根据相机的角度来决定移动方向
    {
        transform.eulerAngles = camera.transform.eulerAngles;
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.velocity += SelfToWorld(Vector3.forward).normalized * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.velocity += SelfToWorld(Vector3.back).normalized * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.velocity += SelfToWorld(Vector3.left).normalized * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.velocity += SelfToWorld(Vector3.right).normalized * speed;
        }
        Vector3 temp = rigidbody.velocity;
        temp.y = 0;
        if (temp.magnitude > 0.02f)
        {
            if (temp.magnitude > speed)//限速，因为同时按下wsad中的两个玩家的速度会变成两倍
            {
                temp.Normalize();
                temp *= speed;
                rigidbody.velocity = new Vector3(temp.x, rigidbody.velocity.y, temp.z);
            }
            lookAt = temp * 1000 + transform.position;//改变玩家model的朝向
            model.transform.LookAt(lookAt, Vector3.up);
        }
        else
        {
            lookAt = SelfToWorld(Vector3.forward) * 1000 + transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpTimes < 2)//空格键跳跃，有二段跳
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, rigidbody.velocity.z);
            jumpTimes++;
        }
    }

    public void NextWord()//读取对应对象的下一句话
    {
        if(this.gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController");
        }
        if(talkTarget == null)
        {
            gameControl.GetComponent<PartControl>().ShowWord();
        }
        else if (talkTarget.transform.tag == "Villager")
        {
            talkTarget.GetComponent<VillagerControl>().Talk();
        }
        else 
        {
            gameControl.GetComponent<PartControl>().ShowWord();
        }
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        hpSlider.value = (float)HP / (float)maxHP;
        if (HP <= 0)
        {
            Die();
        }
        else if (HP > maxHP)
        {
            HP = maxHP;
        }
    }

    public void PowerLost(int value)
    {
        power -= value;
        powerSlider.value = (float)power / (float)maxPower;
        if (power <= 0)
        {
            GoBackHome();
        }
        else if (power > maxPower)
        {
            power = maxPower;
        }
    }

    private void GoBackHome()//能量全部消失后会直接回家并提醒玩家睡觉
    {
        this.transform.position = startPosition;
        power += 5;
        gameControl.GetComponent<GameControl>().Prompt("你已经很累了，快回去飞船睡觉吧");
    }

    public void Return()
    {
        this.transform.position = startPosition;
    }

    public float attackRate;
    public float rate = 0;

    private void Attack()//攻击，有自动瞄准功能
    {
        if (enemyList.Count > 0)//发现敌人会自动瞄准
        {
            GameObject nearestEnemy = enemyList[0];
            float minDis = 9999.0f;
            foreach (GameObject i in enemyList)
            {
                if(i == null)
                {
                    enemyList.Remove(i);
                    goto attack;
                }
                float dis = (i.transform.position - transform.position).magnitude;
                if (minDis > dis)
                {
                    minDis = dis;
                    nearestEnemy = i;
                }
            }
            lookAt = nearestEnemy.transform.position;//玩家model会朝向最近的敌人
            lookAt.y = model.transform.position.y;
        }
        model.transform.LookAt(lookAt, Vector3.up);
        lookAt = model.transform.forward;
    attack:
        if (rate >= attackRate)//攻击时间结束后停止攻击，并停止播放动画
        {
            rate = 0;
            if (equipmentType == EquipmentType.Arch)
            {
                if (gameControl.GetComponent<GameControl>().backpack.GetComponent<Backpack>().DeleteGood("Arrow"))
                {
                    GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position + lookAt.normalized * 0.5f, transform.rotation);
                    bullet.GetComponent<BulletControl>().SetVelocity(lookAt.normalized);
                }
            }
            isAttack = false;
        }
    }

    private void Die()//玩家死亡，调用gameover提示
    {
        if (isAlive)
        {
            isAlive = false;
            gameControl.GetComponent<PartControl>().GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)//触碰到地板可以再次跳跃
    {
        if (collision.transform.tag == "Floor")
        {
            jumpTimes = 0;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Villager")//获得对话对象
        {
            talkTarget = collision.gameObject;
        }
        else if(collision.transform.tag == "Floor")//用于动画转换
        { 
            onFloor = true;
        }
        else if (collision.transform.tag == "Building")//防止玩家爬着房子进入到奇怪的地方
        {
            jumpTimes = 2;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Villager")
        {
            talkTarget = null;
        }
        if (collision.transform.tag == "Floor")//用于动画转换
        {
            onFloor = false;
        }
    }

    public void AddHP(int hp)//增加血量，使用药水的时候会调用
    {
        GetDamage(-hp);
    }

    public void AddPower(int power)//增加能量，使用药水的时候会调用
    {
        PowerLost(-power);
    }

    public List<GameObject> enemyList= new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")//发现敌人则将之增加到list中，用于自动瞄准
        {
            enemyList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)//从list中移除敌人，取消对其的自动瞄准
    {
        if(other.transform.tag == "Enemy")
        {
            enemyList.Remove(other.gameObject);
        }
    }
}
public enum EquipmentType
{
    None,
    Sword,
    StoneSword,
    SmallSword,
    Arch
}
