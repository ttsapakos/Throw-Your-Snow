using UnityEngine;
using System.Collections;

// Purpose of this script:
// To provide a structured way to hold the player's scores
public class ScoreText : MonoBehaviour {

	// The Graphic display of the left player's score
	public GUIText scoreTextLeftPlayer;
	// The Graphic display of the right player's score
	public GUIText scoreTextRightPlayer;
	// Left player's score
	public int scoreLeft = 0;
	// Right player's score
	public int scoreRight = 0;

	void Update () {
		// Update left score
		UpdateScoreLeft ();
		// Update right score
		UpdateScoreRight ();

	}

	// Add score to left player
	public void AddScoreLeft (int newScoreValue)
	{
		scoreLeft += newScoreValue;
		UpdateScoreLeft ();
	}

	// Display the left player's text and score
	void UpdateScoreLeft()
	{
		scoreTextLeftPlayer.text = "Score: " + scoreLeft;
	}

	// Add score to right player
	public void AddScoreRight (int newScoreValue)
	{
		scoreRight += newScoreValue;
		UpdateScoreRight ();
	}

	// Display the right player's text and score
	void UpdateScoreRight()
	{
		scoreTextRightPlayer.text = "Score: " + scoreRight;
	}
}
