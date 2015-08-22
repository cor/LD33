using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {
	
	public float currentHealth = 100;
	public float maxHealth = 100;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public float getPercentage () {
		return currentHealth / maxHealth;
	}
}
