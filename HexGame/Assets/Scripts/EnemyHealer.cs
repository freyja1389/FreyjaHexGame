using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealer : Enemy
{
    public int HealPoints;
    // Start is called before the first frame update
    void Awake()
    {
        HealPoints = SetHealPoints();
        HitPoints = SetHitPoints();
        DmgPoints = SetDmgPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private int SetHealPoints()
    {
        return Random.Range(2, 2 * 3);
    }
}
