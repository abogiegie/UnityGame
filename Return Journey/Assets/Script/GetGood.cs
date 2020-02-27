using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGood : MonoBehaviour
{
    public GameObject backpack;

    private void Start()
    {
        backpack = GameObject.FindWithTag("GameController").GetComponent<GameControl>().backpack;
    }

    public void MakeHPPotion()//生命药水制作
    {
        int appleCount = 1;
        int meatCount = 1;
        if (backpack.GetComponent<Backpack>().DeleteGood("Apple", appleCount) == false)//水果不足
        {
            return;
        }
        if(backpack.GetComponent<Backpack>().DeleteGood("Meat", meatCount) == false)//肉不足
        {
            backpack.GetComponent<Backpack>().AddGood("Apple", appleCount, false);
            return;
        }
        if (backpack.GetComponent<Backpack>().AddGood("HPPotion") == false)//背包已满
        {
            backpack.GetComponent<Backpack>().AddGood("Apple", appleCount, false);
            backpack.GetComponent<Backpack>().AddGood("Meat", meatCount, false);
            return;
        }
    }

    public void MakePowerPotion()//体力药水制作
    {
        int appleCount = 1;
        int meatCount = 1;
        if (backpack.GetComponent<Backpack>().DeleteGood("Apple", appleCount) == false)//水果不足
        {
            return;
        }
        if (backpack.GetComponent<Backpack>().DeleteGood("Meat", meatCount) == false)//肉不足
        {
            backpack.GetComponent<Backpack>().AddGood("Apple", appleCount, false);
            return;
        }
        if (backpack.GetComponent<Backpack>().AddGood("PowerPotion") == false)//背包已满
        {
            backpack.GetComponent<Backpack>().AddGood("Apple", appleCount, false);
            backpack.GetComponent<Backpack>().AddGood("Meat", meatCount, false);
            return;
        }
    }

    public void MakeSparePart()//制作飞船零件
    {
        int count = 3;
        if(backpack.GetComponent<Backpack>().DeleteGood("Steel", count) == false)//钢不足
        {
            return;
        }
        if(backpack.GetComponent<Backpack>().AddGood("SparePart") == false)//背包已满
        {
            backpack.GetComponent<Backpack>().AddGood("Steel", count, false);
            return;
        }
    }

    public void MakeFire()//制作火把
    {
        int count = 2;
        if (backpack.GetComponent<Backpack>().DeleteGood("Wood", count) == false)//木材不足
        {
            return;
        }
        if (backpack.GetComponent<Backpack>().AddGood("Fire") == false)//背包已满
        {
            backpack.GetComponent<Backpack>().AddGood("Wood", count, false);
            return;
        }
    }

    public void BuyCookedMeat(int plan)//买肉，有两种购买方案
    {
        if(plan == 0)
        {
            int price = 1;
            int meatCount = 1;
            if(backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if(backpack.GetComponent<Backpack>().DeleteGood("Meat", meatCount) == false)//生肉不够
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
            if(backpack.GetComponent<Backpack>().AddGood("CookedMeat") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                backpack.GetComponent<Backpack>().AddGood("Meat", meatCount, false);
                return;
            }
        }
        else if (plan == 1)
        {
            int price = 3;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("CookedMeat") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
        }
    }

    public void EatRice()
    {
        int price = 6;
        if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
        {
            return;
        }
        backpack.GetComponent<Backpack>().player.GetComponent<PlayerControl>().AddPower(100);
    }

    public void SellWood()//卖木材
    {
        int price = 1;
        if(backpack.GetComponent<Backpack>().DeleteGood("Wood") == false)//木材不够
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
    }

    public void SellStone()//卖石头
    {
        int price = 1;
        if (backpack.GetComponent<Backpack>().DeleteGood("Stone") == false)//石头不够
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
    }

    public void SellIronOre()//卖铁矿
    {
        int price = 2;
        if (backpack.GetComponent<Backpack>().DeleteGood("IronOre") == false)//铁矿不够
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
    }

    public void SellApple()//卖苹果
    {
        int price = 2;
        if (backpack.GetComponent<Backpack>().DeleteGood("Apple") == false)//苹果不够
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
    }

    public void SellMeat()//卖肉
    {
        int price = 2;
        if (backpack.GetComponent<Backpack>().DeleteGood("Meat") == false)//肉不够
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
    }

    public void BuyIronSword(int plan)//购买铁剑，两种方案
    {
        if (plan == 0)
        {
            int price = 5;
            int count = 10;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().DeleteGood("IronOre", count) == false)//铁矿不够
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
            if(backpack.GetComponent<Backpack>().AddGood("IronSword") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                backpack.GetComponent<Backpack>().AddGood("IronOre", count, false);
                return;
            }
        }
        else if (plan == 1)
        {
            int price = 30;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("IronSword") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
        }
    }

    public void BuyStoneSword(int plan)//购买石剑，两种方案
    {
        if (plan == 0)
        {
            int price = 3;
            int count = 10;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().DeleteGood("Stone", count) == false)//石头不够
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("StoneSword") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                backpack.GetComponent<Backpack>().AddGood("Stone", count, false);
                return;
            }
        }
        else if (plan == 1)
        {
            int price = 20;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("StoneSword") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
        }
    }

    public void BuyArch(int plan)//购买弓，两种方案
    {
        if (plan == 0)
        {
            int price = 1;
            int count = 6;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().DeleteGood("Wood", count) == false)//木材不够
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("Arch") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                backpack.GetComponent<Backpack>().AddGood("Wood", count, false);
                return;
            }
        }
        else if (plan == 1)
        {
            int price = 8;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("Arch") == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
        }
    }

    public void BuyArrow(int plan)//购买箭，两种方案
    {
        if (plan == 0)
        {
            int price = 2;
            int countIronOre = 1;
            int countWood = 5;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().DeleteGood("Wood", countWood) == false)//木材不够
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
            if (backpack.GetComponent<Backpack>().DeleteGood("IronOre", countIronOre) == false)//铁矿不够
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                backpack.GetComponent<Backpack>().AddGood("Wood", countWood, false);
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("Arrow", 30) == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                backpack.GetComponent<Backpack>().AddGood("Wood", countWood, false);
                backpack.GetComponent<Backpack>().AddGood("IronOre", countIronOre, false);
                return;
            }
        }
        else if (plan == 1)
        {
            int price = 1;
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//钱不够
            {
                return;
            }
            if (backpack.GetComponent<Backpack>().AddGood("Arrow", 5) == false)//背包已满
            {
                backpack.GetComponent<Backpack>().ChangeMoney(price, false);
                return;
            }
        }
    }

    public void BuySteel()
    {
        int count = 1;
        int price = 1;
        if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//金币不足
        {
            return;
        }
        if (backpack.GetComponent<Backpack>().DeleteGood("IronOre", count) == false)//铁矿不足
        {
            backpack.GetComponent<Backpack>().ChangeMoney(price, false);
            return;
        }
        if (backpack.GetComponent<Backpack>().AddGood("Steel") == false)//背包已满
        {
            backpack.GetComponent<Backpack>().ChangeMoney(price, false);
            backpack.GetComponent<Backpack>().AddGood("IronOre", count, false);
            return;
        }
    }

    public void WoodTask()
    {
        int count = 10;
        int price = 12;
        if(backpack.GetComponent<Backpack>().DeleteGood("Wood", count) == false)//木材不足
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
        Destroy(this.gameObject);
    }

    public void StoneTask()
    {
        int count = 5;
        int price = 7;
        if (backpack.GetComponent<Backpack>().DeleteGood("Stone", count) == false)//石头不足
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
        Destroy(this.gameObject);
    }

    public void IronOreTask()
    {
        int count = 4;
        int price = 10;
        if (backpack.GetComponent<Backpack>().DeleteGood("IronOre", count) == false)//铁矿不足
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
        Destroy(this.gameObject);
    }

    public void AppleTask()
    {
        int count = 10;
        int price = 25;
        if (backpack.GetComponent<Backpack>().DeleteGood("Apple", count) == false)//苹果不足
        {
            return;
        }
        backpack.GetComponent<Backpack>().ChangeMoney(price);
        Destroy(this.gameObject);
    }

    public void FixDoor(int plan)//修门
    {
        GameObject gameControl = backpack.GetComponent<Backpack>().gameControl;
        int price = 20;
        int count = 10;
        int HP = 100;
        if (plan == 0)
        {
            if (backpack.GetComponent<Backpack>().DeleteGood("Wood", count) == false)//木材不足
            {
                return;
            }
        }
        else if(plan == 1)
        {
            if (backpack.GetComponent<Backpack>().ChangeMoney(-price) == false)//Do币不足
            {
                return;
            }
        }
        else if(plan == 2)
        {
            gameControl.GetComponent<GameControl>().Prompt("门HP:" + gameControl.GetComponent<GameControl>().door.GetComponent<DoorControl>().Fix(0).ToString(), false);
            return;
        }
        gameControl.GetComponent<GameControl>().Prompt("门HP:" + gameControl.GetComponent<GameControl>().door.GetComponent<DoorControl>().Fix(HP).ToString(), false);

    }

    public void StrengthenGuard()//强化卫兵
    {
        int count = 10;
        int number = 1;
        if(backpack.GetComponent<Backpack>().DeleteGood("Apple", count) == false)//苹果不足
        {
            return;
        }
        GameObject gameControl = backpack.GetComponent<Backpack>().gameControl;
        backpack.GetComponent<Backpack>().gameControl.GetComponent<GameControl>().archer.GetComponent<GuardControl>().damage += number;
        gameControl.GetComponent<GameControl>().Prompt("卫兵攻击力+1", false);

    }

    public void FixUFO()//修理飞船
    {
        int count = 1;
        int HP = 5;
        if (backpack.GetComponent<Backpack>().DeleteGood("SparePart", count) == false)//零件不足
        {
            return;
        }
        GameObject gameControl = backpack.GetComponent<Backpack>().gameControl;
        gameControl.GetComponent<GameControl>().UFO.GetComponent<UFOControl>().Fix(HP);
        gameControl.GetComponent<GameControl>().Prompt("剩余进度：" + gameControl.GetComponent<GameControl>().UFO.GetComponent<UFOControl>().GetHP().ToString() + "%");
    }

    public void Sleep()
    {
        backpack.GetComponent<Backpack>().player.GetComponent<PlayerControl>().AddPower(100);
        GameControl gameControl = backpack.GetComponent<Backpack>().gameControl.GetComponent<GameControl>();
        gameControl.SetTime(gameControl.GetTime() + (4 * 60));//睡8个小时
    }
}
