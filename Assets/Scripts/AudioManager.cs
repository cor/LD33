using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;
	
	public AudioClip humanDies;
	public AudioClip monsterDies;

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
}
