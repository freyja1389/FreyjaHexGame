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
    public Text PlayersHPTextBox;
    //public List<BaseCell> HexCells;
    public BaseCell[,] HexCells;
    public int Rows = 0;
    public Image Bar;
    public GameObject MenuPanel;
    public MenuControls Menu;
    public Text LvlInfo;
    // public UIController EnemyHitBarPref;
    public Canvas WSCanvas;

    [SerializeField]
    private MapGenerator mGenerator;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private GameObject GameOverTextBar;

    private List<BaseCell> neighbors;
    private Vector2Int playerPositionInMap;
    private PlayersProgress playerProgress;
    private string filePath;
    private BaseCell PrevCell;
    public List<Enemy> OpenEnemy;

    public readonly List<Vector2Int> neighborRulesEvenY = new List<Vector2Int> //чет y
    {
         new Vector2Int(-1, 1),
         new Vector2Int(0, -1),
         new Vector2Int(1, 0),
         new Vector2Int(0, 1),
         new Vector2Int(-1, -1),
         new Vector2Int(-1, 0),
    };
    public readonly List<Vector2Int> neighborRulesOddY = new List<Vector2Int> //нечет y
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
        Bar.fillAmount = 1f;
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

    public void cellUsed(CellContent content)
    {
        if (content is Bonus bonus)
        {
            player.SetHeal(bonus.HealPoints);
            PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
            Bar.fillAmount = (float)player.HitPoints / 100;
            Destroy(content.gameObject);
        }
        else if (content is Enemy enemy)// enemy
        {
            player.SetDamage(enemy.DmgPoints);
            if (enemy is EnemyHealer)
            {
                EnemyHealer healer = (EnemyHealer)enemy;
                GetHealToOpenEnemies(healer.HealPoints);
            }
            PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
            Bar.fillAmount = (float)player.HitPoints / 100;

            if (player.HitPoints <= 0)
            {
                var instGameOverText = Instantiate(GameOverTextBar, new Vector3(0, 0, 0), Quaternion.identity);
                var canv = GameObject.FindGameObjectWithTag("MainCanvas");
                instGameOverText.transform.SetParent(canv.transform, false);
                var text = instGameOverText.GetComponent<Text>();
                text.text = "Game Over!";
            }
            else
            {
                enemy.SetDamage(player.DmgPoints);
                //enemy.HitBar.GetComponentsInChildren<Image>()[1].fillAmount = (float)enemy.HitPoints / 100;
                enemy.HitBar.ChangeHitBarFillAmount(enemy.HitPoints);
                if (enemy.HitPoints <= 0)
                {
                    var enemyCont = content as Enemy;
                    Destroy(enemyCont.HitBar.gameObject);
                    Destroy(content.gameObject);
                    OpenEnemy.Remove(enemy);
                }
                /*else
                {
                    PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
                    Bar.fillAmount = (float)player.HitPoints / 100;
                }*/
            }
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
        if (player.HitPoints < 0)
        {
            var instGameOverText = Instantiate(GameOverTextBar, new Vector3(0, 0, 0), Quaternion.identity);
            var canv = GameObject.FindGameObjectWithTag("MainCanvas");
            instGameOverText.transform.SetParent(canv.transform, false);
            var text = instGameOverText.GetComponent<Text>();

            PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
            Bar.fillAmount = (float)player.HitPoints / 100;
            text.text = "Game Over!";
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
    
    public void OnCellClicked(BaseCell cellClicked)
    {
        if (!IsNeighbor(cellClicked)) return;

        var emptyCell = (EmptyCell)cellClicked;

        if (emptyCell.ContentLink == null)
        {
            MovePlayer(emptyCell);//move player here
            emptyCell.Opened = true;
            return;
        }

        if (!emptyCell.Opened)
        {
            emptyCell.ShownContent += OnContentShown;
            emptyCell.ShowContent(WSCanvas);
            emptyCell.ShownContent -= OnContentShown;
            emptyCell.Opened = true;
            // emptyCell.ContentLink.ContentClicked += OnContentClicked;
            return;
        }

       

        cellUsed(emptyCell.ContentLink);

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
        /*foreach (var neighbor in neighbors)
        {
            if (!neighbor.Open)
            {
                if ((neighbor is EmptyCell empty))
                {
                    if (!(empty.CellType == EmptyCell.CellTypes.StartCell) & !(empty.CellType == EmptyCell.CellTypes.EndCell))
                    {
                        neighbor.SetMaterial(materials[0]);
                    }
                }
                else
                {
                    neighbor.SetMaterial(materials[0]);
                }
            }
        }*/
        neighbors = GetAvailableCells(cellClicked);
        foreach (var neighbor in neighbors)
        {
            if (!neighbor.Open)
            {
                if ((neighbor is EmptyCell empty))
                {
                    if (empty.CellType == CellType.StartCell)//StartCell
                    {
                        continue; 
                    }
                    if (empty.CellType == CellType.EndCell)//StartCell
                    {
                        continue;
                    }
                    neighbor.SetMaterial(materials[1]);
                }
                /*else
                {
                    neighbor.SetMaterial(materials[1]);
                }*/
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
        {
            material = materials[4];
        }
        cellClicked.SetMaterial(material);
    }

    private BaseCell OnCellActivated(BaseCell cellClicked, BaseCell PrevCell)
    {
        
           //PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
        //Bar.fillAmount = (float)player.HitPoints / 100;
        playerPositionInMap = cellClicked.CellIndex;

        player.Relocation(cellClicked.transform.position);

        cellClicked.Open = true;

        //ActivateCellMaterial(cellClicked);

        SetNeighbornsMaterial(cellClicked);

        if (cellClicked is EmptyCell empty)
        {
            if (empty.CellType == CellType.StartCell)//StartCell
            {
                cellClicked.SetMaterial(materials[6]);
            }
            if (empty.CellType == CellType.EndCell)//EndCell
            {
                cellClicked.SetMaterial(materials[5]);
            }
        }
        else
        {
            cellClicked.SetMaterial(materials[4]);
        }
       PrevCell = cellClicked;
       return PrevCell;
        //cellClicked.Activate();
    }
}
