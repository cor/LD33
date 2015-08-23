using UnityEngine;
using System.Collections;
using System;

public class GameLogic : MonoBehaviour {

	private static GameLogic instance;

	private const int INITIAL_LEVEL = 0;
	private const int INITIAL_ROUND = 0;

	private static int currentLevel = INITIAL_LEVEL;
	private static int currentRound = INITIAL_ROUND;

	public int numberOfRounds = 2;
	public float levelDuration = 10;
	public float levelStartedTime;

	public enum GameState { Playing, Destroyed, Completed }

	public GameState gameState;
	public GUIStyle style;

	public int minimumNumberOfHousesToProtect = 4;

	public static GameLogic GetInstance () {
		return instance;
	}

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy( this.gameObject );
			return;
		} 
		else 
		{
			instance = this;
			
			DontDestroyOnLoad(instance.gameObject);
		}
	}

	void Start () {
		SetGameState (GameState.Playing);
	}
	
	void Update () {
		if (gameState == GameState.Playing) {
			if (IsLevelDestroyed ()) {
				AudioManager.GetInstance ().MonsterDies ();

				SetGameState (GameState.Destroyed);
			} else if (IsLevelCompleted ()) {
				SetGameState (GameState.Completed);
			}
		}
	}

	void SetGameState(GameState newGameState) {
		gameState = newGameState;

		if (newGameState == GameState.Playing) {
			levelStartedTime = Time.time;
		}

		AudioManager.GetInstance ().GameStateChanged (newGameState);
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(10,10, 200, 30), "Level " + (currentLevel + 1), style);
		GUI.Label(new Rect(10,50, 200, 30), "Round " + (currentRound + 1), style);

		if (gameState == GameState.Destroyed) {
			FreezeAllObjects();

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Try again")) {
				Restart ();
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2 + 25, 150, 25), "Quit")) {
				Application.Quit ();
			}
		} else if (gameState == GameState.Completed) {
			FreezeAllObjects();

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Next Round")) {
				NextRound();
			}
		}
	}

	void NextRound() {
		currentRound++;

		if (currentRound >= numberOfRounds) {
			currentRound = INITIAL_ROUND;
			currentLevel++;
		}

		SetGameState(GameState.Playing);
		Application.LoadLevel (currentRound);
	}
	
	bool IsLevelDestroyed() {
		GameObject[] gos = GameObject.FindGameObjectsWithTag("House");
		
		return gos.Length < minimumNumberOfHousesToProtect;
	}

	void FreezeAllObjects() {
		FreezeAllObjects ("Human");
		FreezeAllObjects ("Friend");
		FreezeAllObjects ("Monster");
	}

	public bool IsPlaying() {
		return gameState == GameState.Playing;
	}

	public int GetCurrentLevel() {
		return currentLevel;
	}

	public int GetCurrentTotalNumberOfRounds() {
		return currentLevel * numberOfRounds + currentRound;
	}

	void FreezeAllObjects(string tag) {
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(tag)) {
			Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

			if (rigidbody2D != null) {
				rigidbody2D.velocity = new Vector2(0,0);
			}
		}
	}
	
	bool IsLevelCompleted() {
		return levelStartedTime + levelDuration < Time.time;
	}

	private void Restart() {
		currentLevel = INITIAL_LEVEL;
		currentRound = INITIAL_ROUND;

		SetGameState(GameState.Playing);

		Application.LoadLevel (currentLevel);
	}
}
