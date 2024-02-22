using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Enemy : CellContent, IFighter
{
    public int DmgPoints;
    public int CurrentHitPoints { get; protected set; }
    public int BasetHitPoints;
    public SceneUI HitBar;
    public Text EnemyInfo;
    public SceneUI EnemyHitBarPref;
    private BaseCell currentcellClicked;
    //public event Action<int> EnemyAlive;
    public event Action<int> EnemyAttackStarted;
    public event Action<int> EnemyAttackCompleted;
    public event Action<Enemy> StateChanged;
    private Animator anim;
    private DamageSTM damageAnimSTM;
    private AttackSTM attackAnimSTM;
    private DieSTM dieAnimSTM;
    private bool isAttackable;


    //public Action <CellContent> ReadyForDestroy;
    // Start is called before the first frame update

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        attackAnimSTM = anim.GetBehaviour<AttackSTM>();
        damageAnimSTM = anim.GetBehaviour<DamageSTM>();
        dieAnimSTM = anim.GetBehaviour<DieSTM>();
        isAttackable = true;

        damageAnimSTM.DamageAnimationComplete += SetAttackAnimation;
        attackAnimSTM.AttackAnimationStarted+= RiseAttackStarted;
        attackAnimSTM.AttackAnimationComplete += RiseAttackCompleted;
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

    private void RiseAttackCompleted(Animator animator)
    {
        if (!animator == anim) return;
        EnemyAttackCompleted?.Invoke(DmgPoints);
        isAttackable = true;
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

    public bool IsAttackable()
    {
        return isAttackable;
    }


    public override void OnContentClicked(Player player, List<Enemy> openEnemy, BaseCell cellClicked)
    {
        if (!IsAttackable()) return;
     
        transform.LookAt(player.transform.position);
        currentcellClicked = cellClicked;
       
            player.SetAttackAnimation(this);
            isAttackable = false;

            player.SetPlayerinteractionEnemyLink(this);
            SetDamageAnimation();
            SetDamage(player.DmgPoints);
            HitBar.ChangeEnemyHitBarFillAmount(CurrentHitPoints, BasetHitPoints);
            //uiController.UpdateEnemyTextInfo(this);
            StateChanged?.Invoke(this);

            Debug.Log(isAttackable);
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
