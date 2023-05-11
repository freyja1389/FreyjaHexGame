using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemSlot : MonoBehaviour
{
    public event Action<Bonus, ItemSlot> UseBonus;
    public Bonus BonusLink;
    // Start is called before the first frame update

    public void OnMouseUpAsButton()
    {
        UseBonus.Invoke(BonusLink, this);
        Destroy(gameObject);
    }
}
//

