using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    }

    // Update is called once per frame
    void Update()
    {

    }
   // public  void OnMouseUpAsButton()
    //{
    //    CellClicked?.Invoke(this);
   // }
}
