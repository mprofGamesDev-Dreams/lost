using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float speed = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		if (h > 0.8f) 
			transform.position += Vector3.right * speed * Time.deltaTime;
		else if(h < -0.8f)
			transform.position += Vector3.left * speed * Time.deltaTime;
			
		if (v > 0.8f) 
			transform.position += Vector3.forward * speed * Time.deltaTime;
		else if(v < -0.8f)
			transform.position += Vector3.back * speed * Time.deltaTime;
	}
}
