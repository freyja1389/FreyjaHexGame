using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    // Start is called before the first frame update
    void Awake()
    {
        HitPoints = SetHitPoints();
        DmgPoints = SetDmgPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override int SetHitPoints()
    {
        return Random.Range(50, 50 * 3);
    }
}
