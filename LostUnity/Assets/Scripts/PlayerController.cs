using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 5f;
    private bool CanTurn = true;
    private PlayerInventory Inventory;

	// Use this for initialization
	void Start ()
    {
        Inventory = GetComponent<PlayerInventory>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		// Moving Forward / Backward
        HandleMovement();

        // Turn
        HandleTurning();

        // Gold Testing
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Inventory.AddGold(10);
        }
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
}
