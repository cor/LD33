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

	private AudioSource playingAudioSource;
	private AudioSource availableAudioSource;

	public float backgroundAudioVolume = 1.0f;
	public float backgroundCrossfadeSpeed = 0.1f;

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
	}

	public static AudioManager GetInstance() 
	{
		return instance;
	}
	
	public void HumanDies() {
		GetComponent<AudioSource> ().PlayOneShot (humanDies);
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
		} else {
			Debug.LogError("Unexpected GameState: " + gameState);
			return;
		}

		availableAudioSource.clip = newClip;
		availableAudioSource.time = playingAudioSource.time;
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
