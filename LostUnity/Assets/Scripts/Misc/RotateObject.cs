using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    public Vector3 RotationSpeed = new Vector3(0,0,0);
	
	// Update is called once per frame
	void Update ()
    {
        // Get the current rotation
        Vector3 CurrentRotation = transform.rotation.eulerAngles;

        // Apply the rotation velocity
        CurrentRotation += (RotationSpeed * Time.deltaTime);

        // Set back to the object
        transform.rotation = Quaternion.Euler(CurrentRotation);
	}
}
