using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {


	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public float height = -20f;
	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (target != null)
		{
			Vector3 delta = target.position - transform.position; //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			Vector3 newPos = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			transform.position = new Vector3(newPos.x, newPos.y, height); 
		}

	
	}
}
