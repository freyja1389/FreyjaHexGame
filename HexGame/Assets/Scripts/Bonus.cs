using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bonus : CellContent
{
    //[SerializeField]
    //private HexCell Type;
    public int HealPoints;
   // public event Action<Bonus> CellClicked;
    public Vector2Int CellIndex;
    public bool Open;
    // Start is called before the first frame update
    void Start()
    {
        HealPoints = SetHealPoints();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private int SetHealPoints()
    {
        return Random.Range(5, 15);
    }
    // public  void OnMouseUpAsButton()
    //{
    //    CellClicked?.Invoke(this);
    // }
}
