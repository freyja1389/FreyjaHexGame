using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bonus : CellContent
{
    //[SerializeField]
    //private HexCell Type;
    public event Action<Bonus, BaseCell> MoveBonusIntoBonusCell;
    public Vector2Int CellIndex;
    public bool Open;
   // private bool readyForApply;
    public BaseCell ParentCell;

 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnContentClicked(Player player, List<Enemy> openEnemy, EmptyCell cellClicked, UIController uiController)
    {
        // player.SetHeal(HealPoints);
        //if (!readyForApply)
        //{
        //Unsubscribe(cellClicked);
            // Destroy(gameObject); - не удаляем, а переносим в бонусы
           // player.SetBonusInBonusCell(this);
        MoveBonusIntoBonusCell?.Invoke(this, ParentCell);
        //cellClicked.ContentLink = null;
           // readyForApply = true; // bonus has been taken
        //} 
        //else
        //{
           // OnContentApplied(player, openEnemy);
        //}
    }
    public override void OnContentApplied(Player player, List<Enemy> openEnemy)
    {
        return;
    }

    public void RelocateBonusIntoBonusCell(Canvas canvas)
    {
        gameObject.transform.SetParent(canvas.transform);
        //gameObject.transform = new Vector3(0, 20, 0)

    }
}
