using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WorldTile : MonoBehaviour {

	public enum TileTypes {
		Cube,
		Bridge,
		Stairs,
		Pyramid,
		Crossroads,
		Mountain
	}
	public TileTypes TileType { get { return this.tileType; } }
	public TileTypes tileType = TileTypes.Cube;
	public bool isFirstTile = false;

	private Transform _myTransform;
	private Material _myMaterial;
	private Vector3 _initialPosition = Vector3.zero;

	void Awake() {
		_myTransform = transform;
		_myMaterial = _myTransform.GetComponent<MeshRenderer>().material;
		_initialPosition = _myTransform.position;

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.OutExpo;

		if (!isFirstTile)
			PlayIntroAnimation();
	}

	void PlayIntroAnimation() {
		_myTransform.position = new Vector3(_initialPosition.x, _initialPosition.y - 10f, _initialPosition.z);
		_myTransform.DOMoveY(_initialPosition.y, 1f);
		_myMaterial.color = new Color(1f, 1f, 1f, 0f);
		_myMaterial.DOFade(1f, 1f);
	}
}
