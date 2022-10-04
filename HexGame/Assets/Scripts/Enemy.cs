using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CellContent
{
    public int DmgPoints;
    public int HitPoints;
    // Start is called before the first frame update
    void Start()
    {
        HitPoints = SetHitPoints();
        DmgPoints = SetDmgPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int SetDamage(int dmg)
    {
        HitPoints = HitPoints - dmg;
        return HitPoints;
    }

    private int SetHitPoints()
    {
        return Random.Range(30, 30 * 3);
    }

    private int SetDmgPoints()
    {
        return Random.Range(5, 10 * 3);
    }
}
