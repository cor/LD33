using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILogic : MonoBehaviour {

	public Text levelText;
	public Text roundText;

	public int currentLevel;
	public int currentRound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		levelText.text = "LEVEL " + (currentLevel + 1);
		roundText.text = "ROUND " + (currentLevel + 1);
	}
}
