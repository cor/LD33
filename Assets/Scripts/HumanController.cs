using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	public Rigidbody2D rb;
	public float travelSpeed = 0.5f;
	public float maxSpeed = 1f; //Replace with your max speed

	public int health = 100;
	public int maxHealth = 100;


	public GameObject healthBarToClone;
	GameObject healthBar;

	void Start() {
		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		healthBar.GetComponent<UpdateHealthBar>().targetToFollow = gameObject;
	}


	void Update() {
		if (healthBar != null) {
			float newScale = (float)health / (float)maxHealth;
			healthBar.GetComponent<UpdateHealthBar>().setScale(newScale);
		}

		if (health == 0) {
			Destroy(gameObject);
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
			Debug.Log ("hit by monster");
			health -= 20;
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
