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

    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked)
    {
        //GetHealToOpenEnemies(HealPoints, openEnemy);

        base.OnContentClicked(player, openEnemy, cellClicked);
    }
    private void GetHealToOpenEnemies(int healpoints, List<Enemy> openEnemy)
    {
        foreach (Enemy enemy in openEnemy)
        {
            enemy.HitPoints += healpoints;
            enemy.HitBar.ChangeEnemyHitBarFillAmount(enemy.HitPoints);
        }
    }

    public override void OnAnyCellClicked(List<Enemy> openEnemy)
    {
        GetHealToOpenEnemies(HealPoints, openEnemy);
    }

}
