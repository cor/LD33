using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	
	private float originalTimescale;

	private bool levelDestroyed;

	void Start () {
		levelDestroyed = false;
		originalTimescale = Time.timeScale;
	}
	
	void Update () {

		if (!levelDestroyed && LevelDetroyed ()) {
			levelDestroyed = true;
			Time.timeScale = 0;
		}
	}

	void OnGUI()
	{
		if (LevelDetroyed ()) {
			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 25), "Try again")) {
				Time.timeScale = originalTimescale;
				Application.LoadLevel ("level_01");
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2 + 25, 150, 25), "Quit")) {
				Application.Quit ();
			}
		}
	}
	
	bool LevelDetroyed() {
		GameObject[] gos = GameObject.FindGameObjectsWithTag("House");

		return gos.Length == 0;
	}
}
