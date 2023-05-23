using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellContent : MonoBehaviour
{
    public event Action ContentClicked;
    public  event Action <CellContent> ReadyForDestroy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void RiseReadyForDestroy(CellContent contentCell)
    {
        ReadyForDestroy?.Invoke(contentCell);
    }

    protected virtual void OnMouseUpAsButton()
    {
        ContentClicked?.Invoke();
    }

    // Update is called once per fram
    void Update()
    {
        
    }

    public void Unsubscribe(BaseCell cell)
    {
        this.ContentClicked -= cell.OnCellClicked;
    }

    public virtual void OnContentClicked(Player player, List <Enemy> openEnemy, BaseCell cellClicked)
    {
    
    }

    public virtual void OnAnyCellClicked(List<Enemy> openEnemy)
    {
        return;
    }
    public virtual void OnContentApplied(Player player, List<Enemy> openEnemy)
    {
        return;
    }
    public virtual void SelfDestroy()
    {

    }

    public virtual void CheckEnemyDeath()
    {

    }

    public void ChangeContentState(bool value)
    {
        gameObject.SetActive(value);
    }

}
 