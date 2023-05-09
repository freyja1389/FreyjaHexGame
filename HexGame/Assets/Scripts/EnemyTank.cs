using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    // Start is called before the first frame update
    void Awake()
    {
        BasetHitPoints = SetHitPoints();
        DmgPoints = SetDmgPoints();
        CurrentHitPoints = BasetHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override int SetHitPoints()
    {
        return Random.Range(50, 50 * 3);
    }

    public override void OnContentClicked(Player player, List<Enemy> openEnemy, BaseCell cellClicked)
    {
        base.OnContentClicked(player, openEnemy, cellClicked);
    }
}
