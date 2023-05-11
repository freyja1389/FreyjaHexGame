using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map //: MonoBehaviour
{
    public readonly BaseCell StartCell;
    public readonly BaseCell EndCell;
    public event Action<BaseCell> CellClicked;
    public event Action<Bonus,BaseCell> BonusClicked;
    public event Action<CellContent> ContentShown;

    public List<Enemy> OpenEnemy { get; private set; } = new();
    //private List<Enemy> openEnemy;
    private BaseCell[,] hexCells;

    private readonly List<Vector2Int> neighborRulesEvenY = new List<Vector2Int> //чет y
    {
         new Vector2Int(-1, 1),
         new Vector2Int(0, -1),
         new Vector2Int(1, 0),
         new Vector2Int(0, 1),
         new Vector2Int(-1, -1),
         new Vector2Int(-1, 0),
    };
    private readonly List<Vector2Int> neighborRulesOddY = new List<Vector2Int> //нечет y
    {
         new Vector2Int(0, -1),
         new Vector2Int(1, -1),
         new Vector2Int(1, 0),
         new Vector2Int(1, 1),
         new Vector2Int(0, 1),
         new Vector2Int(-1, 0),

    };

   

    public Map(BaseCell[,] hexCells, BaseCell startCell, BaseCell endCell)
    {
        this.hexCells = hexCells;
        StartCell = startCell;
        EndCell = endCell;

        foreach (var cell in hexCells)
        {
            if (cell is null) continue;
            cell.CellClicked += OnCellClicked;
            cell.ShownContent += CellShownContent;
        }
    }

    private void CellShownContent(BaseCell cellClicked )
    {
        ContentShown?.Invoke(cellClicked.ContentLink);

        if (cellClicked.ContentLink is Bonus bonus)
        {
            bonus.ParentCell = cellClicked;
            bonus.MoveBonusIntoBonusCell += MoveBonusIntoBonusCell;
        }
        else if (cellClicked.ContentLink is Enemy enemy)
        {
            enemy.ReadyForDestroy += DestroyCellContent;
            OpenEnemy.Add(enemy);
        }
    }

    private void MoveBonusIntoBonusCell(Bonus bonus, BaseCell cellClicked)
    {
        BonusClicked?.Invoke(bonus, cellClicked);
    }

    public List<BaseCell> GetAvailableCells(BaseCell startCell) 
    {
        var NeighborCells = new List<BaseCell>();
        foreach (var cell in hexCells)
        {
            if (cell is not null)
            {
                var distance = cell.CellIndex - startCell.CellIndex;
                if (startCell.CellIndex.y % 2 == 0)
                {
                    if (neighborRulesEvenY.Contains(distance))
                    {
                        NeighborCells.Add(cell);
                    }
                }
                else
                {
                    if (neighborRulesOddY.Contains(distance))
                    {
                        NeighborCells.Add(cell);
                    }
                }
            }

        }
        return NeighborCells;
    }

    public bool IsNeighbor(BaseCell cellClicked, Vector2Int playerPositionInMap)
    {
        var distance = cellClicked.CellIndex - playerPositionInMap;
        if (playerPositionInMap.y % 2 == 0)
        {
            return neighborRulesEvenY.Contains(distance);
        }
        return neighborRulesOddY.Contains(distance);
    }

    public void OpenEnemyOnCellClicked()
    {
        foreach (CellContent enemy in OpenEnemy)
        {
            enemy.OnAnyCellClicked(OpenEnemy);
        }
    }

private void OnCellClicked(BaseCell cellClicked)
    {
        CellClicked?.Invoke(cellClicked);

    }

    private void DestroyCellContent(CellContent cellContent)
    {

        if (cellContent is Enemy enemy)
        {
            OpenEnemy.Remove(enemy);
        }

        cellContent.SelfDestroy();
    }







}
