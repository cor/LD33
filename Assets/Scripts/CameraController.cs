using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform levelCenter;
	public Transform player;

	public float dampTime = 0.15f;

	public int cameraMovementMultiplier = 5;

	void Update () {

		float newX =(levelCenter.position.x * cameraMovementMultiplier) + player.position.x / (cameraMovementMultiplier + 1); 
		float newY =(levelCenter.position.y * cameraMovementMultiplier) + player.position.y / (cameraMovementMultiplier + 1); 
		Vector3 target = new Vector3(newX, newY, transform.position.z);
		transform.position = target;
	}
}
