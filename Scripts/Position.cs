using UnityEngine;
using System.Collections;
using Leap;

// Purpose of this Script:
// Control the position of the players
public class Position : MonoBehaviour {
	
	// The distance to the screen so that we can have them move realistically
	private float distance_to_screen;
	// The mosition to move to
	private Vector3 posn;
	// The controller object for the hand
	private Controller controller;
	// A OFFSET to make the hand movement feel better
	private float OFFSET = 8.0f;

	// To control whether we are using Leap of Mouse controls
	public bool connected = false;
	// To know which hand is controlling which player
	public bool RightmostHand;
	
	
	void Start() {
		// Initialize the controller object
		controller = new Controller();
	}
	
	void Update () {
		// This is needed for the hand controls
		Frame frame = controller.Frame();
		if (! connected) {
			UpdateWithMouse ();
		} else {
			UpdateWithHand (frame);
		}
		
		// Resets the player's rotation so that when they are hit with an object they don't spin around
		transform.rotation = Quaternion.identity;
	}
	
	// To control the players using the mouse, this is for debug purposes
	void UpdateWithMouse () {
		// Calculate the distance from the player to the camera
		distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		// Calculate where we want our player to be using the new mouse position
		posn = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
		UpdatePosn(posn.x);
	}
	
	// To control the players using hand controls
	void UpdateWithHand (Frame frame) {
		// An object for our hand
		Hand hand;
		// Initialize the hand depending on whether it is the right or left hand
		if(RightmostHand) {
			hand = frame.Hands.Rightmost;
		} else {
			hand = frame.Hands.Leftmost;
		}
		
		// Calculate the distance from the player to the camera
		distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		// Calculate where we want our player to be using the new hand position
		posn = Camera.main.ScreenToWorldPoint(new Vector3(hand.PalmPosition.x, hand.PalmPosition.y, distance_to_screen ));
		// Limit the x position based on the player's side
		posn.x = 2.0f * (posn.x + OFFSET);
		UpdatePosn(posn.x);
		
	}

	// Uses the info from Update from Mouse or Update from Controller to set the new position
	void UpdatePosn (float x) {
		// right bound for the edge of the screen
		const float RIGHT_BOUND = 0.8f; 
		// left bound for the edge of the screen
		const float LEFT_BOUND = -0.9f;
		// The Y coordinate to draw the player at
		const float Y_POSITION = 2.2f;
		// The Z coordinate to draw the player at
		const float Z_POSITION = 3.0f;
		
		if(RightmostHand) {
			// Threshold for movement, so the players cannot go off their own side
			if(x < RIGHT_BOUND) {
				x = RIGHT_BOUND;
			}
		} else {
			if(x > LEFT_BOUND) {
				x = LEFT_BOUND;
			}
		}
		
		// Set the new position of the player
		transform.position = new Vector3( x, Y_POSITION, Z_POSITION );
	}
}
