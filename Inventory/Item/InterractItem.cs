using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory/Item/CreateInterractItem")]
public class InterractItem : Item
{
    public InterractType interractType;
    public bool needKey;
    public bool isLocked;
    public bool isAnimated = false;
}

public enum InterractType
{
    Object,
    Door,
    Drawer
}
