using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public int HitPoints => hitPoints;
    public int DmgPoints;
    public GameObject PlayerInstance;
    public int Lvl;

    public List<Bonus> Bonuses;

    public UIController UIController;
    [SerializeField]
    private int hitPoints;
    private Animator anim;
    private DamageSTM damageAnimSTM;
    private AttackSTM attackAnimSTM;

    public Enemy InteractionEnemyLink;

    public event Action <Animator> CheckEnemyDeath;

   // public event Action AttackAnimationPlayedCountinue;
    //public event Action DamageAnimationPlayedCountinue;


    public void Update()
    {
////        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack" && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
       // {
            
           // AttackAnimationPlayedCountinue?.Invoke();
        //}

       // if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Damage_00" && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
       // {
        //   // DamageAnimationPlayedCountinue?.Invoke();
       // }

        //return;
    }

    public bool PlayerCanAction()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return true;
        }
        else 
        {
            return false;
        }

    }

    public void Start()
    {
        
        //damageAnimSTM = anim.GetBehaviour<DamageSTM>();
       // damageAnimSTM.DamageAnimationComplete += SetDamageWithAnimation;

    }
    public void Awake()
    {

        InteractionEnemyLink = null;
        SetHitDamagePoints();
        anim = gameObject.GetComponentInChildren<Animator>();
        attackAnimSTM = anim.GetBehaviour<AttackSTM>();
        attackAnimSTM.AttackAnimationComplete += AttackCompleated;

    }
    
    private void SetHitDamagePoints()
    {
        hitPoints = 100;
        DmgPoints = 40;
    }

    public void SetAttackAnimation()
    {
        anim.SetTrigger("Attack");
       // this.AttackAnimationPlayedCountinue += this.SetDamageWithAnimation;
    }

    public void SetPlayerinteractionEnemyLink(Enemy enemy)
    {
        if (InteractionEnemyLink == null)  
        {
            InteractionEnemyLink = enemy;
        }
    }

    public void AttackCompleated(Animator animator)
    {
        if (!animator == anim) return;
        CheckEnemyDeath?.Invoke(null);
    }


    public void SetDamageWithAnimation(int dmgPoints)
    {
        // yield return new WaitForSeconds(2);
        //new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        
            if (dmgPoints > 0)
            {
                SetDamageAnim();
                SetDamage(dmgPoints);
                // this.AttackAnimationPlayedCountinue -= this.SetDamageWithAnimation;
            
                UIController.ShowPlayerHP(this);
                //DamageAnimationPlayedCountinue?.Invoke();
            }
        
    }

    private void SetDamageAnim()
    {
        anim.SetTrigger("Damage");
    }
    private void SetDamage(int value)
    {
        hitPoints = Mathf.Clamp(hitPoints - value, 0, 100);
    }    

    public void IncreaseDamage(int bonusDMG)
    {
        DmgPoints += bonusDMG;
    }

    

    // public void SetBonusInBonusCell(Bonus bonus)
    //{
    //  AddBonus(bonus);

    // }

    public void SetHeal(int value)
    {
      
            //hitPoints += value <= 100 ? hitPoints : 100;
            hitPoints = Mathf.Clamp(hitPoints + value, 0, 100);
    }

    public void RelocateInstantly(Vector3 position)
    { 
        transform.position = position;
    }

    private IEnumerator MoveToTarget(Vector3 vector)
    {
        Animator anim = gameObject.GetComponentInChildren<Animator>();
        float timeCounter = 0;
       anim.SetTrigger("Walk");
        while (timeCounter < 1)
        {
            timeCounter = timeCounter + Time.deltaTime;
            PlayerInstance.transform.position = Vector3.Lerp(PlayerInstance.transform.position, vector, timeCounter);
            yield return null;
        }
       anim.SetTrigger("Idle");
    }
    
    public void Relocation(Vector3 vector)
    {
        StartCoroutine(MoveToTarget(vector));
    }

    public void UnsubscribePlayerAnimationEvents()
    {
        //attackAnimSTM.AttackAnimationComplete -= SetDamageWithAnimation;
        //damageAnimSTM.DamageAnimationComplete -= SetDamageWithAnimation;

    }
}
