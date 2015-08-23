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
	public float levelDuration = 60;
	public float levelStartedTime;

	public enum GameState { Initializing, Playing, Destroyed, Completed }

	public GameState gameState;
	public GUIStyle style;

	public int minimumNumberOfHousesToProtect = 4;
	
	public float minimumDieCameraShake = 2.0f;
	public float maximumDieCameraShake = 3.0f;

	public static GameLogic GetInstance () {
		Debug.Assert (instance != null);

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
		SetGameState (GameState.Initializing);
	}
	
	void Update () {
		if (gameState == GameState.Initializing) {
			SetGameState (GameState.Playing);
		} else if (gameState == GameState.Playing) {
			if (IsLevelDestroyed ()) {
				Camera.main.GetComponent<CameraController>().Shake(UnityEngine.Random.Range (minimumDieCameraShake, maximumDieCameraShake));

				AudioManager.GetInstance ().MonsterDies ();

				SetGameState (GameState.Destroyed);
			} else if (IsLevelCompleted ()) {
				SetGameState (GameState.Completed);
			}
		}
	}

	void SetGameState(GameState newGameState) {
		gameState = newGameState;

		AudioManager.GetInstance ().GameStateChanged (newGameState);

		if (newGameState == GameState.Playing) {
			levelStartedTime = Time.time;
		}
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(10,10, 200, 30), "Level " + (currentLevel + 1), style);
		GUI.Label(new Rect(10,50, 200, 30), "Round " + (currentRound + 1), style);

		if (gameState == GameState.Destroyed) {

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Try again")) {
				Restart ();
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2 + 25, 150, 25), "Quit")) {
				Quit();
			}
		} else if (gameState == GameState.Completed) {

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Next Round")) {
				NextRound();
			}
		}
	}

	private void Restart() {
		currentLevel = INITIAL_LEVEL;
		currentRound = INITIAL_ROUND;
		
		SetGameState(GameState.Initializing);
		Application.LoadLevel (currentLevel);
	}
	
	private void Quit() {
		Application.Quit ();
	}

	void NextRound() {
		currentRound++;

		if (currentRound >= numberOfRounds) {
			currentRound = INITIAL_ROUND;
			currentLevel++;
		}

		SetGameState(GameState.Initializing);
		Application.LoadLevel (currentRound);
	}
	
	bool IsLevelDestroyed() {
		GameObject[] gos = GameObject.FindGameObjectsWithTag("House");
		
		return gos.Length < minimumNumberOfHousesToProtect;
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

	public float GetRoundPercentage() {
		return (Time.time - levelStartedTime) / levelDuration;
	}

	bool IsLevelCompleted() {
		return levelStartedTime + levelDuration < Time.time;
	}
}
