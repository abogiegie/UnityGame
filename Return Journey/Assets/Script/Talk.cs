using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    PlayerControl player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }

    public void NextWord()
    {
        player.NextWord();
    }
}
