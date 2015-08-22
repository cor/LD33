using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float travelSpeed = 10f;

	void FixedUpdate()
	{
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();

		rigidbody2D.velocity = new Vector2(
			Input.GetAxis("Horizontal") * travelSpeed * Time.deltaTime, 
			Input.GetAxis("Vertical") * travelSpeed * Time.deltaTime);
	}
}
