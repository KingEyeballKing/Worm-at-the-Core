using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameControl : MonoBehaviour {

	public static GameControl Instance;

	[HideInInspector]
	public List<GameObject> TilesList = new List<GameObject>();
	[HideInInspector]
	public List<GameObject> PlaquesList = new List<GameObject>();
	[HideInInspector]
	public List<GameObject> SoulsList = new List<GameObject>();
	[HideInInspector]
	public List<GameObject> PropsList = new List<GameObject>();

	private string savePath = "";

	void Awake() {
		// SINGLETON
		if (Instance == null) {
			DontDestroyOnLoad(Instance);
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		savePath = Application.persistentDataPath + "/worm.dat";
	}

	public void SaveWorld() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(savePath, FileMode.Open);

		WorldState thisWorld = new WorldState();
		thisWorld.tilesList = TilesList;
		thisWorld.plaquesList = PlaquesList;
		thisWorld.soulsList = SoulsList;
		thisWorld.propsList = PropsList;

		bf.Serialize(file, thisWorld);
		file.Close();
	}

	public void LoadWorld() {
		if (File.Exists(savePath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(savePath, FileMode.Open);

			WorldState thisWorld = (WorldState)bf.Deserialize(file);

			file.Close();
		}
	}

	[Serializable]
	class WorldState {
		public List<GameObject> tilesList = new List<GameObject>();
		public List<GameObject> plaquesList = new List<GameObject>();
		public List<GameObject> soulsList = new List<GameObject>();
		public List<GameObject> propsList = new List<GameObject>();
	}
}