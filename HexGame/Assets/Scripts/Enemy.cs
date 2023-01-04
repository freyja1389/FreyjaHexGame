using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Enemy : CellContent
{
    public int DmgPoints;
    public int HitPoints;
    public  UIController HitBar;
   public UIController EnemyHitBarPref;
    // Start is called before the first frame update
  
    public int SetDamage(int dmg)
    {
        HitPoints = HitPoints - dmg;
        return HitPoints;
    }

    protected virtual int SetHitPoints()
    {
        return UnityEngine.Random.Range(30, 30 * 3);
    }

    protected virtual int SetDmgPoints()
    {
        return UnityEngine.Random.Range(5, 10 * 3);
    }

    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked)
    {

        player.SetDamage(DmgPoints);

        SetDamage(player.DmgPoints);

        HitBar.ChangeHitBarFillAmount(HitPoints);
        if (HitPoints <= 0)
        {
            Unsubscribe(cellClicked);
            openEnemy.Remove(this);
            Destroy(HitBar.gameObject);
            Destroy(gameObject);
        }

    }
}
