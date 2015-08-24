using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour {

	public GameObject healthImage;
	public GameObject targetToFollow;

	public Vector3 healthBarOffset = new Vector3(0, 0.5f, 3f);

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
		if (GameLogic.GetInstance ().IsPlaying ()) {
			if (targetToFollow != null) {
				float newX = targetToFollow.transform.position.x + healthBarOffset.x;
				float newY = targetToFollow.transform.position.y + healthBarOffset.y;
				float newZ = targetToFollow.transform.position.z + healthBarOffset.z;
				transform.position = new Vector3 (newX, newY, newZ);
			} else {

				Destroy (gameObject);
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

			healthImage.GetComponent<Image> ().color = newColor;
		}
	}

	public void setScale(float scale) {
		healthImage.GetComponent<RectTransform>().localScale = new Vector2(scale, 1);
	}

}
