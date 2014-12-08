using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WorldTile : MonoBehaviour {

	public static WorldTile Instance;

	public enum TileTypes {
		Cube,
		Bridge,
		Stairs,
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
			switch(this.tileType) {
				case TileTypes.Mountain:
					return "mountain";
					break;
				case TileTypes.Pyramid:
					return "pyramid";
					break;
				default:
					return "legacy";
					break;
			}
		}
	}
	public bool isFirstTile = false;
	[HideInInspector]
	public bool isEpic = false;

	private Transform _myTransform;
	private Material _myMaterial;
	private Color _myColor;
	private Vector3 _initialPosition = Vector3.zero;

	void Awake() {
		Instance = this;
		_myTransform = transform;
		_myMaterial = _myTransform.GetComponent<MeshRenderer>().material;
		_myColor = _myMaterial.color;
		_initialPosition = _myTransform.position;

		// Set Epic tile flag.
		switch (tileType) {
			case TileTypes.Mountain:
				isEpic = true;
				break;
			case TileTypes.Pyramid:
				isEpic = true;
				break;
			default:
				isEpic = false;
				break;
		}

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.OutExpo;

		if (!isFirstTile) {
			if (isEpic) {
				PlayIntroAnimation(5f);
			} else {
				PlayIntroAnimation(2f);
			}
		}
	}

	void PlayIntroAnimation(float d) {
		_myTransform.position = new Vector3(_initialPosition.x, _initialPosition.y - (d * 2f), _initialPosition.z);
		_myTransform.DOMoveY(_initialPosition.y, d);
		// _myMaterial.color -= new Color(0f, 0f, 0f, 1f);
		// _myMaterial.DOFade(1f, 1f);
		_myMaterial.DOColor(Color.white, d).From();
		Camera.main.GetComponent<MainCamera>().ShakeCamera(d);
	}
}
