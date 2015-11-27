using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public enum INVENTORY_TYPE
    {
        GOLD, TORCH
    };

    [Header("Sprites")]
    public Sprite NormalSprite;
    public Sprite HighlightedSprite;

    [Header("Item Information")]
    public INVENTORY_TYPE ItemType;
    public int Count;
    public int StackSize = 20;

	#region Constructors
	public InventoryItem(){}

	public InventoryItem(INVENTORY_TYPE type)
	{
		ItemType = type;
		Count++;
		StackSize = 1;
	}
	#endregion

    public void Use()
    {
        switch (ItemType)
        {
            case INVENTORY_TYPE.GOLD:
                // No use for gold
                Debug.Log("I threw gold in the air like I just don't care");
                break;

            case INVENTORY_TYPE.TORCH:
                // Create torch at the position
                Debug.Log("Come on baby, light my fire");

				Transform obj = GameObject.Find("Player").transform;
				this.gameObject.transform.position = obj.position;
				this.GetComponent<Light>().enabled = true;

                break;
        }
    }
}