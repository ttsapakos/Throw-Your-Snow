using UnityEngine;
using System.Collections;

//Purpose of this Script: 
// Destroy Snowflakes when they go off the screen
// This is so that the game isn't calculating the positions 
// of irrelevant objects and taking up computational power
public class DestroyByPosition : MonoBehaviour {

	// A reference to the Score Text so that we can add to it when a snowflake hits the ground
	private ScoreText scoreText;
	// The point at which the ground is
	private float destroyPoint = 1.8f;
	// The text to determine whether the players are ready
	private ReadyText readyText;
	// to detect whether the game is over to know whether to update the scores
	private bool gameOver;
	// the game contoller object
	GameObject GM;

	// How much a snowflake landing is worth
	public int scoreValue = 10;

	
	void Start () {
		// Find the GameController object so that we can get to the score text
		GM = GameObject.FindGameObjectWithTag ("GameController");
		if (GM != null) {
			// provide a reference for the scoretext, assuming it exists
			scoreText = GM.GetComponent<ScoreText>();
			// provide a reference for the readytext, assuming it exists
			readyText = GM.GetComponent<ReadyText>();
		} else {
			// Let the player know that the scoretext doesn't exist
			Debug.Log("Text couldn't be found!");
		}
	}
	
	void Update () {
		// If it's not null, get it's gameOver field, else log the error
		if (GM != null) {
			gameOver = GM.GetComponent<GameOver> ().gameOver;
		} else {
			Debug.Log ("GM cannot be found!");
		}

		// See it the position is below the threshold
		if(transform.position.y < destroyPoint) {
			// See if the snowflake is on the left or right side of the screen, which corresponds with each player's side
			if(transform.position.x > 0) {
				// If the snowflake spawned on the left, left player is ready
				if(gameObject.CompareTag("LeftSideSnowflake")){
					// Change left side ready boolean to true
					readyText.SetLeftSideReady();
					Debug.Log ("Left Side Ready");
				}
				// If the snowflake is on the right, add score to the left player and destroy snowflake
				GameObject.Destroy(this.gameObject);
				// if the game isn't over, add to the score
				if(!gameOver)
					scoreText.AddScoreLeft (scoreValue);
			} else {	
				// If the snowflake spawned on the right, right player is ready
				if(gameObject.CompareTag("RightSideSnowflake")){
					// Change right side ready boolean to true
					readyText.SetRightSideReady();
					Debug.Log ("Right Side Ready");
				}
				// If the snowflake is on the left, add score to the right player and destroy snowflake
				GameObject.Destroy(this.gameObject);
				// if the game isn't over, add to the score
				if(!gameOver)
					scoreText.AddScoreRight (scoreValue);
			}
		}
	}
}
