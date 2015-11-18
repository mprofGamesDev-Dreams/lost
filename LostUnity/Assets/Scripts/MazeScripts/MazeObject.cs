using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeObject : MonoBehaviour {

	[SerializeField] private GameObject[,] mazeMap;
	[SerializeField] private int mazeWidth = 100;
	[SerializeField] private int mazeHeight = 100;
	[SerializeField] private int tileWidth = 2;
	[SerializeField] private int tileHeight = 2;
	[SerializeField] private GameObject wallPrefab;
	[SerializeField] private GameObject floorPrefab;

	[SerializeField] private int startX = 49;
	[SerializeField] private int startY = 49;
	private List<GameObject> frontierBlocks;

	// Use this for initialization
	void Start () {

		mazeMap = new GameObject[mazeWidth,mazeHeight];
		frontierBlocks = new List<GameObject>();
		for (int cntx = 0; cntx<mazeWidth; cntx++) {

			for(int cnty = 0; cnty<mazeHeight; cnty++){
				mazeMap[cntx,cnty] = (GameObject)Instantiate(wallPrefab,new Vector3(cntx*tileWidth,0,cnty*tileHeight),Quaternion.identity);
				mazeMap[cntx,cnty].transform.parent = this.transform;
				mazeMap[cntx,cnty].GetComponent<BlockObject>().xPos = cntx;
				mazeMap[cntx,cnty].GetComponent<BlockObject>().yPos = cnty;
			}
		}
		mazeMap[startX,startY].SetActive(false);
		frontierBlocks.Add(mazeMap[startX,startY]);
		frontierBlocks.Add(mazeMap[startX-1,startY]);
		frontierBlocks.Add(mazeMap[startX+1,startY]);
		frontierBlocks.Add(mazeMap[startX,startY-1]);
		frontierBlocks.Add(mazeMap[startX,startY+1]);
	}
	
	// Update is called once per frame
	void Update () {
		DoNextStep ();
		DoNextStep ();
		DoNextStep ();
		DoNextStep ();
		DoNextStep ();
	}

	bool DoNextStep()
	{
		if (frontierBlocks.Count == 0) {
			return false;
		} else {

			int itterator = Random.Range(0, frontierBlocks.Count);
			GameObject currentFrontier = frontierBlocks[itterator];
			frontierBlocks.RemoveAt(itterator);

			if(currentFrontier.activeSelf && IsValidPath(currentFrontier))
			{
				Vector2 frontierPos = new Vector2(currentFrontier.GetComponent<BlockObject>().xPos,currentFrontier.GetComponent<BlockObject>().yPos);
				if ((frontierPos.x -1 > 0.0f) && (frontierPos.x -1 <= (float)mazeWidth) && mazeMap[(int)frontierPos.x-1,(int)frontierPos.y].activeSelf)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x-1,(int)frontierPos.y]);
				}
				if ((frontierPos.x +1 > 0.0f) && (frontierPos.x +1 <= (float)mazeWidth) && mazeMap[(int)frontierPos.x+1,(int)frontierPos.y].activeSelf)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x+1,(int)frontierPos.y]);
				}
				if ((frontierPos.y -1 > 0.0f) && (frontierPos.y -1 <= (float)mazeHeight) && mazeMap[(int)frontierPos.x,(int)frontierPos.y-1].activeSelf)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x,(int)frontierPos.y-1]);
				}
				if ((frontierPos.y +1> 0.0f) && (frontierPos.y +1 <= (float)mazeHeight) && mazeMap[(int)frontierPos.x,(int)frontierPos.y+1].activeSelf)
				{
					frontierBlocks.Add(mazeMap[(int)frontierPos.x,(int)frontierPos.y+1]);
				}

				mazeMap[(int)frontierPos.x,(int)frontierPos.y].SetActive(false);

			}
			return true;
		}
	}

	bool IsValidPath(GameObject block)
	{
		int surroundingPaths = 0;
		Vector2 blockPos = new Vector2(block.GetComponent<BlockObject>().xPos,block.GetComponent<BlockObject>().yPos);

		if (blockPos.x - 1 > 0 && blockPos.x - 1 < mazeWidth && blockPos.x + 1 > 0 && blockPos.x + 1 < mazeWidth &&
		    blockPos.y - 1 > 0 && blockPos.y - 1 < mazeHeight && blockPos.y + 1 > 0 && blockPos.y + 1 < mazeHeight) 
		{
			if(mazeMap[(int)blockPos.x -1,(int)blockPos.y].activeSelf == false)
			{
				surroundingPaths ++;
			}
			if(mazeMap[(int)blockPos.x +1,(int)blockPos.y].activeSelf == false)
			{
				surroundingPaths ++;
			}
			if(mazeMap[(int)blockPos.x,(int)blockPos.y-1].activeSelf == false)
			{
				surroundingPaths ++;
			}
			if(mazeMap[(int)blockPos.x,(int)blockPos.y+1].activeSelf == false)
			{
				surroundingPaths ++;
			}
			if(surroundingPaths>1 && Random.Range(0,250) != 0)
			{
				return false;
			}

			return true;
		}
		return false;
	}
}
