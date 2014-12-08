using UnityEngine;
using System.Collections;

public class Plaque : MonoBehaviour {

	public string _myName = "Another Player";
	public string _myType = "";

	private Collider _playerCollider;
	private SphereCollider myCollider;
	private bool isColliding = false;

	void Awake() {
		// Add collider when Plaque is loaded.
		myCollider = gameObject.AddComponent<SphereCollider>();
		myCollider.radius = 1.5f;
		myCollider.isTrigger = true;
	}

	void Start() {
		_playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
	}

	void OnDrawGizmos() {
		SphereCollider myCollider = this.GetComponent<SphereCollider>();
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, myCollider.radius);
	}

	void OnTriggerEnter(Collider _playerCollider) {
		// Show name of the person who placed the plaque when player approaches.
		GameUI.Instance.ShowFounderName(_myName, _myType);
	}

	void OnTriggerExit(Collider _playerCollider) {
		GameUI.Instance.HideFounderName();
	}
}
