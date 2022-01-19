using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //make the end hex another color
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

    private Vector2Int playerPositionInMap;

    private readonly List<Vector2Int> neighborRules = new List<Vector2Int>
    {
         new Vector2Int(0, 1),
         new Vector2Int(1, 0),
         new Vector2Int(1, 1),
         new Vector2Int(-1, 1),
         new Vector2Int(0, -1),
         new Vector2Int(-1, 0),
    };

    void Start()
    { 
        HexCells = mGenerator.MapCreate(Rows, Columns, mGenerator.transform);

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

            cellClicked.Activate();
        }
    }

    private bool IsNeighbor(BaseCell cellClicked)
    {
        var distance = cellClicked.CellIndex - playerPositionInMap;
        return neighborRules.Contains(distance);
    }
}
