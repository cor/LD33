using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float hitRate = 1f; // Delay between hits, in seconds

	public float hitDamage = 20f;
	public float hitDamageLevelFactor = 0.5f;
	public float hitDamageForLevel;

	public GameObject currentHitTargetCollision;
	public float previousHitTime;

	public float travelSpeed = 10f;
	public float travelSpeedLevelFactor = 0.2f;
	public float travelSpeedForLevel;

	void Start() {
		travelSpeedForLevel = travelSpeed * (1 + (GameLogic.Get ().GetCurrentLevel() * travelSpeedLevelFactor));

		hitDamageForLevel = hitDamage * Mathf.Pow (hitDamageLevelFactor, (float)GameLogic.Get ().GetCurrentLevel()); 
	}

	void FixedUpdate()
	{
		if (GameLogic.Get().IsPlaying ()) {
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

			rigidbody2D.velocity = new Vector2 (
				Input.GetAxis ("Horizontal") * travelSpeedForLevel * Time.deltaTime, 
				Input.GetAxis ("Vertical") * travelSpeedForLevel * Time.deltaTime);
		}
	}


	void Update() {
		if (currentHitTargetCollision != null) {
			if (previousHitTime + hitRate < Time.time) {
				currentHitTargetCollision.gameObject.GetComponent<HealthManager>().currentHealth -= hitDamageForLevel;
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
