using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage = 10;
    public GameObject playerControl;
    public GameObject swordHitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            if (playerControl.GetComponent<PlayerControl>().isAttack && (this.transform.position - other.transform.position).magnitude <= 5)
            {
                other.GetComponent<EnemyControl>().GetDamage(damage);
                GameObject effect = GameObject.Instantiate(swordHitEffect,other.transform);
                effect.transform.LookAt(playerControl.transform.position);
                Destroy(effect, 1);
            }
        }
        if (other.transform.tag == "Animal")
        {
            if (playerControl.GetComponent<PlayerControl>().isAttack)
            {
                other.GetComponent<AnimalControl>().GetDamage(damage);
                GameObject effect = GameObject.Instantiate(swordHitEffect,other.transform);
                effect.transform.LookAt(playerControl.transform.position);
                Destroy(effect, 1);
            } 
        }
        if (other.transform.tag == "Tree")
        {
            if (playerControl.GetComponent<PlayerControl>().isAttack)
            {
                if (other.GetComponent<TreeControl>().MakeWood() <= 0)
                {
                    playerControl.GetComponent<PlayerControl>().gameControl.GetComponent<GameControl>().Prompt("为了可持续发展，请采集其他树木", false);
                }
            }
        }
        if(other.transform.tag == "FirstEnemy")
        {
            if (playerControl.GetComponent<PlayerControl>().isAttack && (this.transform.position - other.transform.position).magnitude <= 5)
            {
                other.GetComponent<FirstEnemy>().Die();
            }
        }
    }


}
