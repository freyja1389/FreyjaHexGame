using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HitPoints => hitPoints;
    public GameObject PlayerInstance;

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

    private IEnumerator MoveToTarget(Vector3 vector)
    {
        PlayerInstance.transform.position =  Vector3.Lerp(PlayerInstance.transform.position, vector, 2);
        yield return null;
    }
    
    public void Relocation(Vector3 vector)
    {
        StartCoroutine(MoveToTarget(vector));
    }

}
