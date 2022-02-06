using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //make the end hex another color +
    //visualise player and active neighbors
    //hitbar
    //win logic
    public int Columns = 0;
    public Text PlayersHPTextBox;
    public BaseCell[,] HexCells;
    public int Rows = 0;

    [SerializeField]
    private MapGenerator mGenerator;
    [SerializeField]
    private Player  player;
    [SerializeField]
    private Material[] materials;

    private List<BaseCell> neighbors;
    
    private Vector2Int playerPositionInMap;

    private readonly List<Vector2Int> neighborRulesEvenY = new List<Vector2Int> //чет y
    {
         new Vector2Int(-1, 1),
         new Vector2Int(0, -1),
         new Vector2Int(1, 0),
         new Vector2Int(0, 1),
         new Vector2Int(-1, 1),
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

    void Start()
    { 
        HexCells = mGenerator.MapCreate(Rows, Columns, mGenerator.transform, player);
        neighbors = GetAvailableCells(mGenerator.StartCell);
        foreach (var neighbor in neighbors)
        {
            neighbor.SetMaterial(materials[1]); 
        }

        player.ChangePosition(mGenerator.StartCell.transform.position);
        playerPositionInMap = mGenerator.StartCell.CellIndex;

        foreach (var cell in HexCells)
        {
            cell.CellClicked += OnCellClicked;
        }
    }

    public void OnCellClicked(BaseCell cellClicked)
    {
        if (IsNeighbor(cellClicked))
        {
            if (cellClicked is EnemyCell enemy)
            {
               player.SetDamage(enemy.DmgPoints);
            }
            else if (cellClicked is BonusCell bonus)
            {
                player.SetHeal(bonus.HealPoints);
            }

            PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
            playerPositionInMap = cellClicked.CellIndex;

            player.Relocation(cellClicked.transform.position);
            cellClicked.Open = true;

            ActivateCellMaterial(cellClicked);

            SetNeighbornsMaterial(cellClicked);

            cellClicked.Activate();
        }
    }

   private bool IsNeighbor(BaseCell cellClicked)
    {
        var distance = cellClicked.CellIndex - playerPositionInMap;
        if (playerPositionInMap.y % 2 == 0)
        {
            return neighborRulesEvenY.Contains(distance);
        }
        {
            return neighborRulesOddY.Contains(distance);
        }
    }

    private List<BaseCell> GetAvailableCells(BaseCell startCell)
    {
        List<BaseCell> NeighborCells = new List<BaseCell>();
        foreach (var cell in HexCells)
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
        return NeighborCells;
    }

    private void SetNeighbornsMaterial(BaseCell cellClicked)
    {
        foreach (var neighbor in neighbors)
        {
            if (!neighbor.Open)
            {
                if ((neighbor is EmptyCell empty))
                {
                    if (!empty.StartCell & !empty.EndCell)
                    {
                        neighbor.SetMaterial(materials[0]);
                    }
                }
                else
                {
                    neighbor.SetMaterial(materials[0]);
                }
            }
        }
        neighbors = GetAvailableCells(cellClicked);
        foreach (var neighbor in neighbors)
        {
            if (!neighbor.Open)
            {
                if ((neighbor is EmptyCell empty))
                {
                    if (!empty.StartCell & !empty.EndCell)
                    {
                        neighbor.SetMaterial(materials[1]);
                    }
                }
                else
                {
                    neighbor.SetMaterial(materials[1]);
                }
            }
        }
    }

    private void ActivateCellMaterial(BaseCell cellClicked)
    {
        Material material;
        if (cellClicked is EnemyCell enemy)
        {
            material = materials[3];
        }
        else if (cellClicked is BonusCell bonus)
        {
            material = materials[2];
        }
        else
        {
            material = materials[4];
        }
        cellClicked.SetMaterial(material);
    }
}
