using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Player Attributes")]
    public float MoveSpeed = 5.0f;
    public bool CanTurn = true;

    [Header("Inventory")]
    public GameObject InventoryUI;
    public PlayerInventory Inventory;
    public bool ShowInventory = false;

	public GameObject torchPrefab;

	void Start ()
    {
		//InventoryItem item = new InventoryItem(InventoryItem.INVENTORY_TYPE.TORCH);

		StartCoroutine(AddItems());
	}

	private IEnumerator AddItems()
	{
		yield return new WaitForEndOfFrame();
	
		InventoryItem item1 = ((GameObject)Instantiate(torchPrefab, Vector3.zero, Quaternion.identity)).GetComponent<InventoryItem>();
		InventoryItem item2 = ((GameObject)Instantiate(torchPrefab, Vector3.zero, Quaternion.identity)).GetComponent<InventoryItem>();
		InventoryItem item3 = ((GameObject)Instantiate(torchPrefab, Vector3.zero, Quaternion.identity)).GetComponent<InventoryItem>();
		
		Inventory.AddItem(item1);
		Inventory.AddItem(item2);
		Inventory.AddItem(item3);
	}

    void Update()
    {
		// Moving Forward / Backward
        HandleMovement();

        // Turn
        HandleTurning();

        // Input
        HandleInput();
	}

    private void HandleMovement()
    {
        float v = Input.GetAxis("Vertical");
        Vector3 vVelocity = transform.forward * MoveSpeed * Time.deltaTime;

        // Move Forward / Back
        if (v > 0.8f)
        {
            transform.position += vVelocity;
        }
        else if (v < -0.8f)
        {
            transform.position -= vVelocity;
        }
    }

    private void HandleTurning()
    {
        float h = Input.GetAxis("Horizontal");

        // Move Left / Right
        if (h > 0.8f & CanTurn)
        {
            Vector3 euler = transform.rotation.eulerAngles + new Vector3(0, 90, 0);
            transform.rotation = Quaternion.Euler(euler);
            CanTurn = false;
        }
        else if (h < -0.8f & CanTurn)
        {
            Vector3 euler = transform.rotation.eulerAngles - new Vector3(0, 90, 0);
            transform.rotation = Quaternion.Euler(euler);
            CanTurn = false;
        }
        else if (h == 0.0f)
        {
            CanTurn = true;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory = !ShowInventory;

            InventoryUI.SetActive(ShowInventory);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            // Add the item to the inventory
            Inventory.AddItem(other.GetComponent<InventoryItem>());

            // Delete the collectable
            Destroy(other.gameObject);
        }
//		else if(other.tag == "Torch")
//		{
//			Inventory.AddItem(other.GetComponent<InventoryItem>());
//
//			other.transform.position -= new Vector3( 0, 3, 0 )
//		}
    }
}


