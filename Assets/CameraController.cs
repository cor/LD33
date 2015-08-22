using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform levelCenter;
	public Transform player;

	public float dampTime = 0.15f;

	public int cameraMovementMultiplier = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		float newX =(levelCenter.position.x * cameraMovementMultiplier) + player.position.x / (cameraMovementMultiplier + 1); 
		float newY =(levelCenter.position.y * cameraMovementMultiplier) + player.position.y / (cameraMovementMultiplier + 1); 
		Vector3 target = new Vector3(newX, newY, transform.position.z);
		transform.position = target;
//		Vector3 velocity = Vector3.zero;

//		Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target);
//		Vector3 delta = target - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
//		Vector3 destination = transform.position + delta;
//		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}


}
