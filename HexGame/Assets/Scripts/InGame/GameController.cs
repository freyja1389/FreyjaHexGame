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
    public StateMachineBehaviour AnimAttack;
    //public Text PlayersHPTextBox;
    //public List<BaseCell> HexCells;
   // public BaseCell[,] HexCells;
    public int Rows = 0;
    // public Image Bar;
    
    public BaseMenuControls Menu;
    public Text LvlInfo;
    // public UIController EnemyHitBarPref;
    
    public Canvas UICanvas;
    public UIController UIController;
    public Player player;

    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private CellMaterials cellMaterials;
    [SerializeField]
    private GameMenuConstants gameMenuConstants;
    [SerializeField]
    private SceneManager sceneManager;

    //private List<BaseCell> neighbors;
    private Vector2Int playerPositionInMap;
    private PlayersProgress playerProgress;
    private string filePath => Application.persistentDataPath + @"\ProgressData";
    private BaseCell PrevCell;
    
    public DamageSTM damageAnimSTM;
    private Map map;


    void Start()
    {
        //Menu.StartClicked += OnStartClicked; //// subcribe on click start not needed
        StartLevel();
    }

    private void StartLevel()
    {
        ClearTheMap();
      
        LoadProgress();

        UIController.ReInit();

        map = mapGenerator.MapGenerate(Rows, Columns, player, playerProgress.Lvl);

        map.StartCell.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.StartMaterial));

        foreach (var neighbor in map.GetAvailableCells(map.StartCell))
        {
            neighbor.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.Unlocked));
        }

        InitPlayer();

        map.CellClicked += OnCellClicked;
        map.ContentShown += UIController.ViewContentInformation;
        map.BonusClicked += MoveBonusIntoBonusCell;

        PrevCell = map.StartCell;
    }


    public void ClearTheMap()
    {
        if (mapGenerator is null) return;
        mapGenerator.Clear();
    }

    public void CellUsed(BaseCell clickedCell)
    {
        var content = clickedCell.ContentLink;

        if(content is Enemy enemy)
        {
            enemy.StateChanged += UIController.UpdateEnemyTextInfo;
        }
        content.OnContentClicked(player, map.OpenEnemy, clickedCell);//rename its not event!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        CheckPlayerDeath();

        if (clickedCell.gameObject is null)
        {
            PrevCell = OnCellActivated(clickedCell, PrevCell); //rename its not event - if dead - why execute?!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }

    public void GetHealToOpenEnemies(int healpoints)
    {
        foreach (Enemy enemy in map.OpenEnemy)
        {
            enemy.SetHeal(healpoints);
        }
    }

    public int CheckTypeOfEnemy()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(1, 3); //1-DD, 2-Healer, 3-Tank
    }

    private void InitPlayer()
    {
        player.SetHitDamagePoints();
        UpdatePlayerInformation();

        player.RelocateInstantly(map.StartCell.transform.position);
        playerPositionInMap = map.StartCell.CellIndex;

        var animPlayer = player.GetComponentInChildren<Animator>();
        damageAnimSTM = animPlayer.GetBehaviour<DamageSTM>();
    }

    private void CheckPlayerDeath()
    {
        if (player.HitPoints <= 0)
        {
            UIController.ShowPlayerHP(player);
            //var text = "Game Over!";
            UIController.ShowWinLooseInformation(gameMenuConstants.LooseMessage);
        }
    }

    private void MovePlayer(BaseCell cell)
    {
        player.Relocation(cell.transform.position);
    }

    /*public void OnContentShown(CellContent content)
    {
        if (content is Enemy enemy)
        {
            OpenEnemy.Add(enemy); //- relocated to Map CellShownContent func
        }
    }*/

    public void OnUseBonus(Bonus bonus, ItemSlot button)
    {
        bonus.OnContentApplied(player, map.OpenEnemy);
        UpdatePlayerInformation();
        UpdateOpenEnemyInformation();
        button.UseBonus -= OnUseBonus;
        UIController.RemoveBonusButton(button);
    }

    public void UpdateOpenEnemyInformation()
    {
        foreach (Enemy enem in map.OpenEnemy)
        {
            enem.HitBar.ChangeEnemyHitBarFillAmount(enem.CurrentHitPoints, enem.BasetHitPoints);
            UIController.UpdateEnemyTextInfo(enem);
        }

    }

    private void UpdatePlayerInformation()
    {
        UIController.ShowPlayerHP(player);
        UIController.ShowPlayerDMG(player);
        player.UIController = UIController;
    }

    private void MoveBonusIntoBonusCell(Bonus bonus, BaseCell cellClicked)
    {
        var BonusButton = UIController.RelocateBonusIntoBonusCell(bonus, player, cellClicked);
        if (BonusButton == null) return;
        bonus.ActivateGameObject(false);
        BonusButton.UseBonus += OnUseBonus;
        OnCellActivated(cellClicked, PrevCell);
    }

 

    public void OnCellClicked(BaseCell cellClicked)
    {
        if (!player.PlayerCanAction()) return;

        if (!map.IsNeighbor(cellClicked, playerPositionInMap)) return;

        player.transform.LookAt(cellClicked.transform.position);


        //var emptyCell = (EmptyCell)cellClicked;

        if (!cellClicked.Opened)
        {
            cellClicked.Opened = true;
            cellClicked.ShowContent();  

            if (cellClicked.ContentLink is Bonus bonus)
            {
                bonus.ParentCell = cellClicked;
                //bonus.MoveBonusIntoBonusCell += MoveBonusIntoBonusCell;
            }
            else if (cellClicked.ContentLink is Enemy enemy)
            {
                // damageAnimSTM.DamageAnimationComplete += enemy.CheckEnemyDeath;
                //player.CheckEnemyDeath += enemy.CheckEnemyDeath;
                //enemy.EnemyAlive += player.SetDamageWithAnimation;
                enemy.EnemyAttackStarted += player.SetDamageAnimation;
                enemy.EnemyAttackCompleted += player.SetDamage;

            }
            if (cellClicked.ContentLink is not null) return;
        }

        if (cellClicked.ContentLink == null)
        {
            cellClicked.Opened = true;
            if (cellClicked == map.EndCell)// вынести в класс 2карта", и сделать подпиской
            {
                player.Relocation(cellClicked.transform.position);
                cellClicked.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.EndMaterial));
                playerProgress.Lvl++;
                UIController.ShowPlayerHP(player);
                //var text = "You Win!";
                UIController.ShowWinLooseInformation(gameMenuConstants.WinMessage);
                UIController.NextLevelMenuSpawn();
                SaveProgress();

            }
            else
            {
                PrevCell = OnCellActivated(cellClicked, PrevCell);
            }
            //return;
        }
        else
        {
            CellUsed(cellClicked);
        }

        map.OpenEnemyOnCellClicked();
    }



    public void OnStartClicked() //  ////////////////////////////////////////
    {
       // Menu.ActivateMenuPanel(false);
       // Menu.ActivateBackMenuPanel(true);
        StartLevel();
    }

    private void SaveProgress()
    {
        var formatter = new BinaryFormatter();

        using var stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, playerProgress);
    }

    private void LoadProgress()
    {
        var formatter = new BinaryFormatter();

        if (!File.Exists(filePath))
        {
            playerProgress = new PlayersProgress();
            return;
        }

        using var stream = new FileStream(filePath, FileMode.Open);
        playerProgress = (PlayersProgress)formatter.Deserialize(stream);
    }

    

   

    private void SetNeighbornsMaterial(BaseCell cellClicked) //incapsulate to Map class
    {

        //neighbors = map.GetAvailableCells(cellClicked);
        foreach (var neighbor in map.GetAvailableCells(cellClicked))
        {
           // var empty = (EmptyCell)neighbor;

            if (neighbor.CellType == CellType.StartCell)//StartCell
            {
                continue;
            }
            // if (empty.CellType == CellType.EndCell)
            // {
            //  neighbor.SetMaterial(materials[5]);
            // continue;
            //  } 
            neighbor.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.Unlocked));
        }
    }


    private void ActivateCellMaterial(BaseCell cellClicked)
    {

        cellClicked.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.EmptyMaterial));
    }

    private BaseCell OnCellActivated(BaseCell cellClicked, BaseCell PrevCell) // is not  subscribtion to event
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

    private void SetLockedMaterial(BaseCell prevCell, BaseCell cellClicked) //its MAP
    {
        //var neighbors = map.GetAvailableCells(PrevCell);
        foreach (var neighbor in map.GetAvailableCells(prevCell))
        {
           // var empty = (EmptyCell)neighbor;

            if (neighbor.Open) continue;
            if (neighbor.CellType == CellType.StartCell) continue;
            if (neighbor == cellClicked) continue;

            neighbor.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.Locked));
        }

    }
}
