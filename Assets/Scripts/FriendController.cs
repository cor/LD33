using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendController : MonoBehaviour {

	public float travelSpeed = 0.5f;
	public float maxSpeed = 1f; //Replace with your max speed
	public float maximumTargetTime = 10f;

	public float targetTime;
	public GameObject target;

	float currentMoveSpeed = 0;

	public Animator animator;


	void Update() {
	
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
	}



	void FixedUpdate() {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (target == null || (Time.time - targetTime) > maximumTargetTime) {
				InitNextTarget ();
			}

			if (target != null) {
				Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();
				
				Vector2 direction = (target.transform.position - transform.position).normalized;
				
				rigidbody2D.AddForce (direction * travelSpeed);
				rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed);

				currentMoveSpeed = rigidbody2D.velocity.magnitude;
			}
		} else {
			GetComponent<Rigidbody2D> ().Sleep ();
		}
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			InitNextTarget ();
		} else {
			GetComponent<Rigidbody2D> ().Sleep ();
		}
	}

	void InitNextTarget() {
		target = FindNextTarget ();
		targetTime = Time.time;
	}

	GameObject FindNextTarget() {
		List<GameObject> candidates = new List<GameObject> ();

		AddCandidates (candidates, "House");
		AddCandidates (candidates, "Friend");
		AddCandidates (candidates, "Monster");

		GameObject result;

		if (candidates.Count != 0) {
			result = candidates[Random.Range (0, candidates.Count)];
		} else {
			result = null;
		}

		return result;
	}

	void AddCandidates(List<GameObject> candidates, string tag) {
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(tag)) {
			if (target != this && (target == null || target != gameObject)) {
				candidates.Add(gameObject);
			}
		}
	}
}
