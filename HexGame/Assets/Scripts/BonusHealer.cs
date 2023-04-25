using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHealer : Bonus
{
    public int HealPoints;

    // Start is called before the first frame update
    void Start()
    {
        HealPoints = SetHealPoints();
    }

    private int SetHealPoints()
    {
        return Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked, UIController uiController)
    {
        base.OnContentClicked(player, openEnemy, cellClicked, uiController);
    }

    public override void OnContentApplied(Player player, List<Enemy> openEnemy)
    {
        player.SetHeal(HealPoints);
    }
}
