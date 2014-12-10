using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour {

	public static TileGenerator Instance;
	public List<GameObject> TilesPool = new List<GameObject>();
	public List<GameObject> EpicTilesPool = new List<GameObject>();
	public List<GameObject> PropsPool = new List<GameObject>();

	private int totalNumberOfTiles = 1;
	private int totalNumberOfPlaques = 0;
	private GameObject newTile;

	void Awake() {
		Instance = this;
	}

	public void GenerateTile(Transform originTransform, bool isGoingDown) {
		// Every 5 tiles, there's a 50% chance the next tile will be EPIC.
		var modulo = totalNumberOfTiles % 5;
		var epicChance = (int)UnityEngine.Random.Range(0, 11);

		if ((modulo == 0) && (epicChance >= 5)) {
			if (EpicTilesPool.Count > 0) {
				// Picks a random tile from the EPIC pool.
				int k = (int)UnityEngine.Random.Range(0, EpicTilesPool.Count);
				newTile = Instantiate(EpicTilesPool[k], 
				                      originTransform.position, 
				                      originTransform.rotation) as GameObject;
				newTile.name = "Epic_Tile_0" + totalNumberOfTiles.ToString();
				newTile.transform.parent = originTransform.parent;
				totalNumberOfTiles++;
				Debug.Log("Total number of tiles = " + totalNumberOfTiles.ToString());
				// Generate a plaque, tile is EPIC.
				GeneratePlaque(originTransform);
			}
		} else {
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

	public void GeneratePlaque(Transform originTransform) {
		if (PropsPool.Count > 0) {
			GameObject newPlaque = Instantiate(PropsPool[0], 
			                                   originTransform.position - new Vector3(0.5f, 0f, 0.5f), 
			                                   originTransform.rotation) as GameObject;
			newPlaque.name = "Plaque_0" + totalNumberOfPlaques.ToString();
			totalNumberOfPlaques++;
			newPlaque.GetComponent<Plaque>()._myName = ThePlayer.Instance._name;
			newPlaque.GetComponent<Plaque>()._myType = newTile.GetComponent<WorldTile>().typeString;
		}
	}
}