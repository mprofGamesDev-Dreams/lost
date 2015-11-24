using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
    [SerializeField] private InventoryItem.INVENTORY_TYPE CollectibleType;
    [SerializeField] private int Value = 1; 
    private PlayerInventory Inventory;

	// Use this for initialization
	void Start ()
    {
        // Find the player
        GameObject Player = GameObject.Find("Player");

        Inventory = Player.GetComponent<PlayerInventory>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Inventory.AddItem(CollectibleType, Value);
            Destroy(this.gameObject);
        }
    }
}
