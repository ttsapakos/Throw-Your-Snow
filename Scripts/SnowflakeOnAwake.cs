using UnityEngine;
using System.Collections;
// Purpose of this script:
// Tag the snowflakes as they spawn as either a right or left flake,
// So that the players can knock one from their side on to the other
// to indicate that they are ready to play
public class SnowflakeOnAwake : MonoBehaviour {

	void Start () {

		// Adds tags to spawned snowflakes
		if(gameObject.transform.position.x > 0){
			// Adds tag if snowflake is spawned on the right
			gameObject.tag = "RightSideSnowflake";
		} else {
			// Adds tag of snowflake is spawned on the left
			gameObject.tag = "LeftSideSnowflake";
		}

	}
}
