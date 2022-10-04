using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCell : BaseCell
{
    public bool StartCell;
    public bool EndCell;
    public bool EnemyCell;
    public bool BonusCell;

    public bool Opened;

    public CellContent ContentLink;
        
    // Start is called before the first frame update
    void Start()
    {
        if (this.EnemyCell)
        {

        }
        else if (this.BonusCell)
        {

        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
