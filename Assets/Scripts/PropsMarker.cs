using UnityEngine;
using System.Collections;

// This class is just a position marker where the Prop will be loaded.

public class PropsMarker : MonoBehaviour {

	[HideInInspector]
	public GameObject propsMarker;

	private Transform _transform;
	private SphereCollider _collider;
	private Collider _playerCollider;
	private GameObject _myProp;
	private bool isColliding;

	void Awake() {
		_transform = transform;

		_playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();

		if (gameObject.GetComponent<Collider>() == null) {
			gameObject.AddComponent<SphereCollider>();
			_collider = gameObject.GetComponent<SphereCollider>();
			_collider.isTrigger = true;
			_collider.radius = 1f;
		}

		_myProp = TileGenerator.Instance.GenerateProp(_transform);
	}

	void Update() {
		if (isColliding) {
			DetectInput();
		}
	}

	void OnTriggerEnter(Collider _playerCollider) {
		isColliding = true;
	}

	void OnTriggerExit(Collider _playerCollider) {
		isColliding = false;
	}

	void DetectInput() {
		if (Input.GetKeyDown(KeyCode.E)) {
			if (_myProp != null)
				_myProp.GetComponent<Prop>().Activate(2f);
		}
	}
}