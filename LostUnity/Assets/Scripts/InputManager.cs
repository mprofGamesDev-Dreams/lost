using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public GameObject InventoryGUI;

    [Header("Flags")]
    private bool ShowInventory = false;

	
	void Start ()
    {
	
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory = !ShowInventory;
            InventoryGUI.SetActive(ShowInventory);
        }
	}
}
