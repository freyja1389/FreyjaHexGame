using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEnemyHPReducer : Bonus
{
    public int ReduceHPPoints;
    // Start is called before the first frame update
    void Start()
    {
        ReduceHPPoints = SetHPReducePoints();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int SetHPReducePoints()
    {
        return Random.Range(5, 15);
    } 


    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked)
    {
        base.OnContentClicked(player, openEnemy, cellClicked);
    }

    public override void OnContentApplied(Player player, List<Enemy> openEnemy)
    {
        foreach (Enemy enemy in openEnemy)
        {
            enemy.SetDamage(ReduceHPPoints);
        }
    }
}
