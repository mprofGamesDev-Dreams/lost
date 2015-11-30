using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/* Class for representing and generating the maze object.
 * Also contains methods for navigatig the maze
 * Please note some generic A* methods are located in the subclass
 * MazeObject.AStar.cs
 * GC
 */

public partial class MazeObject : MonoBehaviour {

	[SerializeField] private GameObject[,] mazeMap;
	[SerializeField] private int mazeWidth = 100;
	[SerializeField] private int mazeHeight = 100;
	[SerializeField] private int tileWidth = 2;
	[SerializeField] private int tileHeight = 2;
	[SerializeField] private GameObject wallPrefab;
	[SerializeField] private GameObject floorPrefab;
	[SerializeField] private GameObject chestPrefab;
	[SerializeField] private int startX = 49;
	[SerializeField] private int startY = 49;
	[SerializeField] private List<GameObject> frontierBlocks;
	[SerializeField] private int itNumber =0;
	[SerializeField] private bool spawnChest = false;
	[SerializeField] private float Speed = 0.5f;
	// Use this for initialization
	void Start () {

		mazeMap = new GameObject[mazeWidth,mazeHeight];
		frontierBlocks = new List<GameObject>();
		for (int cntx = 0; cntx<mazeWidth; cntx++) {

			for(int cnty = 0; cnty<mazeHeight; cnty++){
				mazeMap[cntx,cnty] = (GameObject)Instantiate(wallPrefab,new Vector3(cntx*tileWidth,1.5f,cnty*tileHeight),Quaternion.identity);
				mazeMap[cntx,cnty].transform.parent = this.transform;
				mazeMap[cntx,cnty].GetComponent<BlockObject>().xPos = cntx;
				mazeMap[cntx,cnty].GetComponent<BlockObject>().yPos = cnty;
			}
		}
		Deactivate(mazeMap[startX,startY]);
		frontierBlocks.Add(mazeMap[startX,startY]);
		frontierBlocks.Add(mazeMap[startX-1,startY]);
		frontierBlocks.Add(mazeMap[startX+1,startY]);
		frontierBlocks.Add(mazeMap[startX,startY-1]);
		frontierBlocks.Add(mazeMap[startX,startY+1]);
	}
	
	// Update is called once per frame
	void Update () {
		while (DoNextStep()) {};

		frontierBlocks.Clear ();
		mazeMap [startX, startY].GetComponent<BlockObject> ().distanceFromCenter = 0;
		frontierBlocks.Add (mazeMap [startX, startY]);

		while (AssignDistanceValues()) {};

	}

	bool DoNextStep() // Compleates the next step of the Primm algorithm. Returns false if maze is compleat.
	{
		if (frontierBlocks.Count == 0) {
			return false;
		} else {

			int itterator = UnityEngine.Random.Range(0, frontierBlocks.Count);
			GameObject currentFrontier = frontierBlocks[itterator];
			frontierBlocks.RemoveAt(itterator);

			int numberOfNewPaths = 0;
			if(!currentFrontier.GetComponent<BlockObject>().isPath && IsValidPath(currentFrontier))
			{
				Vector2 frontierPos = new Vector2(currentFrontier.GetComponent<BlockObject>().xPos,currentFrontier.GetComponent<BlockObject>().yPos);
				if ((frontierPos.x -1 > 0.0f) && (frontierPos.x -1 <= (float)mazeWidth) 
				    && !mazeMap[(int)frontierPos.x-1,(int)frontierPos.y].GetComponent<BlockObject>().isPath
				    && IsValidPath(mazeMap[(int)frontierPos.x-1,(int)frontierPos.y]))
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x-1,(int)frontierPos.y]);
					numberOfNewPaths ++;
				}
				if ((frontierPos.x +1 > 0.0f) && (frontierPos.x +1 <= (float)mazeWidth) 
				    && !mazeMap[(int)frontierPos.x+1,(int)frontierPos.y].GetComponent<BlockObject>().isPath
				    && IsValidPath(mazeMap[(int)frontierPos.x+1,(int)frontierPos.y]))
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x+1,(int)frontierPos.y]);
					numberOfNewPaths ++;
				}
				if ((frontierPos.y -1 > 0.0f) && (frontierPos.y -1 <= (float)mazeHeight) 
				    && !mazeMap[(int)frontierPos.x,(int)frontierPos.y-1].GetComponent<BlockObject>().isPath
				    && IsValidPath(mazeMap[(int)frontierPos.x,(int)frontierPos.y-1]))
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x,(int)frontierPos.y-1]);
					numberOfNewPaths ++;
				}
				if ((frontierPos.y +1> 0.0f) && (frontierPos.y +1 <= (float)mazeHeight) 
				    && !mazeMap[(int)frontierPos.x,(int)frontierPos.y+1].GetComponent<BlockObject>().isPath
				    && IsValidPath(mazeMap[(int)frontierPos.x,(int)frontierPos.y+1]))
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x,(int)frontierPos.y+1]);
					numberOfNewPaths ++;
				}

				Deactivate(mazeMap[(int)frontierPos.x,(int)frontierPos.y]);

				if (numberOfNewPaths <= 0 && spawnChest)
				{
					Instantiate(chestPrefab,mazeMap[(int)frontierPos.x,(int)frontierPos.y].transform.position, Quaternion.identity);
				}

			}
			return true;
		}
	}

	bool IsValidPath(GameObject block) //checks to see if a block is a valid path block. This is decided by checking to see if the block has more than one neighbour that is already a path. If it does then the block is ineligable.
	{
		int surroundingPaths = 0;
		Vector2 blockPos = new Vector2(block.GetComponent<BlockObject>().xPos,block.GetComponent<BlockObject>().yPos);
		
		if (blockPos.x - 1 > 0 && blockPos.x - 1 < mazeWidth && blockPos.x + 1 > 0 && blockPos.x + 1 < mazeWidth &&
		    blockPos.y - 1 > 0 && blockPos.y - 1 < mazeHeight && blockPos.y + 1 > 0 && blockPos.y + 1 < mazeHeight) 
		{
			if(mazeMap[(int)blockPos.x -1,(int)blockPos.y].GetComponent<BlockObject>().isPath == true)
			{
				surroundingPaths ++;
			}
			if(mazeMap[(int)blockPos.x +1,(int)blockPos.y].GetComponent<BlockObject>().isPath == true)
			{
				surroundingPaths ++;
			}
			if(mazeMap[(int)blockPos.x,(int)blockPos.y-1].GetComponent<BlockObject>().isPath == true)
			{
				surroundingPaths ++;
			}
			if(mazeMap[(int)blockPos.x,(int)blockPos.y+1].GetComponent<BlockObject>().isPath == true)
			{
				surroundingPaths ++;
			}
			if(surroundingPaths>1 && UnityEngine.Random.Range(0,250) != 0)
			{
				return false;
			}
			
			return true;
		}
		return false;
	}

	bool AssignDistanceValues() //Assigns values to each cell representing how far from the origin of the maze they are.
	{
		
		if (frontierBlocks.Count == 0) {
			return false;
		} else {

			GameObject currentFrontier = frontierBlocks[0];
			int currentDist = currentFrontier.GetComponent<BlockObject>().distanceFromCenter;
			frontierBlocks.RemoveAt(0);
			Vector2 frontierPos = new Vector2(currentFrontier.GetComponent<BlockObject>().xPos,currentFrontier.GetComponent<BlockObject>().yPos);

			if ((frontierPos.x -1 > 0.0f) && (frontierPos.x -1 <= (float)mazeWidth) && mazeMap[(int)frontierPos.x-1,(int)frontierPos.y].GetComponent<BlockObject>().isPath)
			{
				if(mazeMap[(int)frontierPos.x-1,(int)frontierPos.y].GetComponent<BlockObject>().distanceFromCenter == -1 || 
				   mazeMap[(int)frontierPos.x-1,(int)frontierPos.y].GetComponent<BlockObject>().distanceFromCenter > currentDist+1)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x-1,(int)frontierPos.y]);
					mazeMap[(int)frontierPos.x-1,(int)frontierPos.y].GetComponent<BlockObject>().distanceFromCenter = currentDist+1;
				}
			}
			if ((frontierPos.x +1 > 0.0f) && (frontierPos.x +1 <= (float)mazeWidth) && mazeMap[(int)frontierPos.x+1,(int)frontierPos.y].GetComponent<BlockObject>().isPath)
			{
				if(mazeMap[(int)frontierPos.x+1,(int)frontierPos.y].GetComponent<BlockObject>().distanceFromCenter == -1 || 
				   mazeMap[(int)frontierPos.x+1,(int)frontierPos.y].GetComponent<BlockObject>().distanceFromCenter > currentDist+1)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x+1,(int)frontierPos.y]);
					mazeMap[(int)frontierPos.x+1,(int)frontierPos.y].GetComponent<BlockObject>().distanceFromCenter = currentDist+1;
				}
			}
			if ((frontierPos.y -1 > 0.0f) && (frontierPos.y -1 <= (float)mazeHeight) && mazeMap[(int)frontierPos.x,(int)frontierPos.y-1].GetComponent<BlockObject>().isPath)
			{
				if(mazeMap[(int)frontierPos.x,(int)frontierPos.y-1].GetComponent<BlockObject>().distanceFromCenter == -1 || 
				   mazeMap[(int)frontierPos.x,(int)frontierPos.y-1].GetComponent<BlockObject>().distanceFromCenter > currentDist+1)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x,(int)frontierPos.y-1]);
					mazeMap[(int)frontierPos.x,(int)frontierPos.y-1].GetComponent<BlockObject>().distanceFromCenter = currentDist+1;
				}
			}
			if ((frontierPos.y +1> 0.0f) && (frontierPos.y +1 <= (float)mazeHeight) && mazeMap[(int)frontierPos.x,(int)frontierPos.y+1].GetComponent<BlockObject>().isPath)
			{
				if(mazeMap[(int)frontierPos.x,(int)frontierPos.y+1].GetComponent<BlockObject>().distanceFromCenter == -1 || 
				   mazeMap[(int)frontierPos.x,(int)frontierPos.y+1].GetComponent<BlockObject>().distanceFromCenter > currentDist+1)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x,(int)frontierPos.y+1]);
					mazeMap[(int)frontierPos.x,(int)frontierPos.y+1].GetComponent<BlockObject>().distanceFromCenter = currentDist+1;
				};
			}

		}
		return true;
	}



	IEnumerator LightToExit(GameObject obj) // debug code for traceback method
	{
		
		List<GameObject> Path = new List<GameObject> ();
		Vector2 pos = new Vector2 (obj.GetComponent<BlockObject>().xPos, obj.GetComponent<BlockObject>().yPos);
		GameObject currentObj = obj;
		//Vector2 pos
		while (pos.x != startX || pos.y != startY) {
			pos = new Vector2 (currentObj.GetComponent<BlockObject>().xPos, currentObj.GetComponent<BlockObject>().yPos);
			
			if( mazeMap[(int)pos.x-1,(int)pos.y].GetComponent<BlockObject>().distanceFromCenter < currentObj.GetComponent<BlockObject>().distanceFromCenter 
			   && mazeMap[(int)pos.x-1,(int)pos.y].GetComponent<BlockObject>().isPath)
			{
				currentObj = mazeMap[(int)pos.x-1,(int)pos.y];
			}
			
			else if( mazeMap[(int)pos.x+1,(int)pos.y].GetComponent<BlockObject>().distanceFromCenter < currentObj.GetComponent<BlockObject>().distanceFromCenter 
			        && mazeMap[(int)pos.x+1,(int)pos.y].GetComponent<BlockObject>().isPath)
			{
				currentObj = mazeMap[(int)pos.x+1,(int)pos.y];
			}
			
			else if( mazeMap[(int)pos.x,(int)pos.y-1].GetComponent<BlockObject>().distanceFromCenter < currentObj.GetComponent<BlockObject>().distanceFromCenter
			        && mazeMap[(int)pos.x,(int)pos.y-1].GetComponent<BlockObject>().isPath)
			{
				currentObj = mazeMap[(int)pos.x,(int)pos.y-1];
			}
			
			else if( mazeMap[(int)pos.x,(int)pos.y+1].GetComponent<BlockObject>().distanceFromCenter < currentObj.GetComponent<BlockObject>().distanceFromCenter 
			        && mazeMap[(int)pos.x,(int)pos.y+1].GetComponent<BlockObject>().isPath)
			{
				currentObj = mazeMap[(int)pos.x,(int)pos.y+1];
			}
			else
			{
				break;
			}
			//SpawnLight(currentObj.transform.position);
			Path.Add(currentObj);
			//yield return new WaitForSeconds(0.05f);
		}
		GameObject trailer = new GameObject("Trailer");
		TrailRenderer trail = trailer.AddComponent<TrailRenderer> ();
		trailer.transform.position = obj.transform.position + new Vector3(0,1,0);
		trail.startWidth = 1;
		trail.endWidth = 1;
		trail.time = 5;
		
		//Path.Reverse ();
		
		for (int cnt =0; cnt<Path.Count; cnt++) {
			while (Vector3.Distance(trailer.transform.position ,Path[cnt].transform.position + new Vector3(0,1,0)) >0.1)
			{
				trailer.transform.position = Vector3.MoveTowards(trailer.transform.position,Path[cnt].transform.position+ new Vector3(0,1,0),Time.deltaTime*Speed);
				yield return new WaitForEndOfFrame();
			}
		}
	}





	void SpawnLight(Vector3 pos)
	{
		GameObject lightGameObject = new GameObject("The Light");
		Light lightComp = lightGameObject.AddComponent<Light>();
		lightComp.color = Color.blue;
		pos.y ++;
		lightGameObject.transform.position = pos;
		Destroy (lightGameObject, 5.0f);
	}
	
	void Deactivate(GameObject obj)
	{
		obj.GetComponent<BoxCollider> ().isTrigger = true;
		obj.GetComponent<MeshRenderer> ().enabled = false;
		obj.GetComponent<BlockObject> ().isPath = true;
	}



	public int TileWidth {
		get {
			return this.tileWidth;
		}
		set {
			tileWidth = value;
		}
	}

	public int TileHeight {
		get {
			return this.tileHeight;
		}
		set {
			tileHeight = value;
		}
	}

	public int StartX {
		get {
			return this.startX;
		}
		set {
			startX = value;
		}
	}

	public int StartY {
		get {
			return this.startY;
		}
		set {
			startY = value;
		}
	}
		

}
