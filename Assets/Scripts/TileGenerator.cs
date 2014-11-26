using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour {

	public static TileGenerator Instance;
	public List<GameObject> TilesPool = new List<GameObject>();

	private int totalNumberOfTiles = 1;
	private GameObject newTile;

	void Awake() {
		Instance = this;
	}

	public void GenerateTile(Transform originTransform, bool isGoingDown) {
		if (TilesPool.Count > 0) {
			// Rotates the title if it's going backwards or down.
			Quaternion tileRotation = isGoingDown ? new Quaternion(0, 180, 0, 0) : Quaternion.identity;
			// Picks a random tile from the pool.
			int k = (int)UnityEngine.Random.Range(0, TilesPool.Count);
			newTile = Instantiate(TilesPool[k], 
			                      originTransform.position, 
			                      Quaternion.identity) as GameObject;
			newTile.name = "Tile_0" + totalNumberOfTiles.ToString();
			newTile.transform.parent = originTransform.parent;
			totalNumberOfTiles++;
			Debug.Log(totalNumberOfTiles.ToString());
			// Disable interaction volume script that was just used.
			originTransform.GetComponent<InteractionVolume>().enabled = false;
		} 
	}
}