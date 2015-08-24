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

	public enum GameState { Menu, Initializing, Playing, Destroyed, Completed }

	public GameState gameState;
	public GUIStyle scoreStyle;
	public GUIStyle titleStyle;
	public GUIStyle buttonStyle;

	public int minimumNumberOfHousesToProtect = 4;
	
	public float minimumDieCameraShake = 2.0f;
	public float maximumDieCameraShake = 3.0f;

	public int overlayDepth = 1;

	private Texture2D pauseOverlay;

	public int roundOffset = 1;

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
		currentRound = roundOffset;
		pauseOverlay = new Texture2D(1, 1, TextureFormat.ARGB32, false);

		// set the pixel values
		pauseOverlay.SetPixel(0, 0, new Color(.22f, .26f, .49f, 0.5f));

		// Apply all SetPixel calls
		pauseOverlay.Apply();

		SetGameState (GameState.Menu);
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
		if (gameState != GameState.Playing) {
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), pauseOverlay);
		}

		if (gameState != GameState.Menu) {
			GUI.Label (new Rect (10, 10, 200, 30), "LEVEL " + (currentLevel + 1), scoreStyle);
			GUI.Label (new Rect (10, 50, 200, 30), "ROUND " + (currentRound + 1), scoreStyle);
		} 

		float titleOffset = -150f;
		float playOffset = 0f;
		float menuOffset = 150f;
		float quitOffset = 225f;

		float width = Screen.width;
		float height = 50;

		if (gameState != GameState.Playing) {
			GUI.Label (new Rect (Screen.width / 2 - 75, Screen.height / 2 + titleOffset, 150, height), "SUDDEN PEACE", titleStyle);

			if (gameState == GameState.Menu) {
				
				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + playOffset, width, height), "PLAY GAME", buttonStyle)) {
					Restart ();
				}

				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + quitOffset, width, height), "QUIT", buttonStyle)) {
					Quit ();
				}
			} else if (gameState == GameState.Completed) {
				
				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + playOffset, width, height), "NEXT ROUND", buttonStyle)) {
					NextRound ();
				}

				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + menuOffset, width, height), "MENU", buttonStyle)) {
					SetGameState(GameState.Menu);
				}

				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + quitOffset, width, height), "QUIT", buttonStyle)) {
					Quit ();
				}
			} else if (gameState == GameState.Destroyed) {
				
				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + playOffset, width, height), "TRY AGAIN", buttonStyle)) {
					Restart ();
				}
				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + menuOffset, width, height), "MENU", buttonStyle)) {
					SetGameState(GameState.Menu);
				}
				if (GUI.Button (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 + quitOffset, width, height), "QUIT", buttonStyle)) {
					Quit ();
				}
			} 
		} 
	}

	private void Restart() {
		currentLevel = INITIAL_LEVEL;
		currentRound = INITIAL_ROUND + roundOffset;
		
		SetGameState(GameState.Initializing);
		Application.LoadLevel (currentRound);
	}
	
	private void Quit() {
		Application.Quit ();
	}

	void NextRound() {
		currentRound++;

		if (currentRound >= numberOfRounds) {
			currentRound = INITIAL_ROUND + roundOffset;
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
