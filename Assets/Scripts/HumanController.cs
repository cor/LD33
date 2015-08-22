using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	public Rigidbody2D rb;
	public float travelSpeed = 0.5f;
	public float maxSpeed = 1f; //Replace with your max speed

	void FixedUpdate()
	{
		GameObject closestHouse = FindClosestHouse ();

		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

		Vector2 direction = (closestHouse.transform.position - transform.position).normalized;

		rigidbody2D.AddForce (direction * travelSpeed);
		rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed);
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		Debug.Log ("OnCollisionEnter2D" + col.collider.tag);

		if (col.collider.tag.Equals("Monster")) {
			Destroy(gameObject);
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
