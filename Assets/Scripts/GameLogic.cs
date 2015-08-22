﻿using UnityEngine;
using System.Collections;
using System;

public class GameLogic : MonoBehaviour {

	private const int INITIAL_LEVEL = 0;
	private const int INITIAL_ROUND = 0;

	private const int NUMBER_OF_ROUNDS = 2;

	private static int currentLevel = INITIAL_LEVEL;
	private static int currentRound = INITIAL_ROUND;

	public float levelDuration = 10;

	private float levelStartedTime;

	enum State { Playing, Destroyed, Completed }

	private State state;

	void Start () {
		levelStartedTime = Time.time;
		
		state = State.Playing;
	}
	
	void Update () {
		if (state == State.Playing) {
			if (IsLevelDestroyed ()) {
				state = State.Destroyed;
			} else if (IsLevelCompleted ()) {
				state = State.Completed;
			}
		}
	}

	void OnGUI()
	{
		if (state == State.Destroyed) {
			FreezeAllObjects();

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Try again")) {
				Restart ();

				Application.LoadLevel (currentLevel);
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2 + 25, 150, 25), "Quit")) {
				Application.Quit ();
			}
		} else if (state == State.Completed) {
			FreezeAllObjects();

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Next Round")) {
				NextRound();

				Application.LoadLevel (currentRound);
			}
		}
	}

	void NextRound() {
		currentRound++;

		if (currentRound >= NUMBER_OF_ROUNDS) {
			currentRound = INITIAL_ROUND;
			currentLevel++;
		}
	}
	
	bool IsLevelDestroyed() {
		GameObject[] gos = GameObject.FindGameObjectsWithTag("House");
		
		return gos.Length == 0;
	}

	void FreezeAllObjects() {
		FreezeAllObjects ("Human");
		FreezeAllObjects ("Friend");
		FreezeAllObjects ("Monster");
	}

	public bool IsPlaying() {
		return state == State.Playing;
	}

	void FreezeAllObjects(string tag) {
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(tag)) {
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		}
	}
	
	bool IsLevelCompleted() {
		return levelStartedTime + levelDuration < Time.time;
	}

	private void Restart() {
		currentLevel = INITIAL_LEVEL;
		currentRound = INITIAL_ROUND;
	}
}
