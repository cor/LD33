using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	float shakeTil = -1;
	public float shakeAmount = 0.25f;
	public float fadeSpeed = 2; // in seconds
	float amount = 0;
	float seed = 0;
	
	public Transform levelCenter;
	public Transform player;

	public float dampTime = 0.15f;

	public int cameraMovementMultiplier = 5;

	private float camaraZ;

	// Use this for initialization
	void Start () {
		seed = Random.value * 100;

		camaraZ = transform.position.z;
	}

	void Update () {
		float newX =(levelCenter.position.x * cameraMovementMultiplier) + player.position.x / (cameraMovementMultiplier + 1); 
		float newY =(levelCenter.position.y * cameraMovementMultiplier) + player.position.y / (cameraMovementMultiplier + 1); 

		float t = Time.time * 10 + seed;
		Vector3 offset =new Vector3(
			Mathf.PerlinNoise(t, t),
			Mathf.PerlinNoise(-t, t),
			Mathf.PerlinNoise(t, -t));
		float dir = Time.time < shakeTil ? 1.0f : -1.0f;
		amount = Mathf.Clamp01(amount + dir * fadeSpeed * Time.deltaTime);

		Vector3 target = new Vector3(newX, newY, camaraZ);

		transform.position = target + offset * amount * shakeAmount;

//		if(Input.GetMouseButtonDown(0)) Shake (Random.Range (0.25f, 0.5f));
	}

	public void Shake(float duration){
		shakeTil = Time.time + duration;
	}

}
