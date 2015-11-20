using UnityEngine;
using System.Collections;

public class PlayerStart : MonoBehaviour {

	private GameObject mazeGameObject;

	// Use this for initialization
	void Start () {
		mazeGameObject = GameObject.Find ("Maze");
		MazeObject mazeComponent = mazeGameObject.GetComponent<MazeObject> ();

		//Setup player position
		gameObject.transform.position = new Vector3
			(mazeGameObject.transform.position.x + (mazeComponent.TileWidth * mazeComponent.StartX),
			mazeGameObject.transform.position.y + 1.0f,
			mazeGameObject.transform.position.z + (mazeComponent.TileHeight * mazeComponent.StartY));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
