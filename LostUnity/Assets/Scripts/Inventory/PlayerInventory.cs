using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    [Header("GUI Accessors")]
    public Text InventoryGUI;

    [Header("Inventory")]
    [SerializeField]private IList<InventoryItem> Inventory;

    void Start()
    {
        // Calculate how many types of inventory items we have
        int NoOfInventoryTypes = System.Enum.GetNames(typeof(InventoryItem.INVENTORY_TYPE)).Length;

        // Create a list of the inventory
        Inventory = new List<InventoryItem>();
        for (int i = 0; i < NoOfInventoryTypes; i++)
        {
            Inventory.Add(new InventoryItem((InventoryItem.INVENTORY_TYPE)i));
        }

        // Set the initial gui text
        RefreshGUI();
    }

    // Testing Purposes only - this can be remove
    public void AddGold(int Amount)
    {
        int ID = (int)InventoryItem.INVENTORY_TYPE.GOLD;

        // Increase gold
        Inventory[ID].Add(Amount);

        RefreshGUI();
    }

    public void AddItem(InventoryItem.INVENTORY_TYPE type, int Amount)
    {
        int ID = (int)type;

        // Increase counter of that item
        Inventory[ID].Add(Amount);

        RefreshGUI();
    }

    public void RefreshGUI()
    {
        string text;
        int count;

        // Reset the text
        InventoryGUI.text = "";

        // Compile the list
        for (int i = 0; i < Inventory.Count(); i++)
        {
            text = Inventory[i].GetItemType().ToString();
            count = Inventory[i].GetCount();

            // Refresh Gold Counter
            InventoryGUI.text += Format(text) + " : " + count + "\n";
        }
    }

    private string Format(string s)
    {
        // Check for empty string.
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        // Force to lower case
        s = s.ToLower();

        // Return char and concat substring.
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}
