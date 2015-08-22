using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float travelSpeed = 10f;
	public float maxSpeed = 5f; //Replace with your max speed

	void FixedUpdate()
	{
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

		Vector2 direction = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		rigidbody2D.AddForce (direction * travelSpeed);
		rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed);
	}
}
