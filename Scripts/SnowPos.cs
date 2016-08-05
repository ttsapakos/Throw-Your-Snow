using UnityEngine;
using System.Collections;

// Purpose of this script:
// To make the snowbanks on each side rise as the snow lands on that side
public class SnowPos : MonoBehaviour {

	// The structure used to store the scores
	private ScoreText scoreText;
	// The starting height of the snowbanks
	private float startHeight = 1.0f;
	// How quickly the snow rises
	public float snowRate = .01f;
	// Is this snowbank the right side snowbank?
	public bool isRight = false;

	void Start ()
	{
		// Get the gameController
		GameObject GM = GameObject.FindGameObjectWithTag ("GameController");
		// If it's not null, get it's ScoreText field, else log the error
		if (GM != null) {
				scoreText = GM.GetComponent<ScoreText> ();
		} else {
				Debug.Log ("GM cannot be found!");
		}
	}

	void Update ()
	{
		// If you're dealing with the right snowbank, look to the left player's score to determine the height, else vice versa
		if (isRight) {
			transform.position = new Vector3 (transform.position.x, (scoreText.scoreLeft * snowRate) + startHeight, transform.position.z);
		} else {
			transform.position = new Vector3 (transform.position.x, (scoreText.scoreRight * snowRate) + startHeight, transform.position.z);
		}
	}
}
