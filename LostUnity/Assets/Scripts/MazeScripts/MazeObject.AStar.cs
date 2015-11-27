using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class MazeObject : MonoBehaviour {

	public List<Vector2> AStarPF(Vector2 startPos, Vector2 endPos) //A* method
	{
		List<Vector2> outputPath = new List<Vector2>();
		
		List<AStarStruct> openSet = new List<AStarStruct>();
		List<AStarStruct> closedSet = new List<AStarStruct>();
		
		AStarStruct start = new AStarStruct (0, startPos, mazeMap[(int)startPos.x,(int)startPos.y]);
		start._fvalue = start._gvalue + HValue (start._mazeCoords, endPos);
		
		openSet.Add (start);
		
		while (openSet.Count >0) {
			openSet.Sort();
			AStarStruct current = openSet[0];
			
			if (current._mazeCoords.x == endPos.x && current._mazeCoords.y == endPos.y)
			{
				Retrace(current, start,outputPath);
				outputPath.Reverse();
				return outputPath;
			}
			
			openSet.Remove(current);
			closedSet.Add(current);
			
			if ((current._mazeCoords.x -1 > 0.0f) && (current._mazeCoords.x -1 <= (float)mazeWidth) 
			    && mazeMap[(int)current._mazeCoords.x-1,(int)current._mazeCoords.y].GetComponent<BlockObject>().isPath
			    && !IsInClosedSet(new Vector2(current._mazeCoords.x -1, current._mazeCoords.y), closedSet))
			{
				AStarStruct neighbor = new AStarStruct( current._gvalue + 1,
				                                       new Vector2(current._mazeCoords.x -1,current._mazeCoords.y),
				                                       current,
				                                       mazeMap[(int)current._mazeCoords.x-1,(int)current._mazeCoords.y]);
				
				neighbor._fvalue = neighbor._gvalue + HValue(new Vector2(current._mazeCoords.x -1,current._mazeCoords.y),
				                                             endPos);
				
				AddToOpenSet(neighbor,openSet);
				
			}
			if ((current._mazeCoords.x +1 > 0.0f) && (current._mazeCoords.x +1 <= (float)mazeWidth) 
			    && mazeMap[(int)current._mazeCoords.x+1,(int)current._mazeCoords.y].GetComponent<BlockObject>().isPath
			    && !IsInClosedSet(new Vector2(current._mazeCoords.x +1, current._mazeCoords.y), closedSet))
			{
				AStarStruct neighbor = new AStarStruct( current._gvalue + 1,
				                                       new Vector2(current._mazeCoords.x +1,current._mazeCoords.y),
				                                       current,
				                                       mazeMap[(int)current._mazeCoords.x+1,(int)current._mazeCoords.y]);
				
				neighbor._fvalue = neighbor._gvalue + HValue(new Vector2(current._mazeCoords.x +1,current._mazeCoords.y),
				                                             endPos);
				
				AddToOpenSet(neighbor,openSet);
				
			}
			if ((current._mazeCoords.y -1 > 0.0f) && (current._mazeCoords.y -1 <= (float)mazeHeight) 
			    && mazeMap[(int)current._mazeCoords.x,(int)current._mazeCoords.y-1].GetComponent<BlockObject>().isPath
			    && !IsInClosedSet(new Vector2(current._mazeCoords.x , current._mazeCoords.y-1), closedSet))
			{
				AStarStruct neighbor = new AStarStruct( current._gvalue + 1,
				                                       new Vector2(current._mazeCoords.x,current._mazeCoords.y-1),
				                                       current,
				                                       mazeMap[(int)current._mazeCoords.x,(int)current._mazeCoords.y-1]);
				
				neighbor._fvalue = neighbor._gvalue + HValue(new Vector2(current._mazeCoords.x,current._mazeCoords.y-1),
				                                             endPos);
				
				AddToOpenSet(neighbor,openSet);
				
			}
			if ((current._mazeCoords.y +1> 0.0f) && (current._mazeCoords.y +1 <= (float)mazeHeight) 
			    && mazeMap[(int)current._mazeCoords.x,(int)current._mazeCoords.y+1].GetComponent<BlockObject>().isPath
			    && !IsInClosedSet(new Vector2(current._mazeCoords.x , current._mazeCoords.y+1), closedSet))
			{
				AStarStruct neighbor = new AStarStruct( current._gvalue + 1,
				                                       new Vector2(current._mazeCoords.x,current._mazeCoords.y+1),
				                                       current,
				                                       mazeMap[(int)current._mazeCoords.x,(int)current._mazeCoords.y+1]);
				
				neighbor._fvalue = neighbor._gvalue + HValue(new Vector2(current._mazeCoords.x ,current._mazeCoords.y+1),
				                                             endPos);
				
				AddToOpenSet(neighbor,openSet);
			}
			
		}
		return null;
	}
	
	IEnumerator PathToExit(GameObject obj)  // Debug code for A* method
	{
		List<GameObject> Path = new List<GameObject> ();
		
		List <Vector2> V2Path = AStarPF (new Vector2 (obj.GetComponent<BlockObject> ().xPos, obj.GetComponent<BlockObject> ().yPos),
		                                 new Vector2 (StartX, StartY));
		
		foreach(Vector2 vect in V2Path)
		{
			Path.Add (mazeMap[(int)vect.x,(int)vect.y]);
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

	private bool IsInClosedSet(Vector2 position, List<AStarStruct> set)
	{
		foreach( AStarStruct obj in set)
		{
			if(position.x == obj._mazeCoords.x && position.y == obj._mazeCoords.y)
			{
				return true;
			}
		}
		return false;
	}
	
	private void Retrace(AStarStruct endPos, AStarStruct startPos, List<Vector2> outputPath)
	{
		outputPath.Add (endPos._mazeCoords);
		AStarStruct current = endPos;
		while (current._mazeObjectRef != startPos._mazeObjectRef) {
			current = current._parentRef;
			outputPath.Add (current._mazeCoords);
		}
	}
	
	
	private void AddToOpenSet(AStarStruct newStruct, List<AStarStruct> set)
	{
		foreach( AStarStruct obj in set)
		{
			if(newStruct._mazeCoords.x == obj._mazeCoords.x && newStruct._mazeCoords.y == obj._mazeCoords.y)
			{
				if(newStruct._gvalue<obj._gvalue)
				{
					set.Remove(obj);
					set.Add(newStruct);
					break;
				}
			}
		}
		set.Add(newStruct);
	}
	
	private float HValue(Vector2 currentPos, Vector2 endPos)
	{
		return Mathf.Abs (endPos.x - currentPos.x) + Mathf.Abs (endPos.y - currentPos.y);
	}
	
	public void RequestPathToExit(GameObject obj)
	{
		//StartCoroutine (LightToExit (obj));
		StartCoroutine (PathToExit (obj));
	}

}

public class AStarStruct : IComparable<AStarStruct>
{
	public float _hvalue;
	public float _gvalue;
	public float _fvalue;
	
	public Vector2 _mazeCoords;
	public AStarStruct _parentRef;
	public GameObject _mazeObjectRef;
	
	public AStarStruct(float g, Vector2 co,AStarStruct parent, GameObject mref)
	{
		_hvalue = 0;
		_fvalue = 0;
		_gvalue = g;
		_parentRef = parent;
		_mazeCoords = co;
		_mazeObjectRef = mref;
	}
	
	public AStarStruct(float g, Vector2 co, GameObject mref)
	{
		_hvalue = 0;
		_fvalue = 0;
		_gvalue = g;
		_parentRef = null;
		_mazeCoords = co;
		_mazeObjectRef = mref;
	}
	
	
	public int CompareTo(AStarStruct other)
	{
		
		return (int)_fvalue - (int)other._fvalue;
		
	}
}