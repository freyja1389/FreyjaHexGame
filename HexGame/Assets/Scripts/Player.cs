using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HitPoints => hitPoints;

    [SerializeField]
    private int hitPoints;

    public void SetDamage(int value)
    {
        hitPoints -= value;
    }

    public void SetHeal(int value)
    { 
        hitPoints += value;
    }

    public void ChangePosition(Vector3 position)
    { 
        transform.position = position;
    }
}
