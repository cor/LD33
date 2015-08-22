using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float travelSpeed = 10f;

	public float hitRate = 1f; // Delay between hits, in seconds
	public int hitDamage = 20;
	public float hitDamageLevelFactor = 0.5f;
	
	private float hitDamageForLevel;

	public float travelSpeedLevelFactor = 0.2f;

	private float travelSpeedForLevel;

	private GameObject currentHitTargetCollision;
	private float previousHitTime;

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
