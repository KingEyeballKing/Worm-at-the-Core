using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour {

	public static TileGenerator Instance;

	public List<GameObject> AllTilesPool = new List<GameObject>();
	public List<GameObject> TilesPool = new List<GameObject>();
	public List<GameObject> EpicTilesPool = new List<GameObject>();
	public List<GameObject> PropsPool = new List<GameObject>();
	public GameObject Plaque;

	private GameObject newTile;

	void Awake() {
		Instance = this;
	}

	public void GenerateTile(Transform originTransform) {
		// Every 5 tiles, there's a 50% chance the next tile will be EPIC.
		if (GameControl.Instance.TilesList == null) {
			return;
		}
		var modulo = GameControl.Instance.TilesList.Count % 5;
		var epicChance = (int)UnityEngine.Random.Range(0, 11);

		if ((modulo == 0) && (epicChance >= 5)) {
			if (EpicTilesPool.Count > 0) {
				// Picks a random tile from the EPIC pool.
				int k = (int)UnityEngine.Random.Range(0, EpicTilesPool.Count);
				if (EpicTilesPool[k] != null) {
					newTile = Instantiate(EpicTilesPool[k], 
					                      originTransform.position, 
					                      originTransform.rotation) as GameObject;
					newTile.name = "Epic_Tile_0" + GameControl.Instance.TilesList.Count.ToString();
					newTile.transform.parent = originTransform.parent;
					GameControl.Instance.TilesList.Add(newTile);
					Debug.Log("Total number of tiles = " + GameControl.Instance.TilesList.Count.ToString());
					// Generate a plaque, tile is EPIC.
					GeneratePlaque(originTransform);
				} else {
					Debug.LogError("Trying to instantiate a null tile!");
				}
			}
		} else {
			if (TilesPool.Count > 0) {
				// Picks a random tile from the pool.
				int k = (int)UnityEngine.Random.Range(0, TilesPool.Count);
				if (TilesPool[k] != null) {
					newTile = Instantiate(TilesPool[k], 
					                      originTransform.position, 
					                      originTransform.rotation) as GameObject;
					newTile.name = "Tile_0" + GameControl.Instance.TilesList.Count.ToString();
					newTile.transform.parent = originTransform.parent;
					GameControl.Instance.TilesList.Add(newTile);
					Debug.Log("Total number of tiles = " + GameControl.Instance.TilesList.Count.ToString());
				} else {
						Debug.LogError("Trying to instantiate a null tile!");
				}
			} 
		}
	}

	public void GeneratePlaque(Transform originTransform) {
		if (Plaque != null) {
			GameObject newPlaque = Instantiate(Plaque, 
			                                   originTransform.position - new Vector3(0.5f, 0f, 0.5f), 
			                                   originTransform.rotation) as GameObject;
			newPlaque.name = "Plaque_0" + GameControl.Instance.PlaquesList.Count.ToString();
			newPlaque.GetComponent<Plaque>()._myName = ThePlayer.Instance._name;
			newPlaque.GetComponent<Plaque>()._myType = newTile.GetComponent<WorldTile>().typeString;
			newPlaque.transform.parent = originTransform.parent;
			GameControl.Instance.PlaquesList.Add(newPlaque);
		} else {
			Debug.LogError("No Plaque game object!");
		}
	}
}