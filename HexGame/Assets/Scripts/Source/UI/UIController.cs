using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    //[SerializeField]
    //private Image enemyHitBar;
    [SerializeField]
    private Text GameOverTextBar;
    [SerializeField]
    private Text UnitType;
    [SerializeField]
    private Text EnemyInfoPref;
    [SerializeField]
    private Canvas WSCanvas;
    [SerializeField]
    private Text LvlInfo;

    public Image PlayerHPBar;
    public Text PlayersHPTextBox;
    public Text PlayersDMGTextBox;

    public ItemSlot ButtonBonusDMGBooster;
    public ItemSlot ButtonBonusEnemyHPReducer;
    public ItemSlot ButtonBonusHealer;

    public GameObject ItemsPanel;
    public BaseMenuControls Menu;

    public List<ItemSlot> BonusButtons;

    public InGameMenuControls MenuControls;

    //public InventoryManager InventoryManager;

    //public InventoryManager InventoryManagerPrefab;



    // Start is called before the first frame update
    void Awake()
    {
        var instance = GameSceneManager.GetInstance();
        instance.UIController = this;
    }

    void Start()
    {
        MenuControls.NextLevelClicked += WhenNextLevelClicked;

    }


    public void UpdateLvlInfo(int lvl)
    {
        LvlInfo.text = "Level: " + lvl.ToString();
    }

    

    public void UpdateEnemyTextInfo(Enemy enemy)
    {
        enemy.EnemyInfo.text = "HP: " + enemy.CurrentHitPoints + "\n DMG: " + enemy.DmgPoints;
    }

    public ItemSlot RelocateBonusIntoBonusCell(Bonus bonus, Player player, BaseCell cellClicked)
    {
        ItemSlot inst = null;
        if (BonusButtons.Count < 3)
        {
            if (bonus is BonusHealer bonusHealer)
            {
                inst = Instantiate(ButtonBonusHealer);
            }
            else if (bonus is BonusDMGBooster bonusDMGBooster)
            {
                inst = Instantiate(ButtonBonusDMGBooster);
            }
            else if (bonus is BonusEnemyHPReducer bonusEnemyHPReducer)
            {
                inst = Instantiate(ButtonBonusEnemyHPReducer);
            }
            SetBonusButtonSettings(inst, bonus);
            AddBonusButton(inst);
            var empty = cellClicked;
            bonus.Unsubscribe(empty);
            empty.ContentLink = null;
            Destroy(bonus.gameObject);
        }
        return inst;
    }

    private void SetBonusButtonSettings(ItemSlot inst, Bonus bonus)
    {
        inst.BonusLink = bonus;
        inst.transform.SetParent(ItemsPanel.transform);
        inst.transform.localScale = new Vector3(1, 1, 1);
    }


    public void ShowPlayerHP(Player player)
    {
        PlayersHPTextBox.text = "Player's HP: " + player.HitPoints;
        PlayerHPBar.fillAmount = (float)player.HitPoints / 100;
    }

    public void ShowPlayerDMG(Player player)
    {
        PlayersDMGTextBox.text = "Player's DMG: " + player.DmgPoints;
    }

    public void ShowWinLooseInformation(string winLooseText) //remove scrobj into UI controller , use bool to get ifo from scrobj
    {
        //var instGameOverText = Instantiate(GameOverTextBar, new Vector3(0, 184, 0), Quaternion.identity);
        //var canv = GameObject.FindGameObjectWithTag("MainCanvas");
        //instGameOverText.transform.SetParent(canv.transform, false);
        //var text = GameOverTextBar.GetComponent<Text>();
        GameOverTextBar.text = winLooseText;
        GameOverTextBar.gameObject.SetActive(true);
    }

    public void ReInit()
    {
       // LoadBonusesFromInventory();
        GameOverTextBar.gameObject.SetActive(false);
    }

    private void AddBonusButton(ItemSlot bonusButton)
    {
        if (BonusButtons.Count < 3)
        {
            BonusButtons.Add(bonusButton);
        }
    }

    public void RemoveBonusButton(ItemSlot button)
    {
        BonusButtons.Remove(button);
    }

    public void ViewContentInformation(CellContent content)//, EmptyCell cell)
    {
        if (content is Enemy enemy)
        {
            SceneUI enemyBar = Instantiate(enemy.EnemyHitBarPref, new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z), Quaternion.Euler(90, 0, 0), WSCanvas.transform);
            enemyBar.ChangeEnemyHitBarFillAmount(enemy.CurrentHitPoints, enemy.BasetHitPoints);
            enemy.HitBar = enemyBar;

            Text EnemyInfo = Instantiate(EnemyInfoPref, new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z + 0.3f), Quaternion.Euler(90, 0, 0), WSCanvas.transform);
            //EnemyInfo.text = "HP: " + enemy.CurrentHitPoints + "\n DMG: " + enemy.DmgPoints;
            EnemyInfo.text = $"HP: {enemy.CurrentHitPoints}\n DMG: {enemy.DmgPoints}";
            enemy.EnemyInfo = EnemyInfo;
        }
    }

    public void NextLevelMenuSpawn(bool win)
    {
        if (win)
        {
            MenuControls.ChangeNextLevelMenuButtonText("Next level");
        }
        else
        {
            MenuControls.ChangeNextLevelMenuButtonText("Retry level");
        }
        MenuControls.NextLevelMenuPanel.SetActive(true);
    }

    public void LoadBonusesFromInventory()
    {
        foreach (ItemSlot button in BonusButtons)
        {
            if (BonusButtons.Count < 3)
            {
                //AddBonusButton(button);
                button.transform.SetParent(ItemsPanel.transform);
                button.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public List<ItemSlot> GetBonusButtons()
    {
        return BonusButtons;   
    }

    private void WhenNextLevelClicked()
    {
        DeleteAllEnemyUIElements();
    }

    private void DeleteAllEnemyUIElements()
    {
        var childscount = WSCanvas.transform.childCount;
        var childList = new List<GameObject>();

        for (int i = 0; i < childscount; i++)
        {
            childList.Add(WSCanvas.transform.GetChild(i).gameObject);
        }

        foreach (var item in childList)
        {
            Destroy(item);
        }

    }

    public void OnDestroy()
    {
        Debug.Log("UIController destroy");
    }

    
}
