using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Enemy : CellContent
{
    public int DmgPoints;
    public int CurrentHitPoints { get; protected set; }
    public int BasetHitPoints;
    public  UIController HitBar;
    public Text EnemyInfo;
    public UIController EnemyHitBarPref;
    private BaseCell currentcellClicked;
    //public event Action<int> EnemyAlive;
    public event Action<int> EnemyAttackStarted;
    public event Action<int> EnemyAttackCompleted;
    public event Action<Enemy> StateChanged;
    private Animator anim;
    private DamageSTM damageAnimSTM;
    private AttackSTM attackAnimSTM;
    private DieSTM dieAnimSTM;


    //public Action <CellContent> ReadyForDestroy;
    // Start is called before the first frame update

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        attackAnimSTM = anim.GetBehaviour<AttackSTM>();
        damageAnimSTM = anim.GetBehaviour<DamageSTM>();
        dieAnimSTM = anim.GetBehaviour<DieSTM>();

        damageAnimSTM.DamageAnimationComplete += SetAttackAnimation;
        attackAnimSTM.AttackAnimationStarted+= RiseAttackStarted;
        attackAnimSTM.AttackAnimationComplete += RiseAttacCompleted;
        dieAnimSTM.DieAnimationComplete += OnDieAnimationComplete;

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

    private void RiseAttacCompleted(Animator animator)
    {
        if (!animator == anim) return;
        EnemyAttackCompleted?.Invoke(DmgPoints);
    }
    public int SetDamage(int dmg)
    {
        CurrentHitPoints -= dmg;
        CheckEnemyDeath();
        return CurrentHitPoints;
    }

    public int SetHeal(int healPoints)
    {
        CurrentHitPoints += healPoints;
        return CurrentHitPoints;
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


    public override void OnContentClicked(Player player, List<Enemy> openEnemy, BaseCell cellClicked)
    {
        transform.LookAt(player.transform.position);
        currentcellClicked = cellClicked;
        player.SetAttackAnimation();


       player.SetPlayerinteractionEnemyLink(this);
        SetDamageAnimation();
        SetDamage(player.DmgPoints);
        HitBar.ChangeEnemyHitBarFillAmount(CurrentHitPoints, BasetHitPoints);
        //uiController.UpdateEnemyTextInfo(this);
        StateChanged?.Invoke(this);
    }

    public override void CheckEnemyDeath()
    {
       // if (!animator == anim) return;
        //player.SetPlayerDamage(DmgPoints);
        if (CurrentHitPoints <= 0)
        {
            EnemyAttackStarted = null;
            EnemyAttackCompleted = null;
            anim.SetTrigger("Die");
        }
    }

    private void OnDieAnimationComplete(Animator animator)
    {
        if (!animator == anim) return;
        base.RiseReadyForDestroy((CellContent)this);
    }

    public override void SelfDestroy()
    {
        Unsubscribe(currentcellClicked);
        Destroy(HitBar.gameObject);
        Destroy(gameObject);
        Destroy(EnemyInfo.gameObject);
    }
}