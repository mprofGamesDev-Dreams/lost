using UnityEngine;
using System.Collections;

public class AddItemsToInventoryOnStart : MonoBehaviour 
{
	private	void Start () 
	{
		PlayerInventory playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();

		playerInventory.AddItem(new InventoryItem(InventoryItem.INVENTORY_TYPE.TORCH));
		playerInventory.AddItem(new InventoryItem(InventoryItem.INVENTORY_TYPE.TORCH));
		playerInventory.AddItem(new InventoryItem(InventoryItem.INVENTORY_TYPE.TORCH));

	
	}
}
