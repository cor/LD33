using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float moveSpeed = 0.10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float horizontalIncrement = Input.GetAxis("Horizontal") * moveSpeed;
		float verticalIncrement = Input.GetAxis("Vertical") * moveSpeed;

		transform.position = new Vector2(transform.position.x + horizontalIncrement, transform.position.y + verticalIncrement);
	
	}
}
