using UnityEngine;
using System.Collections;

// Purpose of this script:
// To detect if the game is over, display text if it is and provide restart mechanic
public class GameOver : MonoBehaviour {

	// The left player's score
	private int scoreLeft;
	// The right player's score
	private int scoreRight;
	// The structures in which the scores are stored
	private ScoreText scores;
	// The spawner script for the items
	private ItemSpawn spawner;
	// The text to display when there's a winner
	private string winner;
	// Is the game over?
	public bool gameOver;
	// The number of hands being detected
	private int handsCount;

	// The GUIText space for when the winner text has to be displayed
	public GUIText winnerText;

	GameObject shovel;

	void Start () {
		// get the ScoreText
		scores = (ScoreText)GetComponent ("ScoreText");
		// get the Spawner
		spawner = (ItemSpawn)GetComponent ("ItemSpawn");
		// Have the winner text displayed be nothing
		winner = "";
		// The game is not over at the start
		gameOver = false;

		shovel = GameObject.FindGameObjectWithTag ("Shovel");
	}
	
	void Update () {
		// Set the left score the to it's appropriate position
		scoreLeft = scores.scoreLeft;
		// Set the right score the to it's appropriate position
		scoreRight = scores.scoreRight;

		// Only run this if the game hasn't been won by a player yet
		if (!gameOver) {
			// Detect whether the game is over
			DetectGameOver ();
		}
		// The game text will be changed if the game has been won
		winnerText.text = winner;
		// If the game is over, enable restarting

		//If it's not null, set the connected variable to the parent's, else log the error
		if (shovel != null) {
			handsCount = shovel.GetComponent<ShovelPos>().handsCount;
		} else {
			Debug.Log("Player cannot be found!");
		}

		if(gameOver)
		{
			// If the player presses the 'R' key, reload the level
			if(handsCount == 0)
			{
				// Load the level
				Application.LoadLevel ("Ready");
			}
		}
	}

	// Detects whether either player has accumulated enough score to be a winner
	void DetectGameOver () {
		// If the player has more than 140 points, they win
		if (scoreLeft > 140) {
			// Limit the score so the height of the snow doesn't rise above the brick wall
			scores.scoreLeft = 150;
			// if the game is over, do nothing
			// Stop spawning the snowflakes
			spawner.isSpawning = false;
			// Display this text
			winner = "Left neighbor wins!\nRemove both hands to return to ready screen!";
			// The game is now over
			gameOver = true;
		}

		// See comments above
		if(scoreRight > 140) {
			scores.scoreRight = 150;
			spawner.isSpawning = false;
			winner = "Right neighbor wins!\nRemove both hands to return to ready screen!";
			gameOver = true;
		}
	}
}
