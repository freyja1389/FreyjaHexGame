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
    public Text EnemyInfo;
    public UIController EnemyHitBarPref;
    private EmptyCell currentcellClicked;
    public event Action<int> EnemyAlive;
    

    //public Action <CellContent> ReadyForDestroy;
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
        currentcellClicked = cellClicked;
        player.SetAttackAnimation();


       player.SetPlayerinteractionEnemyLink(this);
        SetDamage(player.DmgPoints);
        HitBar.ChangeHitBarFillAmount(HitPoints);
    }

    public override void CheckEnemyDeath()
    {
        //player.SetPlayerDamage(DmgPoints);
        if (HitPoints <= 0)
        {
            base.RiseReadyForDestroy((CellContent)this);
        }
        else
        {
            EnemyAlive?.Invoke(DmgPoints);
        }
    }

    public override void SelfDestroy()
    {
        Unsubscribe(currentcellClicked);
        Destroy(HitBar.gameObject);
        Destroy(gameObject);
        Destroy(EnemyInfo.gameObject);
    }
}
