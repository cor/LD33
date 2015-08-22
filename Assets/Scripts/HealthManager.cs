using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {
	
	public int currentHealth = 100;
	public int maxHealth = 100;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public float getPercentage () {
		return (float)currentHealth / (float)maxHealth;
	}
}
