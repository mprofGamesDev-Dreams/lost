using UnityEngine;
using System.Collections;

public class UIFunctions : MonoBehaviour 
{
	public void LoadScene(string name)
	{
		Application.LoadLevel(name);
	}

	public void LoadScene(int index)
	{
		Application.LoadLevel(index);
	}
}
