using UnityEngine;
using System.Collections;

public class DoveController : MonoBehaviour {

	public float moveSpeed = 3f;
	public float flyHeight = -8f;

	Vector2 moveVector;
	// Use this for initialization
	void Start () {
		GameObject monster = GameObject.Find ("Monster");
		moveVector = monster.transform.position - transform.position;
		moveVector *= 0.004f;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 oldPosition = transform.position;

		transform.position += (Vector3) moveVector;
		transform.position = new Vector3(transform.position.x, transform.position.y, flyHeight);

		if (transform.position.x > oldPosition.x) {
			transform.localScale = new Vector3(-1f - Mathf.Sin (Time.time * 2) * 0.25f, 1 + Mathf.Sin (Time.time * 2) * 0.25f, transform.localScale.z);
		} else {
			transform.localScale = new Vector3(1f + Mathf.Sin (Time.time * 2) * 0.25f, 1 + Mathf.Sin (Time.time * 2) * 0.25f, transform.localScale.z);
		}


	
	}
}
