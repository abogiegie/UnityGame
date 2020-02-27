using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalControl : MonoBehaviour
{
    public GameObject meat;
    private Rigidbody animalRigidbody;
    private bool isBeingAttack = false;
    float time = 0;
    float stopTime = 5;
    public float speed = 1.5f;
    public int HP = 50;
    private Vector3 vel;//行走方向
    int i = 0;

    private void Start()
    {
        this.animalRigidbody = this.GetComponent<Rigidbody>();
        this.animalRigidbody.velocity = Vector3.forward * speed;
        this.GetDamage(0, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingAttack == false)
        {
            i++;
            if (i > (int)(50 + 65 * Random.value))//随机改变移动方向
            {
                vel = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1);
                vel.Normalize();
                vel = vel * speed;
                i = 0;
            }
            animalRigidbody.velocity = new Vector3(vel.x, animalRigidbody.velocity.y, vel.z);
            this.transform.LookAt(transform.position + vel);
        }
        else
        {
            time += Time.deltaTime;
            if (time <= stopTime)
            {
                animalRigidbody.velocity = new Vector3(vel.x, animalRigidbody.velocity.y, vel.z);
                this.transform.LookAt(transform.position + vel);
            }
            else
            {
                time = 0;
                isBeingAttack = false;
            }
        }
        if (animalRigidbody.velocity.magnitude > speed * 10)//速度过快
        {
            animalRigidbody.velocity /= 2;
        }
        if (this.transform.position.y < -3)
        {
            Die();
        }
    }

    public void GetDamage(int damage, float runTime = 0)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            isBeingAttack = true;
            vel = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1);
            vel.Normalize();
            vel = vel * speed * 2;//两倍速逃跑
            time -= runTime;
        }
    }

    private void Die()
    {
        GameObject newmeat = GameObject.Instantiate(meat, transform.position, transform.rotation);
        newmeat.transform.position += Vector3.up * 0.1f;
        Destroy(newmeat, 60);//一分钟之内删除
        this.transform.parent.GetComponent<AnimalMaker>().AnimalDie();
        Destroy(this.gameObject);
    }
}
