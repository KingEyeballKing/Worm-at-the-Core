using UnityEngine;
using System.Collections;

public class Soul : MonoBehaviour {

	public string _mySoulName = "John";

	private SphereCollider myCollider;
	private Collider _playerCollider;
	private bool isColliding = false;

	void Awake() {
		myCollider = gameObject.AddComponent<SphereCollider>() as SphereCollider;
		myCollider.radius = 2f;
		myCollider.isTrigger = true;

		Light myLight = gameObject.AddComponent<Light>() as Light;
		myLight.type = LightType.Point;
		myLight.range = 5f;
		myLight.intensity = 0.3f;
	}

	void Start() {
		_playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
	}

	void OnTriggerEnter(Collider _playerCollider) {
		isColliding = true;
		GameUI.Instance.ShowSoulName(_mySoulName);
	}

	void OnTriggerExit(Collider _playerCollider) {
		isColliding = false;
		GameUI.Instance.HideSoulName();
	}
}
