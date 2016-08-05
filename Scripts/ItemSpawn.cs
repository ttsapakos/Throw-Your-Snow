using UnityEngine;
using System.Collections;

// Purpose of this script:
// To spawn snowflakes out of frame above the players
public class ItemSpawn : MonoBehaviour {

	// How far up the items spawn
	private int spawnHeight = 9;
	// Initialize this so that we can have a spawn delay
	private float timeToSpawn = 0;

	// The item to spawn (a snowflake)
	public GameObject item;
	// Camera Position
	public GameObject cameraPosition;
	// To control how often the snowflakes fall
	public float spawnRate = 1;
	// To indicate whether the snowflakes are currently falling
	public bool isSpawning;

	
	void Start () {
		// start spawining at the beginning
		isSpawning = true;
	}

	void Update () {
		// If there has been as much time as the spawn delay, spawn a new item
		if ( Time.time > timeToSpawn && isSpawning) {
			// Reset the spawn timer
			timeToSpawn = Time.time + 1/spawnRate;	
			SpawnItem();
		}
	}


	// Spawn a new item
	void SpawnItem () {
		// Set the spawn position randomly, bounded by the range of the character's movement
		Vector3 spawnPosition = new Vector3 (Random.Range (-3.0f, 3.0f), spawnHeight, -1.0f);
		// Spawn rotation should be just none
		Quaternion spawnRotation = Quaternion.identity;
		// Instantiate the item
		Instantiate (item, spawnPosition, spawnRotation);
	}
}
