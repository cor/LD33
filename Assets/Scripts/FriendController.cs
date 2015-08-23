using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendController : MonoBehaviour {

	public float travelSpeed = 0.5f;
	public float maxSpeed = 1f; //Replace with your max speed
	public float maximumTargetTime = 10f;

	public float targetTime;
	public GameObject target;

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
