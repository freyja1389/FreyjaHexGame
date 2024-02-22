using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : MonoBehaviour
{
    [SerializeField]
    private Image enemyHitBar;

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
}
