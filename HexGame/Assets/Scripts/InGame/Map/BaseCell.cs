using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCell : MonoBehaviour
{
    public event Action<BaseCell> CellClicked;
    public event Action<BaseCell> ShownContent;

    public bool Opened;
    public bool Open;

    public Vector2Int CellIndex;
    public CellType ContentType;

    public CellContent ContentLink;

   // private CellContent contentPrefab;

    public void Activate()
    {
       // GetComponent<Rigidbody>().useGravity = true;
    }

    protected virtual void OnMouseUpAsButton()
    {
        RiseCellClicked();
    }

    private void OnDestroy()
    {
        CellClicked = null; 
    }

    protected void  RiseCellClicked()
    {
        CellClicked?.Invoke(this);
    }

    public void SetMaterial(Material material)
    {
        GetComponentInChildren<MeshRenderer>().material = material;
    }

   //public void SetContentPrefab(CellContent prefab)
   // {
     //   contentPrefab = prefab;
   // }

    public void SetContentLink(CellContent contentLink)
    {
        ContentLink = contentLink;
    }

    public void OnCellClicked()
    {
       RiseCellClicked();
    }

    internal void ShowContent()
    {
        if (ContentLink is null) return;
        Opened = true;
        ContentLink.ChangeContentState(true); ; // инкапсулировать ++
        ShownContent?.Invoke(this);
    }

    public void InstantiateContentPrefab(CellContent prefab)
    {
        var content = Instantiate(prefab, transform.position, transform.rotation, transform);
        content.ChangeContentState(false);                                                                                                               // TO DO incapsulate
        SetContentLink(content);                                                                                                                        // TO DO celltype is needed???if not - delete, incapsulate this function into basecell (parameter - prefab, celltype??? if needed - rename to content type) 
        content.ContentClicked += OnCellClicked;

    }
}
