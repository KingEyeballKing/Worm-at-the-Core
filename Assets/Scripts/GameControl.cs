using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DG.Tweening;

public class GameControl : MonoBehaviour {

	public static GameControl Instance;

	public Image fadeImage;
	public GameObject _player;
	public GameObject _mainCamera;
	public GameObject _timeOfDay;

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

		if (_player == null)
			_player = GameObject.FindWithTag("Player");

		if (_mainCamera == null)
			_mainCamera = GameObject.FindWithTag("MainCamera");

		if (_timeOfDay == null)
			Debug.LogError("Time of Day field is empty. Death won't work.");

		savePath = Application.persistentDataPath + "/worm.dat";

		Screen.lockCursor = true;
	}

	void Start() {
		fadeImage.color = new Color(0f, 0f, 0f, 1f);
		fadeImage.DOFade(0f, 3f).SetEase(Ease.Linear);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.X)) {
			TriggerDeath();
		}
	}

	public void TriggerDeath() {
		_player.GetComponent<ThePlayer>().isAlive = false;
		_timeOfDay.GetComponent<TimeOfDay>().slider = 1f;

		GenerateSoul(_player.transform.position);

		// Hide UI
		GameUI.Instance.HideAll();

		// Fade to black.
		fadeImage.color = new Color(0f, 0f, 0f, 0f);
		fadeImage.DOFade(1f, 5f).SetEase(Ease.Linear);

		// Disable input.
		_player.GetComponent<PlayerMotor>().SetControllable(false);
		_player.GetComponent<MouseLook>().SetControllable(false);
		_mainCamera.GetComponent<MouseLook>().SetControllable(false);
		
		// SaveWorld();

		StartCoroutine("ResetGame");

		// Application.Quit();
	}

	private IEnumerator ResetGame() {
		yield return new WaitForSeconds(15f);
		Application.LoadLevel(0);
	}

	private void GenerateSoul(Vector3 pos) {
		var newSoul = GameObject.Instantiate(Resources.Load("Soul_src"), pos, Quaternion.identity) as GameObject;
		GameControl.Instance.SoulsList.Add(newSoul);
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