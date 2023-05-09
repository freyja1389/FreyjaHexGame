using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDMGBooster : Bonus
{
    public int BoostDMGPoints;
    // Start is called before the first frame update
    void Start()
    {
        BoostDMGPoints = SetBoostDMGPoints();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int SetBoostDMGPoints()
    {
        return Random.Range(5, 15);
    }

    public override void OnContentClicked(Player player, List<Enemy> openEnemy, BaseCell cellClicked)
    {
        base.OnContentClicked(player, openEnemy, cellClicked);
    }

    public override void OnContentApplied(Player player, List<Enemy> openEnemy)
    {
        player.IncreaseDamage(BoostDMGPoints);
    }
}
