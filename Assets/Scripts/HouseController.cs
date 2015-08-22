using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour {

	public int health = 100;
	public int maxHealth = 100;

	public GameObject healthBarToClone;
	GameObject healthBar;

	// Use this for initialization
	void Start () {
		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		healthBar.GetComponent<UpdateHealthBar>().targetToFollow = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (healthBar != null) {
			float newScale = (float)health / (float)maxHealth;
			healthBar.GetComponent<UpdateHealthBar>().setScale(newScale);
		}
		
		if (health == 0) {
			Destroy(gameObject);
		}
	
	}

	void OnCollisionEnter2D (Collision2D col) {

		if (col.collider.tag == "Human") {
			health -= 10;
		}

	}
}
