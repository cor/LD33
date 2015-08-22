using UnityEngine;
using System.Collections;

public class HumanSpawner : MonoBehaviour {

	public float timeBetweenSpawns = 2f;
	public GameObject human;



	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnHuman", 0, timeBetweenSpawns);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void SpawnHuman() {
		Instantiate(human, transform.position, Quaternion.identity);
	}

	void SpawnHuman(Vector2 position) {
		Instantiate(human, position, Quaternion.identity);
	}


}
