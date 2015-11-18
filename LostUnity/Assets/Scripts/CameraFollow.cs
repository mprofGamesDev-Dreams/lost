using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Range(2.0f, 100.0f)]
    public float FollowDistance;

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
        // Retrieve the position of the player
        Vector3 trackingPosition = PlayerPrefab.transform.position;

        // Adjust the tracking position
        trackingPosition.y += FollowDistance;

        // Apply the position to the object
        this.gameObject.transform.position = trackingPosition;
	}
}
