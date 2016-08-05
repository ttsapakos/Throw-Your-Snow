using UnityEngine;
using System.Collections;
using Leap;

// Purpose of this script:
// Control the movement and rotation of the shovel
public class ShovelPos : MonoBehaviour {
	
	// The distance from the camera to the screen, needed for movement accuracy
	private float distance_to_screen;
	// The position of the shovel
	private Vector3 posn;
	// Indicates whether the Leap Motion is connected
	private bool connected;
	// Controller for the hand used by the Leap Motion
	private Controller controller;
	// A buffer to correct x value positioning of the shovel
	private const float OFFSET = 4f;
	// The height limit for the shovel
	private const float HEIGHT_LIMIT = 6.75f;
	// Z position for the shovel
	private const float Z_POSITION = -1.0f;
	// The number of hands currently being detected
	public int handsCount;

	// variables for movement smoothing
	// container for previous frame data
	private ArrayList oldFrames = new ArrayList();
	// number of frames stored stored
	public int smoothFrames = 3;
	
	private Rigidbody2D rb2d;
	
	// Is this the rightmost player?
	public bool RightmostPlayer;
	
	void Start() {
		// Initialize the controller
		controller = new Controller();
		// Get the player gameobject
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		//If it's not null, set the connected variable to the parent's, else log the error
		if (player != null) {
			connected = player.GetComponent<Position>().connected;
			rb2d = GetComponent<Rigidbody2D>();
			transform.position = new Vector3(transform.position.x, transform.position.y, Z_POSITION);

		} else {
			Debug.Log("Player cannot be found!");
		}
		if (smoothFrames <= 0) {
			Debug.Log ("MUST NOT SET SMOOTHFRAMES TO 0 OR LESS");
			Application.Quit();
		}
		oldFrames.Capacity = smoothFrames;
		for (int i = 0; i < smoothFrames; i++) {
			oldFrames.Add(new Vector2(transform.position.x, transform.position.y));
		}
	}
	
	void FixedUpdate () {
		Frame frame = controller.Frame();
		// If the leap isn't connected, use the mouse, else use the leap
		handsCount = frame.Hands.Count;
		if (! connected) {
			UpdateWithMouse ();
		} else {
			UpdateWithHand (frame);
		}
	}
	
	// Update using the mouse, debug purposes
	void UpdateWithMouse () {
		// Set the distance to the screen
		distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		// Set the position to move to to the mouse location
		posn = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
		
		UpdatePosn(posn.x, posn.y);
	}
	
	// Update using one or more hands
	void UpdateWithHand (Frame frame) {
		// Y Axis buffer for the leap motion
		const float HAND_Y_OFFSET = 0.5f;
		
		// Make a new hand for the Leap
		Hand hand;
		// Grab hands based on number of hands
		if (frame.Hands.Count >= 2) {
			// If we are dealing with the right player, get the rightmost hand, else the leftmost
			if(RightmostPlayer) {
				hand = frame.Hands.Rightmost;
			} else {
				hand = frame.Hands.Leftmost;
			}
		} else 
		// if there's only one hand,   
		if (frame.Hands.Count == 1) {
			// grab the only hand available
			hand = frame.Hands.Rightmost;
			// if this shovel is assigned to the right player
			if (RightmostPlayer) {
				// and the hand is on the left side of the screen
				if (hand.PalmPosition.x <= 0f) {
					rb2d.MoveRotation(rb2d.rotation);
					UpdatePosn(rb2d.position.x, rb2d.position.y);
					return;
				}
			} 
			// if this shovel is assigned to the left player
			else {
				// and the hand is on the right side of the screen
				if (hand.PalmPosition.x > 0f) {
					// do not update position
					UpdatePosn(rb2d.position.x, rb2d.position.y);
					rb2d.MoveRotation(rb2d.rotation);
					return;
				}
			}
		} else {
			rb2d.MoveRotation(rb2d.rotation);
			UpdatePosn(rb2d.position.x, rb2d.position.y);
			return;
		}
		
		// Get the distance to the screen
		distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		// Set the position to move to to the hand location
		posn = Camera.main.ScreenToWorldPoint(new Vector3(hand.PalmPosition.x, hand.PalmPosition.y, distance_to_screen ));
		// multipply X position to compensate for leap range
		posn.x = 3.15f * (posn.x + OFFSET);
		
		// Update position
		UpdatePosn(posn.x, posn.y + HAND_Y_OFFSET);
		
		// Get the rotation of the hand
		float rotation = Mathf.Rad2Deg * hand.Direction.Pitch;
		// set the rotation of the hand, changing from radians to degrees
		if (!RightmostPlayer) {
			// Invert rotation for the left hand
			rb2d.MoveRotation(-1.0f * rotation);
		} else {
			rb2d.MoveRotation(rotation);
		}
	}
	
	void UpdatePosn (float x, float y) {
		// Limit for x position so that the right player can't leave their side	
		const float RIGHT_BOUND = 0.35f;
		// Limit for x position so that the left player can't leave their side	
		const float LEFT_BOUND = -0.35f;
		// Invoke the height limit
		if (y > HEIGHT_LIMIT)
			y = HEIGHT_LIMIT;
		// This is to make sure that the player's can't leave their own side, set the limit to their x range of movement
		if(RightmostPlayer) {
			if(x < RIGHT_BOUND) {
				x = RIGHT_BOUND;
			}
		} else {
			if(x > LEFT_BOUND) {
				x = LEFT_BOUND;
			}
		}
		oldFrames.RemoveAt (0);
		oldFrames.Add (new Vector2 (x, y));
		float newX = 0;
		float newY = 0;
		for (int i = 0; i < smoothFrames; i++) {
			newX += ((Vector2) oldFrames[i]).x;
			newY += ((Vector2) oldFrames[i]).y;
		}
		newX /= smoothFrames;
		newY /= smoothFrames;
		// Set the position of the player to our new value
		rb2d.MovePosition (new Vector2(newX, newY));
	}
}