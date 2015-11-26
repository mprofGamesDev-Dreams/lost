using UnityEngine;
using System.Collections;

public class BlockObject : MonoBehaviour {

	public bool isPath;
	public int xPos;
	public int yPos;
	public int distanceFromCenter;

	private GameObject maze;
	// Use this for initialization
	void Start () {
		bool isPath = false;
		distanceFromCenter = -1;

		maze = gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown()
	{
		Debug.Log ("click");
		if (isPath)
		{
			maze.GetComponent<MazeObject>().RequestPathToExit(gameObject);
		}
	}
}
