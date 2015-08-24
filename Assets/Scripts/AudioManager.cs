using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;
	
	public AudioClip humanDies;
	public AudioClip monsterDies;

	public AudioClip[] playing;
	public AudioClip destroyed;
	public AudioClip completed;

	private AudioSource humanDiesAudioSource;

	private AudioSource playingAudioSource;
	private AudioSource availableAudioSource;

	public float backgroundAudioVolume = 1.0f;
	public float backgroundCrossfadeSpeed = 0.1f;

	public float minimumPlayingPitch = 1.5f;
	public float maximumPlayingPitch = 2.0f;
	public float pausedPlayingPitch = 1.0f;
	public float pitchChangeSpeed = 0.1f;

	public bool mute = false;

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

		humanDiesAudioSource = AddAudio (humanDies, false, false, 1.0f);

		playingAudioSource = AddAudio (completed, true, false, 1.0f);
		availableAudioSource = AddAudio (completed, true, false, 1.0f);
	}

	void Update() {
		if (playingAudioSource.volume < backgroundAudioVolume) {
			float newPlayingVolume = playingAudioSource.volume + backgroundCrossfadeSpeed * Time.deltaTime;
			playingAudioSource.volume = Mathf.Min(newPlayingVolume, backgroundAudioVolume);
		}
		
		if (availableAudioSource.volume > 0.0f) {
			float newPlayingVolume = availableAudioSource.volume - backgroundCrossfadeSpeed * Time.deltaTime;
			availableAudioSource.volume = Mathf.Max(newPlayingVolume, 0.0f);
		}

		float targetPitch = GetTargetPlayingPitch ();

		if (playingAudioSource.pitch != targetPitch) {
			float maximumChange = pitchChangeSpeed * Time.deltaTime;

			float difference = targetPitch - playingAudioSource.pitch;

			float change;

			if (difference > 0.0f) {
				change = Mathf.Min (difference, maximumChange);
			} else {
				change = Mathf.Max (difference, -maximumChange);
			}

			float newPitch = playingAudioSource.pitch + change;

			playingAudioSource.pitch = newPitch;
			availableAudioSource.pitch = newPitch;
		}

		if (Input.GetKeyUp (KeyCode.M)) {
			ToggleMute();
		}
	}

	public void ToggleMute()
	{
		AudioListener.pause = !AudioListener.pause;
	}

	private float GetTargetPlayingPitch()
	{
		float result;

		if (GameLogic.GetInstance ().IsPlaying ()) {
			result = minimumPlayingPitch + GameLogic.GetInstance().GetRoundPercentage() * (maximumPlayingPitch - minimumPlayingPitch);
		} else {
			result = pausedPlayingPitch;
		}

		return result;
	}

	public static AudioManager GetInstance() 
	{
		return instance;
	}
	
	public void HumanDies() {
		humanDiesAudioSource.pitch = UnityEngine.Random.Range (0.7f, 0.9f);
		humanDiesAudioSource.PlayOneShot (humanDies);
	}
	
	public void MonsterDies() {
		GetComponent<AudioSource> ().PlayOneShot (monsterDies);
	}

	public void GameStateChanged(GameLogic.GameState gameState) {
		AudioClip newClip;

		if (gameState == GameLogic.GameState.Completed) {
			newClip = completed;
		} else if (gameState == GameLogic.GameState.Destroyed) {
			newClip = destroyed;
		} else if (gameState == GameLogic.GameState.Playing) {
			int indexOfClip = GameLogic.GetInstance().GetCurrentTotalNumberOfRounds() % playing.Length;

			newClip = playing[indexOfClip];
		} else if (gameState == GameLogic.GameState.Initializing) {
			newClip = completed;
		} else if (gameState == GameLogic.GameState.Menu) {
			newClip = completed;
		} else {
			Debug.LogError("Unexpected GameState: " + gameState);
			return;
		}

		availableAudioSource.clip = newClip;
		availableAudioSource.time = playingAudioSource.time;
		availableAudioSource.pitch = playingAudioSource.pitch;
		availableAudioSource.volume = 0.0f;
		availableAudioSource.Play ();

		AudioSource temp = playingAudioSource;

		playingAudioSource = availableAudioSource;
		availableAudioSource = temp;
	}
	
	private AudioSource AddAudio(AudioClip clip, bool loop, bool playOnAwake, float volume) { 
		AudioSource result = gameObject.AddComponent<AudioSource>();
		result.clip = clip; 
		result.loop = loop;
		result.playOnAwake = playOnAwake;
		result.volume = volume; 
		
		return result; 
	}
}
