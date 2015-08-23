using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour {

	public GameObject healthBarToClone;
	public GameObject healthBar;

	public HealthManager healthManager;
	public UpdateHealthBar updateHealthBar;

	void Start () {
		healthManager = GetComponent<HealthManager>();

		healthBar = (GameObject)Instantiate(healthBarToClone, transform.position, Quaternion.identity);
		updateHealthBar = healthBar.GetComponent<UpdateHealthBar> ();
		updateHealthBar.targetToFollow = gameObject;
	}
	
	void Update () {
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (healthBar != null) {
				updateHealthBar.setScale (healthManager.getPercentage ());
			}
			
			if (healthManager.currentHealth <= 0) {
				Destroy (gameObject);
			}
		}
	}

}
