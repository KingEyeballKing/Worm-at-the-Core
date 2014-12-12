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

		_collider = gameObject.AddComponent<SphereCollider>() as SphereCollider;
		_collider.isTrigger = true;
		_collider.radius = 2f;

		GenerateProp(_transform);
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

	private void DetectInput() {
		if (Input.GetKeyDown(KeyCode.E)) {
			if (_myProp != null)
				_myProp.GetComponent<Prop>().Activate(2f);
		}
	}

	private void GenerateProp(Transform originTransform) {
		if (TileGenerator.Instance.PropsPool.Count > 0) {
			int r = (int)UnityEngine.Random.Range(0, TileGenerator.Instance.PropsPool.Count);
			if (TileGenerator.Instance.PropsPool[r] != null) {
				GameObject newProp = Instantiate(TileGenerator.Instance.PropsPool[r], 
				                                 originTransform.position,
				                                 originTransform.rotation) as GameObject;
				newProp.name = "Prop_0" + GameControl.Instance.PropsList.Count.ToString();
				newProp.transform.parent = originTransform.parent;
				GameControl.Instance.PropsList.Add(newProp);
			} else {
				Debug.LogError("Trying to instantiate a null tile!");
			}
		}
	}
}