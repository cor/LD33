using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour {

	public GameObject healthImage;
	public GameObject targetToFollow;

	public Color minColor = Color.red;
	public Color maxColor = Color.green;

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

		healthImage.GetComponent<Image>().color = Color.Lerp(minColor, maxColor, Mathf.Lerp(0.0f, 1.0f, healthImage.transform.localScale.x));

	}

	public void setScale(float scale) {
		healthImage.GetComponent<RectTransform>().localScale = new Vector2(scale, 1);
	}

}
