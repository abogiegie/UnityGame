using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public const string ATTACK = "Arrow Attack";

    private Animator animator;
    public GameObject arrow;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Attack(GameObject target)
    {
        transform.LookAt(target.transform.position);
        animator.SetTrigger(ATTACK);
        GameObject a = GameObject.Instantiate(arrow, transform.position, transform.rotation);
        int damage = transform.parent.GetComponent<GuardControl>().damage;
        a.GetComponent<BulletControl>().damage = damage;
        a.GetComponent<BulletControl>().bulletSpeed = 0.6f;
        a.GetComponent<BulletControl>().SetVelocity((target.transform.position-transform.position).normalized);
    }

}
