using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WorldTile : MonoBehaviour {

	public static WorldTile Instance;

	public enum TileTypes {
		Cube,
		Bridge,
		Stairs,
		StairsDown,
		Pyramid,
		Crossroads,
		Mountain
	}
	public TileTypes TileType { 
		get { return this.tileType; }
	}
	public TileTypes tileType = TileTypes.Cube;
	public string typeString {
		get {
			string r = "legacy";
			switch(this.tileType) {
				case TileTypes.Mountain:
					r = "mountain";
					break;
				case TileTypes.Pyramid:
					r = "pyramid";
					break;
				default:
					break;
			}
			return r;
		}
	}
	public bool isFirstTile = false;
	public bool isEpic = false;
	public float powerCost = -2f;

	private Transform _myTransform;
	private Material _myMaterial;
	// private Color _myColor;
	private Vector3 _initialPosition = Vector3.zero;
	private GameObject _timeOfDay;
	private GameObject _player;

	void Awake() {
		Instance = this;
		_myTransform = transform;
		_myMaterial = _myTransform.GetComponent<MeshRenderer>().material;
		// _myColor = _myMaterial.color;
		_initialPosition = _myTransform.position;
		_timeOfDay = GameObject.FindWithTag("TimeOfDay");
		_player = GameObject.FindWithTag("Player");

		if (isEpic) { powerCost *= -1f; }

		if (!isFirstTile) {
			if (isEpic) { PlayIntroAnimation(5f); } 
			else { PlayIntroAnimation(2f); }
		} else {
			if (GameControl.Instance != null)
				GameControl.Instance.TilesList.Add(gameObject);
		}
	}

	void Start() {
		// Subtract cost of this tile to player power.
		if (!isFirstTile) {
			if (_player.GetComponent<ThePlayer>().currentPower > powerCost) {
				_player.GetComponent<ThePlayer>().currentPower -= powerCost;
			}
		}
		SoundControl.Instance.PlayNote();
	}

	void PlayIntroAnimation(float d) {
		_myTransform.position = new Vector3(_initialPosition.x, _initialPosition.y - (d * 2f), _initialPosition.z);
		_myTransform.DOMoveY(_initialPosition.y, d);
		// _myMaterial.color -= new Color(0f, 0f, 0f, 1f);
		// _myMaterial.DOFade(1f, 1f);
		_myMaterial.DOColor(Color.white, d).From().SetEase(Ease.InExpo);
		Camera.main.GetComponent<MainCamera>().ShakeCamera(d);
	}
}
