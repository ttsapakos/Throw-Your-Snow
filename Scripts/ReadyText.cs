using UnityEngine;
using System.Collections;

// Purpose of this script:
// To control the start of the game so that until both players are 
// indicated as 'Ready' the game won't start
public class ReadyText : MonoBehaviour {

	// The Text space for the Left Player
	public GUIText readyTextLeftPlayer;
	// The Text Space for the Right Player
	public GUIText readyTextRightPlayer;
	// Whether each player is ready
	public bool readyLeft = false;
	public bool readyRight = false;

	void Start () {
		// Set both GUIText fields to NOT READY at the start
		readyTextLeftPlayer.text = "NOT READY!";
		readyTextRightPlayer.text = "NOT READY!";
	}
	
	void Update () {
		if(readyLeft && readyRight){
			//yield return new WaitForSeconds(2);
			Application.LoadLevel("Play");
		}
	}

	// Sets the left side player to ready and changes the text associated
	public void SetLeftSideReady(){
		readyLeft = true;
		readyTextLeftPlayer.text = "READY!";
	}

	// Sets the right side player to ready and changes the text associated
	public void SetRightSideReady(){
		readyRight = true;
		readyTextRightPlayer.text = "READY!";
	}
}
