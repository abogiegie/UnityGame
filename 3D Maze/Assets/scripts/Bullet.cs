using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 7.7f;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if(collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().GetDamage(1);
            Destroy(gameObject, 0.5f);
        }
    }
}
