using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	public float travelSpeed = 0.5f;
	public float maxSpeed = 1f; //Replace with your max speed

	public float hitRate = 1f; // Delay between hits, in seconds
	public int hitDamage = 10;

	HealthManager healthManager;

	float previousHitTime = 0f;

	Collision2D currentHitTargetCollision;

	public GameObject healthBarToClone;
	GameObject healthBar;

	void Start() {
		healthManager = GetComponent<HealthManager>();

		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		healthBar.GetComponent<UpdateHealthBar>().targetToFollow = gameObject;
	}


	void Update() {
		if (healthBar != null) {
			healthBar.GetComponent<UpdateHealthBar>().setScale(healthManager.getPercentage());
		}

		if (healthManager.currentHealth == 0) {
			Destroy(gameObject);
		}

		if (currentHitTargetCollision != null) {
			if (previousHitTime + hitRate < Time.time) {
				Debug.Log("Going to hit");
				currentHitTargetCollision.gameObject.GetComponent<HealthManager>().currentHealth -= hitDamage;
				previousHitTime = Time.time;
			}
		}


	}


	void FixedUpdate() {
		GameObject closestHouse = FindClosestHouse ();

		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

		Vector2 direction = (closestHouse.transform.position - transform.position).normalized;

		rigidbody2D.AddForce (direction * travelSpeed);
		rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed);
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		if (col.collider.tag == "Monster") {
			healthManager.currentHealth -= 20;
		}

		if (col.collider.tag == "House") {
			currentHitTargetCollision = col;
			Debug.Log ("New Target");
		}

	}

	void OnCollisionExit2D (Collision2D col) {
		if (col == currentHitTargetCollision) {
			currentHitTargetCollision = null;
			Debug.Log ("Target gone");
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
