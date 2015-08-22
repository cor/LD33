using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject closestHouse = FindClosestHouse ();

		transform.position = Vector2.MoveTowards(transform.position, closestHouse.transform.position, 1 * Time.deltaTime);
	}

	void OnCollisionEnter2D (Collision2D col) {
		Debug.Log ("OnCollisionEnter2D");
		Destroy(gameObject);
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
