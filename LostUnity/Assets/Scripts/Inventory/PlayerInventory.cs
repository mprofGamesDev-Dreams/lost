using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [Header("GUI Configuration")]
    public int NoOfColumns = 9;
    public int NoOfRows = 3;
    public float SlotPadding = 3;
    public float SlotSize = 25;
    public GameObject BaseSlot;
    private List<GameObject> InventorySlots;
    private RectTransform InventoryBG;
    private float InventoryWidth;
    private float InventoryHeight;

    [Header("Inventory")]
    private static int EmptySlots;
    public static int FreeSlots
    {
        get { return EmptySlots; }
        set { EmptySlots = value; }
    }

	// Use this for initialization
	void Start ()
    {
        CreateInterface();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void CreateInterface()
    {
        // Calculate the height of the background
        InventoryWidth = NoOfColumns * (SlotSize + SlotPadding) + SlotPadding;
        InventoryHeight = NoOfRows * (SlotSize + SlotPadding) + SlotPadding;

        // Create a list to store the inventory
        InventorySlots = new List<GameObject>();

        // Set up the inventory background
        InventoryBG = GetComponent<RectTransform>();
        InventoryBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, InventoryWidth);
        InventoryBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, InventoryHeight);

        int SlotID = 1;

        for (int y = 0; y < NoOfRows; y++)
        {
            for (int x = 0; x < NoOfColumns; x++)
            {
                // Create a new slot
                GameObject InvSlot = (GameObject)Instantiate(BaseSlot);
                RectTransform InvTransform = InvSlot.GetComponent<RectTransform>();

                // Calculate slot properties
                float xPos = (InventoryBG.localPosition.x) + SlotPadding * (x + 1) + (SlotSize * x);
                float yPos = (InventoryBG.localPosition.y) - SlotPadding * (y + 1) - (SlotSize * y);

                // Set slot properties
                InvSlot.name = "Slot " + SlotID.ToString();
                InvSlot.transform.parent = this.transform.parent;

                // Set slot rect propertie
                InvTransform.localPosition = new Vector3(xPos, yPos, 0);
                InvTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SlotSize);
                InvTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SlotSize);

                // Add to our array
                InventorySlots.Add(InvSlot);

                // Move to next slot
                SlotID++;
            }
        }

        // Store how many slots we start with
        EmptySlots = NoOfColumns * NoOfRows;
    }

    public bool AddItem(InventoryItem item)
    {
        // If theres only one of the item
        if (item.StackSize == 1)
        {
            // Add to the invetory
            PlaceEmpty(item);
            return true;
        }
        else
        {
            // Search the inventory for the item and add to the current stack
            foreach (GameObject obj in InventorySlots)
            {
                InventorySlot slot = obj.GetComponent<InventorySlot>();

                if (!slot.IsEmpty() && slot.IsAvailable())
                {
                    if (slot.StackItem().ItemType == item.ItemType)
                    {
                        slot.AddItem(item);
                        EmptySlots--;
                        return true;
                    }
                }
            }

            // Not possible to stack the item so add to the inventory
            if (EmptySlots > 0)
            {
                PlaceEmpty(item);
                return true;
            }
        }

        return false;
    }

    private bool PlaceEmpty(InventoryItem item)
    {
        if (EmptySlots == 0)
            return false;

        foreach (GameObject obj in InventorySlots)
        {
            InventorySlot slot = obj.GetComponent<InventorySlot>();

            if (slot.IsEmpty())
            {
                slot.AddItem(item);
                EmptySlots--;
                return true;
            }
        }

        return false;
    }
}
