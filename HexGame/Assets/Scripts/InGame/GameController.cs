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

    // public UIController EnemyHitBarPref;
    
   // public Canvas UICanvas;
    public UIController UIController;
    public Player player;

    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private CellMaterials cellMaterials;
    [SerializeField]
    private GameMenuConstants gameMenuConstants;

    //private List<BaseCell> neighbors;
    private Vector2Int playerPositionInMap;
    private PlayersProgress _playerProgress;
    public PlayersProgress playerProgress { get { return _playerProgress;  }}
    private string filePath => Application.persistentDataPath + @"\ProgressData";
    private BaseCell PrevCell;
    
    public DamageSTM damageAnimSTM;
    private Map map;
    private GameSceneManager gameSceneManager;


    private void Awake()
    {
        gameSceneManager = GameSceneManager.GetInstance();
        gameSceneManager.GameController = this;
    }
    void Start()
    {
        Debug.Log("game Controller");
        //Menu.StartClicked += OnStartClicked; //// subcribe on click start not needed
        SetUILinks(gameSceneManager);
        StartLevel();
        //Menu.NextLevelClicked += StartLevel;
    }


    private void SetUILinks(GameSceneManager gameSceneManager)
    {
        UIController = gameSceneManager.UIController;
        Menu = UIController.Menu;

    }

    private void StartLevel()
    {
        //ClearTheMap();
        if (UIController == null)
        {
            SetUILinks(gameSceneManager);
        }

        LoadProgress();

        UIController.ReInit();

        UIController.UpdateLvlInfo(playerProgress.Lvl);

        map = mapGenerator.MapGenerate(Rows, Columns, player, _playerProgress.Lvl);

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
       // var bonusButtons = UIController.GetBonusButtons();
       // foreach (ItemSlot bonusButton in bonusButtons)
       //{
          //  bonusButton.UseBonus += OnUseBonus;
        //}
    }


    public void ClearTheMap()
    {
        if (mapGenerator is null) return;
        mapGenerator.Clear();
        map = null;
    }

    public void CellUsed(BaseCell clickedCell)
    {
        var content = clickedCell.ContentLink;

        if(content is Enemy enemy)
        {
            enemy.StateChanged += UIController.UpdateEnemyTextInfo;
            enemy.EnemyAttackCompleted += CheckPlayerDeath;
           // enemy.EnemyAttackCompleted += player.SetAttackAnimation;
        }
        content.OnContentClicked(player, map.OpenEnemy, clickedCell);   //rename its not event!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //CheckPlayerDeath();

        if (clickedCell.gameObject is null)
        {
            PrevCell = OnCellActivated(clickedCell, PrevCell);         //rename its not event - if dead - why execute?!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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

        player.RelocateInstantly(new Vector3(map.StartCell.transform.position.x, map.StartCell.transform.position.y+0.1f, map.StartCell.transform.position.z));
        playerPositionInMap = map.StartCell.CellIndex;

        var animPlayer = player.GetComponentInChildren<Animator>();
        damageAnimSTM = animPlayer.GetBehaviour<DamageSTM>();
    }

    private void CheckPlayerDeath(int dmg)
    {
        if (player.HitPoints <= 0)
        {
            UIController.ShowPlayerHP(player);
            OnLoose();
        }
    }

    private void MovePlayer(BaseCell cell)
    {
        player.Relocation(cell.transform.position);
    }

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


        if (!cellClicked.Opened)
        {
            cellClicked.Opened = true;
            cellClicked.ShowContent();  

            if (cellClicked.ContentLink is Bonus bonus)
            {
                bonus.ParentCell = cellClicked;
            }
            else if (cellClicked.ContentLink is Enemy enemy)
            {
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
                player.PlayerRelocated += OnWin;
                player.Relocation(cellClicked.transform.position);
                cellClicked.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.EndMaterial));
                _playerProgress.Lvl++;
                UIController.ShowPlayerHP(player);
            }
            else
            {
                PrevCell = OnCellActivated(cellClicked, PrevCell);
            }
        }
        else
        {
            CellUsed(cellClicked);
        }

        map.OpenEnemyOnCellClicked();
    }

    private void OnWin()
    {
        UIController.ShowWinLooseInformation(gameMenuConstants.WinMessage);
        UIController.NextLevelMenuSpawn(true);
        SaveProgress();
    }

    private void OnLoose()
    {
        UIController.ShowWinLooseInformation(gameMenuConstants.LooseMessage);
        UIController.NextLevelMenuSpawn(false);
    }



    public void OnStartClicked()
    { 
        StartLevel();
    }

    private void SaveProgress()
    {
        var formatter = new BinaryFormatter();

        using var stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, _playerProgress);
    }

    private void LoadProgress()
    {
        var formatter = new BinaryFormatter();

        if (!File.Exists(filePath))
        {
            _playerProgress = new PlayersProgress();
            return;
        }

        using var stream = new FileStream(filePath, FileMode.Open);
        _playerProgress = (PlayersProgress)formatter.Deserialize(stream);
    }

    

   

    private void SetNeighbornsMaterial(BaseCell cellClicked) //incapsulate to Map class
    {

        //neighbors = map.GetAvailableCells(cellClicked);
        foreach (var neighbor in map.GetAvailableCells(cellClicked))
        {
           // var empty = (EmptyCell)neighbor;

            if (neighbor.ContentType == CellType.StartCell)//StartCell
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
            if (neighbor.ContentType == CellType.StartCell) continue;
            if (neighbor == cellClicked) continue;

            neighbor.SetMaterial(cellMaterials.GetMaterial(CellMaterials.MaterialNames.Locked));
        }

    }

    public void OnDestroy()
    {
        Debug.Log("GameController destroy");
    }
}
