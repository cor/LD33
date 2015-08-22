using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float travelSpeed = 10f;

	public float hitRate = 1f; // Delay between hits, in seconds
	public int hitDamage = 20;

	GameObject currentHitTargetCollision;
	float previousHitTime;

	void FixedUpdate()
	{
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

		rigidbody2D.velocity = new Vector2(
			Input.GetAxis("Horizontal") * travelSpeed * Time.deltaTime, 
			Input.GetAxis("Vertical") * travelSpeed * Time.deltaTime);

	}


	void Update() {
		if (currentHitTargetCollision != null) {
			if (previousHitTime + hitRate < Time.time) {
				currentHitTargetCollision.gameObject.GetComponent<HealthManager>().currentHealth -= hitDamage;
				previousHitTime = Time.time;
			}
		}

	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Human") {
			currentHitTargetCollision = col.gameObject;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject == currentHitTargetCollision) {
			currentHitTargetCollision = null;
		}
	}
}
