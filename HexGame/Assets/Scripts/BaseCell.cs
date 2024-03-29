using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCell : MonoBehaviour
{
    public event Action<BaseCell> CellClicked;
    public Vector2Int CellIndex;

    public void Activate()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }

    protected virtual void OnMouseUpAsButton()
    {             
        CellClicked?.Invoke(this);
    }
}
