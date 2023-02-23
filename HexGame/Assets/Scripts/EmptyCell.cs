using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCell : BaseCell
{

    /* 1)сразу генерировать и записывать ссылку на префаб содержимого €чейки
     * 2)разнести логику по функци€м в эмпти целл и гейм контроллер
     */
    public event Action<CellContent> ShownContent;

    public CellType CellType;

    public bool Opened;

    public CellContent ContentLink;

    private CellContent ContentPrefab;

    public void SetContentPrefab(CellContent prefab)
    {
        ContentPrefab = prefab;
    }

    public void OnCellClicked()
    {
        RiseCellClicked();
    }
   
    internal void ShowContent(Canvas wSCanvas, UIController uIController)
    {
        switch (CellType)
        {
            case CellType.EnemyCell:
                {
                    var enemy = Instantiate((Enemy)ContentPrefab, transform.position, transform.rotation, transform);
                   // UIController enemyBar = Instantiate(enemy.EnemyHitBarPref, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.Euler(90, 0, 0), wSCanvas.transform);
                    ContentLink = enemy;
                    enemy.ContentClicked += OnCellClicked;
                    //OpenEnemy.Add(enemy);
                    //enemyBar.ChangeHitBarFillAmount(enemy.HitPoints);
                    // enemy.HitBar = enemyBar;
                    uIController.ViewEnemyInformation(enemy, this, wSCanvas);
                }
                break;
            case CellType.BonusCell:
                {
                    Bonus bonus = Instantiate((Bonus)ContentPrefab, transform.position, transform.rotation, transform);
                    ContentLink = bonus;
                    Opened = true;
                   // bonus.ContentClicked += OnContentClicked;
                }
                break;
            case CellType.EndCell:
                {

                }
                break;
        }
        ShownContent?.Invoke(ContentLink);
    }
  
}
