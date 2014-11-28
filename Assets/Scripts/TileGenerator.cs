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
			// Picks a random tile from the pool.
			int k = (int)UnityEngine.Random.Range(0, TilesPool.Count);
			// TODO: If isGoingDown and tile is stairs, make stairs that go down.
			newTile = Instantiate(TilesPool[k], 
			                      originTransform.position, 
			                      originTransform.rotation) as GameObject;
			newTile.name = "Tile_0" + totalNumberOfTiles.ToString();
			newTile.transform.parent = originTransform.parent;
			totalNumberOfTiles++;
			Debug.Log("Total number of tiles = " + totalNumberOfTiles.ToString());
		} 
	}
}