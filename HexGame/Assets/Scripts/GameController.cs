using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //make the end hex another color +
    //visualise player and active neighbors+
    //hitbar +
    //win logic
    public int Columns = 0;
    //public Text PlayersHPTextBox;
    //public List<BaseCell> HexCells;
    public BaseCell[,] HexCells;
    public int Rows = 0;
   // public Image Bar;
    public GameObject MenuPanel;
    public MenuControls Menu;
    public Text LvlInfo;
    // public UIController EnemyHitBarPref;
    public Canvas WSCanvas;
    public Canvas UICanvas;
    public UIController UIController;

    [SerializeField]
    private MapGenerator mGenerator;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    //private GameObject GameOverTextBar;

    private List<BaseCell> neighbors;
    private Vector2Int playerPositionInMap;
    private PlayersProgress playerProgress;
    private string filePath;
    private BaseCell PrevCell;
    public List<Enemy> OpenEnemy;



    public readonly List<Vector2Int> neighborRulesEvenY = new List<Vector2Int> //��� y
    {
         new Vector2Int(-1, 1),
         new Vector2Int(0, -1),
         new Vector2Int(1, 0),
         new Vector2Int(0, 1),
         new Vector2Int(-1, -1),
         new Vector2Int(-1, 0),
    };
    public readonly List<Vector2Int> neighborRulesOddY = new List<Vector2Int> //����� y
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
        filePath = Application.persistentDataPath + @"\ProgressData";
        LoadProgress();
        LvlInfo.text = "Lvl:" + playerProgress.Lvl;
        UpdatePlayerInformation();
        //HexCells = mGenerator.MapCreate1(Rows, Columns, mGenerator.transform, player, playerProgress.Lvl);
        HexCells = mGenerator.MapGenerate(Rows, Columns, mGenerator.transform, player, playerProgress.Lvl);
        neighbors = GetAvailableCells(mGenerator.StartCell);
        foreach (var neighbor in neighbors)
        {
            /* if (neighbor is EmptyCell empty && empty.CellType == EmptyCell.CellTypes.StartCell)
            {
                continue; 
            }*/
            neighbor.SetMaterial(materials[1]);
        }

        player.RelocateInstantly(mGenerator.StartCell.transform.position);
        playerPositionInMap = mGenerator.StartCell.CellIndex;

        foreach (var cell in HexCells)
        {
            if (!(cell == null))
            {
                cell.CellClicked += OnCellClicked;
            }
        }
        Menu.StartClicked += OnStartClicked;

        PrevCell = mGenerator.StartCell;
    }

    public void cellUsed(EmptyCell cellClicked)
    {  
        var content = cellClicked.ContentLink;
        content.OnContentClicked(player, OpenEnemy, cellClicked);

        UIController.ShowPlayerHP(player);
        CheckPlayerDeath();

        if (cellClicked.gameObject == null)
        {
            PrevCell = OnCellActivated(cellClicked, PrevCell);
        }
    }

    public void GetHealToOpenEnemies(int healpoints)
    {
        foreach (Enemy enemy in OpenEnemy)
        {
            enemy.HitPoints += healpoints;
        }
    }

    public int CheckTypeOfEnemy()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(1, 3); //1-DD, 2-Healer, 3-Tank
    }

    private void CheckPlayerDeath()
    {
        if (player.HitPoints <= 0)
        {
            UIController.ShowPlayerHP(player);
            var text = "Game Over!";
            UIController.ShowWinLooseInformation(text);
        }
    }

    private void MovePlayer(EmptyCell cell)
    {
        player.Relocation(cell.transform.position);
    }

    public void OnContentShown(CellContent content)
    {
        if (content is Enemy enemy)
        {
            OpenEnemy.Add(enemy);
        }
    }
    
    public void OnUseBonus(Bonus bonus, ItemSlot button)
    {
        bonus.OnContentApplied(player, OpenEnemy);
        UpdatePlayerInformation();
        button.UseBonus -= OnUseBonus;
        UIController.RemoveBonusButton(button);
    }

    private void UpdatePlayerInformation()
    {
        UIController.ShowPlayerHP(player);
        UIController.ShowPlayerDMG(player);
    }

    private void MoveBonusIntoBonusCell(Bonus bonus, BaseCell cellClicked)
    {
        var BonusButton = UIController.RelocateBonusIntoBonusCell(bonus, player, cellClicked);
        if (BonusButton == null) return;
        bonus.gameObject.SetActive(false);
        BonusButton.UseBonus += OnUseBonus;
        OnCellActivated(cellClicked, PrevCell);
    }
    public void OnCellClicked(BaseCell cellClicked)
    {
        if (!IsNeighbor(cellClicked)) return;

        var emptyCell = (EmptyCell)cellClicked;

        if (!emptyCell.Opened)
        {
            emptyCell.ShownContent += OnContentShown;
            emptyCell.ShowContent(WSCanvas);
            emptyCell.ShownContent -= OnContentShown;
            emptyCell.Opened = true;
            if (emptyCell.ContentLink  is Bonus bonus)
            {
                bonus.ParentCell = cellClicked;
                bonus.MoveBonusIntoBonusCell += MoveBonusIntoBonusCell;
            }
            if (!(emptyCell.ContentLink == null))
            {
                return;
            }
        }

        if (emptyCell.ContentLink == null)
        {           
            emptyCell.Opened = true;
            if (cellClicked == mGenerator.EndCell)
            {   
                cellClicked.SetMaterial(materials[5]);
                playerProgress.Lvl++;
                SaveProgress();
                UIController.ShowPlayerHP(player);
                var text = "You Win!";
                UIController.ShowWinLooseInformation(text);

            }
                PrevCell = OnCellActivated(cellClicked, PrevCell);
            //return;
        }
        else
        {
            cellUsed(emptyCell);
        }
        foreach (CellContent enemy in OpenEnemy)
        {
            enemy.OnAnyCellClicked(OpenEnemy);
        }
    }

    public void OnStartClicked()
    {
        MenuPanel.SetActive(false);
    }

    private void SaveProgress()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, playerProgress);
        }
    }

    private void LoadProgress()
    {
        BinaryFormatter formatter = new BinaryFormatter();
     
        if (!File.Exists(filePath))
        {
            playerProgress = new PlayersProgress();
            return;
        }

        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            playerProgress = (PlayersProgress)formatter.Deserialize(stream);
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
            if (!(cell == null))
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

    private void SetNeighbornsMaterial(BaseCell cellClicked)
    {

        neighbors = GetAvailableCells(cellClicked);
        foreach (var neighbor in neighbors)
        {
            var empty = (EmptyCell)neighbor;

            if (empty.CellType == CellType.StartCell)//StartCell
            {
                continue;
            }
           // if (empty.CellType == CellType.EndCell)
           // {
              //  neighbor.SetMaterial(materials[5]);
               // continue;
          //  } 
            neighbor.SetMaterial(materials[1]);
        }
    }


    private void ActivateCellMaterial(BaseCell cellClicked)
    {
       /*Material material;
        if (cellClicked is EnemyCell enemy)
        {
            material = materials[3];
        }
        else if (cellClicked is BonusCell bonus)
        {
            material = materials[2];
        }
        else if (cellClicked is EmptyCell empty)
        {
            if (empty.CellType == CellType.EndCell)//EndCell
            {
                material = materials[5];
            }
            else
            {
                material = materials[4];
            }
        }
        else
        {*/
          //  material = materials[4];
        //}
        cellClicked.SetMaterial(materials[4]);
    }

    private BaseCell OnCellActivated(BaseCell cellClicked, BaseCell PrevCell)
    {
        
           //PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
        //Bar.fillAmount = (float)player.HitPoints / 100;
        playerPositionInMap = cellClicked.CellIndex;

        player.Relocation(cellClicked.transform.position);

        cellClicked.Open = true;

        //ActivateCellMaterial(cellClicked);
        SetLockedMaterial(PrevCell, cellClicked);
        SetNeighbornsMaterial(cellClicked);
       
       PrevCell = cellClicked;
       return PrevCell;
        //cellClicked.Activate();
    }

    private void SetLockedMaterial(BaseCell PrevCell, BaseCell cellClicked)
    {
        neighbors = GetAvailableCells(PrevCell);
        foreach (var neighbor in neighbors)
        {
            var empty = (EmptyCell)neighbor;

            if (neighbor.Open) continue;
            if (empty.CellType==CellType.StartCell) continue;
            if (neighbor == cellClicked) continue;

            neighbor.SetMaterial(materials[0]);
        }

    }
}
