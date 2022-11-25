using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDD : Enemy
{
    public int CriticalChanse;

    // Start is called before the first frame update
    void Awake()
    {
        CriticalChanse = SetCriticalChanse();
        DmgPoints = SetDmgPoints();
        HitPoints = SetHitPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int SetCriticalChanse()
    {
         return Random.Range(10, 100);
    }

}
