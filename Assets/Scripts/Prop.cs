using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Prop : MonoBehaviour {

	private Transform _transform;
	private Material _myMaterial;
	private Color _color;
	private Vector3 _initialPosition = Vector3.zero;

	void Awake() {
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.Linear;

		_transform = transform;
		_myMaterial = _transform.GetComponent<MeshRenderer>().material;
		_color = _myMaterial.color;
		_initialPosition = _transform.position;
	}

	void Start() {
		// _myMaterial.color -= new Color(0f, 0f, 0f, 0.5f);
	}

	public void Activate(float d) {
		// _transform.position = new Vector3(_initialPosition.x, _initialPosition.y, _initialPosition.z);
		// _transform.DOMoveY(_initialPosition.y, d);
		_myMaterial.color = Color.white;
		_myMaterial.DOColor(_color, d);
	}
}
