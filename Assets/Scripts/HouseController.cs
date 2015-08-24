using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour {

	public GameObject healthBarToClone;
	public GameObject healthBar;

	public HealthManager healthManager;
	public UpdateHealthBar updateHealthBar;

	float shakeTil = -1;
	private float shakeAmount = 0.1f;
	public float fadeSpeed = 2; // in seconds
	float amount = 0;
	float seed = 0;

	Vector3 originalPosition;

	void Start () {
		originalPosition = transform.position;

		healthManager = GetComponent<HealthManager>();

		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		updateHealthBar = healthBar.GetComponent<UpdateHealthBar> ();
		updateHealthBar.targetToFollow = gameObject;
		updateHealthBar.healthBarOffset = new Vector3(0, 1f, -0.2f);

		seed = Random.value * 100;
	}
	
	void Update () {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (healthBar != null) {
				updateHealthBar.setScale (healthManager.getPercentage ());
			}
			
			if (healthManager.currentHealth <= 0) {
				Destroy (gameObject);
			}
			
			float t = Time.time * 10 + seed;
			Vector3 offset =new Vector3(
				Mathf.PerlinNoise(t, t),
				Mathf.PerlinNoise(-t, t),
				originalPosition.z);
			float dir = Time.time < shakeTil ? 1.0f : -1.0f;
			amount = Mathf.Clamp01(amount + dir * fadeSpeed * Time.deltaTime);
			
			transform.position = originalPosition + offset * amount * shakeAmount;
		}
	}

	public void Shake(float duration){
		shakeTil = Time.time + duration;
	}
}
