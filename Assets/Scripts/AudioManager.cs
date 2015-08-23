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

	private AudioSource background1;
	private AudioSource background2;

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
		}

		background1 = AddAudio (completed, true, false, 1.0f);
		background2 = AddAudio (completed, true, false, 1.0f);
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

		AudioSource playingSource;
		AudioSource availableSource;
		
		if (background1.isPlaying) {
			availableSource = background2;
			playingSource = background1;
		} else {
			availableSource = background1;
			playingSource = background2;
		}

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

		availableSource.clip = newClip;
		availableSource.Play ();
		playingSource.Stop ();
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
