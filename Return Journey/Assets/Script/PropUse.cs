using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropUse : MonoBehaviour
{
    public GameObject player;
    public GameObject backpack;

    private bool isFirstUse = true;

    public int weight = 3;
    public showType type = showType.All;

    //道具使用
    public void IncreaseHP(int HP)//恢复血量
    {
        //在背包中减少物品数
        string propName = this.name;
        propName = propName.Substring(0, propName.Length - 7);
        if (backpack.GetComponent<Backpack>().DeleteGood(propName))
        {
            player.GetComponent<PlayerControl>().GetDamage(-HP);//回复生命
        }
        
    }

    public void IncreasePower(int power)//回复能量
    {
        //在背包中减少物品数
        string propName = this.name;
        propName = propName.Substring(0, propName.Length - 7);
        if (backpack.GetComponent<Backpack>().DeleteGood(propName))
        {
            player.GetComponent<PlayerControl>().PowerLost(-power);//回复能量
        }
    }

    public void Equip(string name)
    {
        switch (name)
        {
            case "IronSword":
                if (player.GetComponent<PlayerControl>().GetEquipmentType() == EquipmentType.Sword)
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.None);
                }
                else
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.Sword);
                }
                break;
            case "StoneSword":
                if (player.GetComponent<PlayerControl>().GetEquipmentType() == EquipmentType.StoneSword)
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.None);
                }
                else
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.StoneSword);
                }
                break;
            case "Arch":
                if (player.GetComponent<PlayerControl>().GetEquipmentType() == EquipmentType.Arch)
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.None);
                }
                else
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.Arch);
                }
                break;
            case "SmallSword":
                if (player.GetComponent<PlayerControl>().GetEquipmentType() == EquipmentType.SmallSword)
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.None);
                }
                else
                {
                    player.GetComponent<PlayerControl>().ChangeEquip(EquipmentType.SmallSword);
                }
                if (isFirstUse)
                {
                    PartControl partControl = player.GetComponent<PlayerControl>().gameControl.GetComponent<PartControl>();
                    partControl.word = new string[] { "点击鼠标左键可以进行攻击" };
                    partControl.ShowWord();
                }
                break;
        }
        
    }

    public void MakeFire()//制作火把
    {
        if (player.GetComponent<PlayerControl>().isOnFire)
        {
            player.GetComponent<PlayerControl>().Fire();
        }
        else
        {
            if (backpack.GetComponent<Backpack>().DeleteGood("Fire") == false)
            {
                return;
            }
            player.GetComponent<PlayerControl>().Fire();
        }
    }

    public void Drop(string name)//丢弃
    {
        backpack.GetComponent<Backpack>().DeleteGood(name);
        backpack.GetComponent<Backpack>().DropProp(name);
    }
}
