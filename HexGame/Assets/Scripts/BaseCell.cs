using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCell : MonoBehaviour
{
    public event Action<BaseCell> CellClicked;
    public Vector2Int CellIndex;
    public bool Open;

    public event Action<BaseCell> ShownContent;

    public CellType CellType;

    public bool Opened;

    public CellContent ContentLink;

    private CellContent ContentPrefab;

    public void Activate()
    {
       // GetComponent<Rigidbody>().useGravity = true;
    }

    protected virtual void OnMouseUpAsButton()
    {
        RiseCellClicked();
    }

    private void OnDestroy()
    {
        CellClicked = null; 
    }

    protected void  RiseCellClicked()
    {
        CellClicked?.Invoke(this);
    }

    public void SetMaterial(Material material)
    {
        GetComponentInChildren<MeshRenderer>().material = material;
    }

    public void SetContentPrefab(CellContent prefab)
    {
        ContentPrefab = prefab;
    }

    public void OnCellClicked()
    {
       RiseCellClicked();
    }

    internal void ShowContent()
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
                    //enemyBar.ChangeEnemyHitBarFillAmount(enemy.HitPoints);
                    // enemy.HitBar = enemyBar;
                    //uIController.ViewEnemyInformation(enemy, this, wSCanvas);
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
        ShownContent?.Invoke(this);
    }
}
