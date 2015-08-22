using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour {

	public GameObject healthImage;
	public GameObject targetToFollow;

	public Color maxColor = Color.green;
	public Color midColor = Color.yellow;
	public Color minColor = Color.red;

	public float minMidColorPoint = 0.25f;
	public float midMaxColorPoint = 0.75f;

	// Use this for initialization
	void Start () {
	}


	// Update is called once per frame
	void Update () {
		if (targetToFollow != null) {
			transform.position = new Vector2(targetToFollow.transform.position.x, targetToFollow.transform.position.y + 0.5f);
		} else {

			Destroy(gameObject);
		}

		Color newColor;
		float xScale = healthImage.transform.localScale.x;

		if (xScale > 0.75f) {
			newColor = maxColor;
		} else if (xScale > 0.25f) {
			newColor = midColor;
		} else {
			newColor = minColor;
		}

		healthImage.GetComponent<Image>().color = newColor;


	}

	public void setScale(float scale) {
		healthImage.GetComponent<RectTransform>().localScale = new Vector2(scale, 1);
	}

}
