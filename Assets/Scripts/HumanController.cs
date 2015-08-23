using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	public float hitRate = 1f; // Delay between hits, in seconds
	public float hitDamage = 10f;
	public float hitDamageLevelFactor = 2.0f;
	public float hitDamageForLevel;

	public GameObject currentHitTargetCollision;
	public float previousHitTime = 0f;

	public float travelSpeed = 0.5f;
	public float maxSpeed = 1f;

	public HealthManager healthManager;
	
	public GameObject healthBarToClone;
	public GameObject healthBar;
	public UpdateHealthBar updateHealthBar;
	
	public float minimumHitCameraShake = 0.1f;
	public float maximumHitCameraShake = 0.2f;
	
	public float minimumDieCameraShake = 0.25f;
	public float maximumDieCameraShake = 0.5f;

	public ParticleSystem dieParticleSystem;

	private bool died = false;

	void Start() {
		healthManager = GetComponent<HealthManager>();

		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		updateHealthBar = healthBar.GetComponent<UpdateHealthBar> ();
		updateHealthBar.targetToFollow = gameObject;

		hitDamageForLevel = hitDamage * Mathf.Pow (hitDamageLevelFactor, (float)GameLogic.GetInstance ().GetCurrentLevel()); 
	}

	void Update() {


		//Animations
//		if (currentHitTargetCollision != null) {
//
//		} else {
//
//			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
//			Vector2 v = rb.velocity;
//			if (v.magnitude > 0.1f) {
//				transform.rotation = Quaternion.AngleAxis (Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg + 90, Vector3.forward);
//
//			}
//		}


		if (GameLogic.GetInstance ().IsPlaying ()) {

			if (healthBar != null) {
				updateHealthBar.setScale (healthManager.getPercentage ());
			}

			if (healthManager.currentHealth <= 0) {
				Die ();
			}

			if (currentHitTargetCollision != null) {
				if (previousHitTime + hitRate < Time.time) {
					Hit ();
				}
			}
		} else {
			GetComponent<Rigidbody2D> ().Sleep ();
		}
	}


	void FixedUpdate() {
		if (GameLogic.GetInstance ().IsPlaying ()) {

			GameObject closestHouse = FindClosestHouse ();
			if (closestHouse != null) {
				Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

				Vector2 direction = (closestHouse.transform.position - transform.position).normalized;

				rigidbody2D.AddForce (direction * travelSpeed);
				rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed);


				// LOOK AT TARGET
				Vector3 diff = closestHouse.transform.position - transform.position;
				diff.Normalize();
				
				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
			}
		}
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (col.gameObject.tag == "House") {
				currentHitTargetCollision = col.gameObject;
			}
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (col.gameObject == currentHitTargetCollision) {
				currentHitTargetCollision = null;
			}
		}
	}
	
	void Hit() {
		Camera.main.GetComponent<CameraController>().Shake(Random.Range (minimumHitCameraShake, maximumHitCameraShake));

		currentHitTargetCollision.GetComponent<HealthManager> ().currentHealth -= hitDamageForLevel;

		previousHitTime = Time.time;
	}
	
	void Die() {
		if (!died) {
			died = true;

			Camera.main.GetComponent<CameraController> ().Shake (Random.Range (minimumDieCameraShake, maximumDieCameraShake));

			AudioManager.GetInstance ().HumanDies ();

			Instantiate (dieParticleSystem, transform.position, Quaternion.identity);

			Destroy (gameObject, 0.5f);
		}
	}

	GameObject FindClosestHouse() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("House");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
}
