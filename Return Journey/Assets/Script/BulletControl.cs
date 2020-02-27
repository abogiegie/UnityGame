using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private Vector3 dir;
    public float bulletSpeed;
    public int damage;

    public GameObject boomEffect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * bulletSpeed;
    }

    public void SetVelocity(Vector3 flyDir)
    {
        this.dir = flyDir;
        this.transform.LookAt(transform.position + dir * 1000);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")
        {
            Boom();
            other.transform.GetComponent<EnemyControl>().GetDamage(damage);
        }
        else if(other.transform.tag == "Animal")
        {
            Boom();
            other.transform.GetComponent<AnimalControl>().GetDamage(damage);
        }
        else
        {
            bulletSpeed = 0;
            Destroy(this.gameObject, 0.3f);
        }
    }

    private void Boom()
    {
        GameObject tempEffect = GameObject.Instantiate(boomEffect, transform.position, transform.rotation);
        Destroy(tempEffect, 1.5f);
        Destroy(this.gameObject);
    }
}
