using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HitPoints => hitPoints;
    public int DmgPoints;
    public GameObject PlayerInstance;

    [SerializeField]
    private int hitPoints;

    public void SetDamage(int value)
    {
        hitPoints = Mathf.Clamp(hitPoints - value, 0, 100);
    }

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
        PlayerInstance.transform.position =  Vector3.Lerp(PlayerInstance.transform.position, vector, 2);
        yield return null;
    }
    
    public void Relocation(Vector3 vector)
    {
        StartCoroutine(MoveToTarget(vector));
    }

}
