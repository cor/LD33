using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {
	
	public float currentHealth = 100;
	public float maxHealth = 100;

	public float getPercentage () {
		return currentHealth / maxHealth;
	}
}
