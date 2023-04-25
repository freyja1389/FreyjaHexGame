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
        BasetHitPoints = SetHitPoints();
        CurrentHitPoints = BasetHitPoints;
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

    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked, UIController uiController)
    {
        //GetHealToOpenEnemies(HealPoints, openEnemy);

        base.OnContentClicked(player, openEnemy, cellClicked, uiController);
    }
    private void GetHealToOpenEnemies(int healpoints, List<Enemy> openEnemy)
    {
        foreach (Enemy enemy in openEnemy)
        {
            enemy.CurrentHitPoints += healpoints;
            enemy.HitBar.ChangeEnemyHitBarFillAmount(enemy.CurrentHitPoints, enemy.BasetHitPoints);
        }
    }

    public override void OnAnyCellClicked(List<Enemy> openEnemy)
    {
        GetHealToOpenEnemies(HealPoints, openEnemy);
    }

}
