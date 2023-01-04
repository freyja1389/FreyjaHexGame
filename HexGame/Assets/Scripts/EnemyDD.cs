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


    private int SetCriticalChanse()
    {
         return Random.Range(10, 100);
    }

    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked)
    {
        base.OnContentClicked(player, openEnemy, cellClicked);
    }

}
