using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;
	
	public AudioClip die;

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

	public void PlayDie() {
		GetComponent<AudioSource> ().PlayOneShot (die);
	}
}
