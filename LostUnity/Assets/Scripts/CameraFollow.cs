using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    // How far back the camera should be
    [Range(2.0f, 100.0f)]
    public float FollowDistance = 2.0f;

    // How many seconds to turn
    public float RotationSpeed = 1.0f;

    // Accessor to the player
    public GameObject PlayerPrefab;

	void Start ()
    {
        if (!PlayerPrefab)
        {
            Debug.Log("No Player Attached to Camera");
        }
	}
	
	void Update ()
    {
        TrackPosition();
        TrackRotation();
	}

    private void TrackPosition()
    {
        // Retrieve the position of the player
        Vector3 trackingPosition = PlayerPrefab.transform.position;

        // Adjust the tracking position
        trackingPosition.y += FollowDistance;

        // Apply the position to the object
        this.gameObject.transform.position = trackingPosition;
    }

    private void TrackRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y = PlayerPrefab.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
