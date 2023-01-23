using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Image enemyHitBar;
    [SerializeField]
    private GameObject GameOverTextBar;

    public Image PlayerHPBar;
    public Text PlayersHPTextBox;
    public Text PlayersDMGTextBox;

    public ItemSlot ButtonBonusDMGBooster;
    public ItemSlot ButtonBonusEnemyHPReducer;
    public ItemSlot ButtonBonusHealer;

    public GameObject ItemsPanel;

    public List<ItemSlot> BonusButtons;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHitBarFillAmount(int hp)
    {
        enemyHitBar.fillAmount = (float)hp / 100;
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
            var empty = (EmptyCell)cellClicked;
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
        var instGameOverText = Instantiate(GameOverTextBar, new Vector3(0, 0, 0), Quaternion.identity);
        var canv = GameObject.FindGameObjectWithTag("MainCanvas");
        instGameOverText.transform.SetParent(canv.transform, false);
        var text = instGameOverText.GetComponent<Text>();
        text.text = winLooseText;
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

}
