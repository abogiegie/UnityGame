using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hp = 10;

    private readonly float speed = 1.0f;
    private State state = State.NotFound;

    private GameObject gameManager;
    private GameObject target;
    private Rigidbody mRigidbody;

    public void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        gameManager.GetComponent<EnemyMaker>().DelEnemy(gameObject);
        Destroy(gameObject);
    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        mRigidbody = gameObject.GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    readonly List<Vector3> vList = new List<Vector3>();

    private void Start()
    {
        vList.Add(Vector3.forward);
        vList.Add((Vector3.forward + Vector3.left).normalized);
        vList.Add((Vector3.forward + Vector3.right).normalized);
        vList.Add((Vector3.forward + Vector3.up).normalized);
        vList.Add((Vector3.forward + Vector3.down).normalized);

        mRigidbody.velocity = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
        mRigidbody.velocity = mRigidbody.velocity.normalized * speed;
        transform.LookAt(transform.position + mRigidbody.velocity);
    }

    const float distance = 0.8f;
    const float turnRate = 0.8f;
    private float tRate = 0;

    private void Update()
    {
        if(state == State.NotFound)
        {
            tRate += Time.deltaTime;
            if(tRate >= turnRate)
            {
                mRigidbody.velocity += new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 0.3f;
                mRigidbody.velocity = mRigidbody.velocity.normalized * speed;
                transform.LookAt(transform.position + mRigidbody.velocity);
                tRate = 0;
            }
        }
        else if (target != null)
        {
            transform.LookAt(target.transform.position);
            mRigidbody.velocity = (target.transform.position - transform.position).normalized * speed;
            if(aRate <= attackRate)
            {
                aRate += Time.deltaTime;
            }
        }

        List<bool> rayCast = new List<bool>();
        foreach (Vector3 v in vList)
        {
            rayCast.Add(Physics.Raycast(transform.position, transform.localToWorldMatrix * v, distance, LayerMask.GetMask("Wall")));
        }
        for (int i = 0, count = rayCast.Count; i < count; i++)
        {
            if (rayCast[i])
            {
                mRigidbody.velocity -= (Vector3)(transform.localToWorldMatrix * vList[i]) * (speed / 8);
                mRigidbody.velocity = mRigidbody.velocity.normalized * speed;
                transform.LookAt(transform.position + mRigidbody.velocity);
            }
        }
    }

    private const float attackRate = 1.0f;
    private float aRate = attackRate;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            if(aRate>= attackRate)
            {
                collision.transform.GetComponent<Player>().GetDamage();
                aRate = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            state = State.Found;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            state = State.NotFound;
        }
    }
}

enum State
{
    NotFound,
    Found
}
