using UnityEngine;
using System.Collections;

public class DropLightSource : MonoBehaviour {

	public GameObject light;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
			Instantiate (light, transform.position, Quaternion.identity);
	}
}
