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

	void Start() {
		healthManager = GetComponent<HealthManager>();

		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		updateHealthBar = healthBar.GetComponent<UpdateHealthBar> ();
		updateHealthBar.targetToFollow = gameObject;

		hitDamageForLevel = hitDamage * Mathf.Pow (hitDamageLevelFactor, (float)GameLogic.Get ().GetCurrentLevel()); 
	}

	void Update() {
		if (healthBar != null) {
			updateHealthBar.setScale(healthManager.getPercentage());
		}

		if (healthManager.currentHealth <= 0) {
			Die ();
		}

		if (currentHitTargetCollision != null) {
			if (previousHitTime + hitRate < Time.time) {
				currentHitTargetCollision.GetComponent<HealthManager> ().currentHealth -= hitDamageForLevel;
				previousHitTime = Time.time;
			}
		}
	}


	void FixedUpdate() {
		GameObject closestHouse = FindClosestHouse ();
		if (closestHouse != null) {
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

			Vector2 direction = (closestHouse.transform.position - transform.position).normalized;

			rigidbody2D.AddForce (direction * travelSpeed);
			rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed);
		}
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "House") {
			currentHitTargetCollision = col.gameObject;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject == currentHitTargetCollision) {
			currentHitTargetCollision = null;
		}
	}


	void Die() {
		Destroy(gameObject);
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
