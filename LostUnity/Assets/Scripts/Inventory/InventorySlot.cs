using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuration")]
    public Text ItemText;
    public Sprite EmptySlot;
    public Sprite EmptyHighlightedSlot;
    public float TextScaler = 0.5f;

    private Stack<InventoryItem> Items;

	// Use this for initialization
	void Start ()
    {
        // Create the stack
        Items = new Stack<InventoryItem>();

        // Configure the text
        RectTransform SlotRect = GetComponent<RectTransform>();
        RectTransform TextRect = ItemText.GetComponent<RectTransform>();

        // Configure text size
        int TextScale = (int)(SlotRect.sizeDelta.x * TextScaler);
        ItemText.resizeTextMinSize = TextScale;
        ItemText.resizeTextMaxSize = TextScale;

        // Set the size of the text element
        float width = SlotRect.sizeDelta.x * 0.9f;
        float height = SlotRect.sizeDelta.y * 0.9f;
        TextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        TextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        // Set default GUI
        UpdateGUI();
	}

    // Add an item to the slot
    public void AddItem(InventoryItem item)
    {
        // Add the item to the slot
        Items.Push(item);

        // Update GUI
        UpdateGUI();

        // Update the sprite
        ChangeSprite(item.NormalSprite, item.HighlightedSprite);
    }

    private void ChangeSprite(Sprite normal, Sprite highlighted)
    {
        // Set the normal sprite
        GetComponent<Image>().sprite = normal;

        // Create a state for switching between normal and highlighted
        SpriteState state = new SpriteState();
        state.highlightedSprite = highlighted;
        state.pressedSprite = normal;

        // Apply to the state
        GetComponent<Button>().spriteState = state;
    }

    private void UseItem()
    {
        if (!IsEmpty())
        {
            // Use the item and remove from the stack
            Items.Pop().Use();

            // Update GUI
            UpdateGUI();
        }
    }

    private void UpdateGUI()
    {
        // Update the text if we have more than one
        if (Items.Count >= 1)
        {
            ItemText.text = Items.Count.ToString();
        }
        else
        {
            ItemText.text = "";
        }

        if (IsEmpty())
        {
            ChangeSprite(EmptySlot, EmptyHighlightedSlot);
            PlayerInventory.FreeSlots++;
        }
    }

    //================================================
    // Getters
    //================================================

    public bool IsEmpty()
    {
        // Check if the slot is empty
        return Items.Count == 0;
    }

    public bool IsAvailable()
    {
        // Check if we have filled the slot
        return StackItem().StackSize > Items.Count;
    }

    public InventoryItem StackItem()
    {
        // Return the type of item we are storing in this slot
        return Items.Peek();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }
}
