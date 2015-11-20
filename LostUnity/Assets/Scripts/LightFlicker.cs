using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	[SerializeField] private Light torchlight;
	[SerializeField] private float intensityMax = 3.0f;
	[SerializeField] private float intensityMin = 1.0f;

	private float frequency = 1.0f;
	private float magnitude = 0.5f;
	private float time;

	// Use this for initialization
	void Start () {
		torchlight = this.GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
