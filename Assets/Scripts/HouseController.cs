using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour {

	public GameObject healthBarToClone;
	GameObject healthBar;

	HealthManager healthManager;
	// Use this for initialization
	void Start () {
		healthManager = GetComponent<HealthManager>();

		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		healthBar.GetComponent<UpdateHealthBar>().targetToFollow = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (healthBar != null) {
			healthBar.GetComponent<UpdateHealthBar>().setScale(healthManager.getPercentage());
		}
		
		if (healthManager.currentHealth <= 0) {
			Destroy(gameObject);
		}
	
	}

}
