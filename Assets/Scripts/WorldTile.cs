using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WorldTile : MonoBehaviour {

	public enum TileTypes {
		Cube,
		Bridge,
		Stairs
	}
	public TileTypes TileType { get { return this.tileType; } }
	public TileTypes tileType = TileTypes.Cube;
	public bool isFirstTile = false;

	private Transform _myTransform;
	private Vector3 _initialPosition = Vector3.zero;

	void Awake() {
		_myTransform = transform;
		_initialPosition = _myTransform.position;

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.OutExpo;

		if (!isFirstTile)
			PlayIntroAnimation();
	}

	void PlayIntroAnimation() {
		_myTransform.position = new Vector3(_initialPosition.x, _initialPosition.y - 50f, _initialPosition.z);
		_myTransform.DOMoveY(_initialPosition.y, 0.5f);
	}
}
