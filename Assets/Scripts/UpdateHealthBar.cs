using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour {

	public RectTransform healthTransform;
	public GameObject targetToFollow;

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

	}

	public void setScale(float scale) {
		healthTransform.localScale = new Vector2(scale, 1);
	}

}
