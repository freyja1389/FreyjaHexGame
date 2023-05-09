using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Image enemyHitBar;
    [SerializeField]
    private Text GameOverTextBar;
    [SerializeField]
    private Text UnitType;
    [SerializeField]
    private Text EnemyInfoPref;
    [SerializeField]
    private Canvas WSCanvas;

    public Image PlayerHPBar;
    public Text PlayersHPTextBox;
    public Text PlayersDMGTextBox;

    public ItemSlot ButtonBonusDMGBooster;
    public ItemSlot ButtonBonusEnemyHPReducer;
    public ItemSlot ButtonBonusHealer;

    public GameObject ItemsPanel;

    public List<ItemSlot> BonusButtons;

    public InGameMenuControls MenuControls;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeEnemyHitBarFillAmount(int currentHp, int baseHp)
    {
        if (currentHp == baseHp)
        {
            enemyHitBar.fillAmount = 100; //enemy has 100% HP
        }
        else
        {
            enemyHitBar.fillAmount = (float)currentHp / (float)baseHp;
        }
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

    public void ShowWinLooseInformation(string winLooseText)
    {
        //var instGameOverText = Instantiate(GameOverTextBar, new Vector3(0, 184, 0), Quaternion.identity);
        //var canv = GameObject.FindGameObjectWithTag("MainCanvas");
        //instGameOverText.transform.SetParent(canv.transform, false);
        //var text = GameOverTextBar.GetComponent<Text>();
        GameOverTextBar.text = winLooseText;
        GameOverTextBar.gameObject.SetActive(true);
    }

    public void DeActivateGameOverTextBar()
    {
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
            UIController enemyBar = Instantiate(enemy.EnemyHitBarPref, new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z), Quaternion.Euler(90, 0, 0), WSCanvas.transform);
            enemyBar.ChangeEnemyHitBarFillAmount(enemy.CurrentHitPoints, enemy.BasetHitPoints);
            enemy.HitBar = enemyBar;

            Text EnemyInfo = Instantiate(EnemyInfoPref, new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z + 0.3f), Quaternion.Euler(90, 0, 0), WSCanvas.transform);
            //EnemyInfo.text = "HP: " + enemy.CurrentHitPoints + "\n DMG: " + enemy.DmgPoints;
            EnemyInfo.text = $"HP: {enemy.CurrentHitPoints}\n DMG: {enemy.DmgPoints}";
            enemy.EnemyInfo = EnemyInfo;
        }
    }

    public void NextLevelMenuSpawn()
    {
      MenuControls.NextLevelMenuPanel.SetActive(true);
    }

}
