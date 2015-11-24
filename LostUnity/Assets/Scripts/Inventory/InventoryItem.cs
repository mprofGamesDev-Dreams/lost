using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InventoryItem
{
    public enum INVENTORY_TYPE
    {
        GOLD, TORCH, ARROW
    };

    private INVENTORY_TYPE ItemType;
    private int Count;

    public InventoryItem(INVENTORY_TYPE type)
    {
        ItemType = type;
    }

    public void Add(int Amount)
    {
        Count += Amount;
    }

    public INVENTORY_TYPE GetItemType()
    {
        return ItemType;
    }

    public int GetCount()
    {
        return Count;
    }
}