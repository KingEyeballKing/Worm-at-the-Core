using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Prop : MonoBehaviour {

	private Transform _myTransform;
	private Material _myMaterial;
	private Vector3 _initialPosition = Vector3.zero;

	void Awake() {
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.Linear;

		_myTransform = transform;
		_myMaterial = _myTransform.GetComponent<MeshRenderer>().material;
		_initialPosition = _myTransform.position;
	}

	void Start() {
		PlayIntroAnimation(5f);
	}

	void PlayIntroAnimation(float d) {
		_myTransform.position = new Vector3(_initialPosition.x, _initialPosition.y - 2f, _initialPosition.z);
		_myTransform.DOMoveY(_initialPosition.y, d);
		_myMaterial.DOColor(Color.white, d).From();
	}
}
