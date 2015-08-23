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

	float currentMoveSpeed = 0;

	public Animator animator;

	void Start() {

		travelSpeedForLevel = travelSpeed * (1 + (GameLogic.GetInstance ().GetCurrentLevel() * travelSpeedLevelFactor));

		hitDamageForLevel = hitDamage * Mathf.Pow (hitDamageLevelFactor, (float)GameLogic.GetInstance ().GetCurrentLevel()); 
	}

	void FixedUpdate()
	{
		if (GameLogic.GetInstance().IsPlaying ()) {
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

			rigidbody2D.velocity = new Vector2 (
				Input.GetAxis ("Horizontal") * travelSpeedForLevel * Time.deltaTime, 
				Input.GetAxis ("Vertical") * travelSpeedForLevel * Time.deltaTime);
			currentMoveSpeed = rigidbody2D.velocity.magnitude;
		}
	}


	void Update() {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			// Animations
			if (animator != null) {
				animator.SetFloat ("MoveSpeed", currentMoveSpeed);
			}
			if (GetComponent<Rigidbody2D> ().velocity.y > 0) {
			}

			Rigidbody2D rb = GetComponent<Rigidbody2D> ();

			//		Vector2 relativePos = (Vector2)transform.position + GetComponent<Rigidbody2D>().velocity;
			//		Quaternion rotation = Quaternion.LookRotation(relativePos);
			//		transform.rotation = rotation;
			Vector2 v = rb.velocity;
			if (v.magnitude > 0.1f) {
				transform.rotation = Quaternion.AngleAxis (Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg, Vector3.forward);

			}



			if (currentHitTargetCollision != null) {
				if (previousHitTime + hitRate < Time.time) {
					currentHitTargetCollision.gameObject.GetComponent<HealthManager> ().currentHealth -= hitDamageForLevel;
					previousHitTime = Time.time;
				}
			}
		} else {
			if (animator != null) {
				animator.SetFloat ("MoveSpeed", 0.0f);
			}

			GetComponent<Rigidbody2D> ().Sleep ();
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (col.gameObject.tag == "Human") {
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
}
