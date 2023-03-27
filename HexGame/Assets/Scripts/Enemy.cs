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
    //public event Action<int> EnemyAlive;
    public event Action<int> EnemyAttackStarted;
    private Animator anim;
    private DamageSTM damageAnimSTM;
    private AttackSTM attackAnimSTM;


    //public Action <CellContent> ReadyForDestroy;
    // Start is called before the first frame update

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        attackAnimSTM = anim.GetBehaviour<AttackSTM>();
        damageAnimSTM = anim.GetBehaviour<DamageSTM>();

        damageAnimSTM.DamageAnimationComplete += SetAttackAnimation;
        attackAnimSTM.AttackAnimationStarted += RiseAttackStarted;
    }

    protected virtual int SetHitPoints()
    {
        return UnityEngine.Random.Range(30, 30 * 3);
    }

    protected virtual int SetDmgPoints()
    {
        return UnityEngine.Random.Range(5, 10 * 3);
    }
/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
    private void RiseAttackStarted(Animator animator)
    {
        if (!animator == anim) return;
        EnemyAttackStarted?.Invoke(DmgPoints);
    }
    public int SetDamage(int dmg)
    {
        HitPoints = HitPoints - dmg;
        return HitPoints;
    }

    public void SetAttackAnimation(Animator animator)
    {
        if (!animator == anim) return;
        anim.SetTrigger("Attack");
    }

    public void SetDamageAnimation()
    {
        anim.SetTrigger("Damage");
    }


    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked)
    {
        transform.LookAt(player.transform.position);
        currentcellClicked = cellClicked;
        player.SetAttackAnimation();


       player.SetPlayerinteractionEnemyLink(this);
        SetDamageAnimation();
        SetDamage(player.DmgPoints);
        HitBar.ChangeHitBarFillAmount(HitPoints);
    }

    public override void CheckEnemyDeath(Animator animator)
    {
        //if (!animator == anim) return;
        //player.SetPlayerDamage(DmgPoints);
        if (HitPoints <= 0)
        {
            base.RiseReadyForDestroy((CellContent)this);
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
