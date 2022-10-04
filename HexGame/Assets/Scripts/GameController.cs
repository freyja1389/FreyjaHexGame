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
        Bar.fillAmount = 1f;
        //HexCells = mGenerator.MapCreate1(Rows, Columns, mGenerator.transform, player, playerProgress.Lvl);
        HexCells = mGenerator.MapGenerate(Rows, Columns, mGenerator.transform, player, playerProgress.Lvl);
        neighbors = GetAvailableCells(mGenerator.StartCell);
        foreach (var neighbor in neighbors)
        {
            if (!(neighbor is EmptyCell empty && empty.StartCell))
            {
                neighbor.SetMaterial(materials[1]);
            }
        }

        player.ChangePosition(mGenerator.StartCell.transform.position);
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

    public void OnContentClicked(CellContent content)
    {
        if (content is Bonus bonus)
        {
            player.SetHeal(bonus.HealPoints);
            Destroy(content.gameObject);
        }
        else if (content is Enemy enemy)// enemy
        {
            player.SetDamage(enemy.DmgPoints);
            
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
                if (enemy.HitPoints <= 0)
                {
                    Destroy(content.gameObject);
                }
                else
                {
                    PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
                    Bar.fillAmount = (float)player.HitPoints / 100;
                }
            }
        }
    }

    public void OnCellClicked(BaseCell cellClicked)
    {
        if (IsNeighbor(cellClicked))
        {
            if (cellClicked is EmptyCell emptyenemy && emptyenemy.EnemyCell)
            {
                //заспавним сюда врага, если €чейка еще не открыта
                if (!emptyenemy.Opened)
                {
                    Enemy enemy = Instantiate((Enemy)mGenerator.CellsContent[1], cellClicked.transform.position, cellClicked.transform.rotation, transform);
                    emptyenemy.ContentLink = enemy;
                    emptyenemy.Opened = true;
                    enemy.ContentClicked += OnContentClicked;
                }
               /* else
                {
                    Enemy enemy = (Enemy)emptyenemy.ContentLink;

                    player.SetDamage(enemy.DmgPoints);
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
                        if (enemy.HitPoints <= 0)
                        {
                            PrevCell = OnCellActivated(cellClicked, PrevCell);
                        }
                        else
                        {
                            ActivateCellMaterial(cellClicked);
                            PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
                            Bar.fillAmount = (float)player.HitPoints / 100;
                        }
                    }
                }*/
            }
            else if (cellClicked is EmptyCell emptybonus && emptybonus.BonusCell)
            {
                if (!emptybonus.Opened)
                {
                    Bonus bonus = Instantiate((Bonus)mGenerator.CellsContent[0], cellClicked.transform.position, cellClicked.transform.rotation, transform);
                    emptybonus.ContentLink = bonus;
                    emptybonus.Opened = true;
                    bonus.ContentClicked += OnContentClicked;
                }
                else
                {
                    Bonus bonus = (Bonus)emptybonus.ContentLink;
                    player.SetHeal(bonus.HealPoints);
                    PrevCell = OnCellActivated(cellClicked, PrevCell);
                }
            }
            else if (cellClicked is EmptyCell empty)
            {
                if (empty.EndCell)
                {
                    var instGameOverText = Instantiate(GameOverTextBar, new Vector3(0, 0, 0), Quaternion.identity);
                    var canv = GameObject.FindGameObjectWithTag("MainCanvas");
                    instGameOverText.transform.SetParent(canv.transform, false);
                    var text = instGameOverText.GetComponent<Text>();
                    if (player.HitPoints > 0)
                    {
                        text.text = "You Win!";
                        playerProgress.Lvl++;
                        SaveProgress();
                    }
                    else
                    {
                        PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
                        Bar.fillAmount = (float)player.HitPoints / 100;
                        text.text = "Game Over!";
                    }
                }
                else
                {
                    PrevCell = OnCellActivated(cellClicked, PrevCell);
                }
            }
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
        else if (cellClicked is EmptyCell empty)
        {
            if (empty.EndCell)
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
        
        PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
        Bar.fillAmount = (float)player.HitPoints / 100;
        playerPositionInMap = cellClicked.CellIndex;

        player.Relocation(cellClicked.transform.position);

        cellClicked.Open = true;

        //ActivateCellMaterial(cellClicked);

        SetNeighbornsMaterial(cellClicked);

        if (PrevCell is EmptyCell empty)
        {
            if (!empty.StartCell)
            {
                PrevCell.SetMaterial(materials[4]);
            }
        }
        else
        {
            PrevCell.SetMaterial(materials[4]);
        }
       PrevCell = cellClicked;
       return PrevCell;
        //cellClicked.Activate();
    }
}
