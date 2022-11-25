using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellContent : MonoBehaviour
{
    public event Action ContentClicked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void OnMouseUpAsButton()
    {
        ContentClicked?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
 