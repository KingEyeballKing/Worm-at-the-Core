using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionVolume : MonoBehaviour {

	public static InteractionVolume Instance;

	public enum InteractionTypes {
		ActivateTheObject,
		UnlockTheBuilding,
		OpenNewArea
	}
	public InteractionTypes InteractionType { 
		get { return this.interactionType; } 
	}
	public InteractionTypes interactionType = InteractionTypes.OpenNewArea;
	public bool isGoingDown = false;

	private Collider _collider;
	private bool isColliding = false;

	void Awake() {
		Instance = this;
	}

	void Start() {
		_collider = GameObject.FindWithTag("Player").GetComponent<Collider>();
	}
	
	void Update() {
		// Start detecting input when player is in the vicinity.
		if (isColliding) {
			DetectInput();
		}
	}

	void OnDrawGizmos() {
		SphereCollider myCollider = this.GetComponent<SphereCollider>();
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, myCollider.radius);
	}

	void OnTriggerEnter(Collider _collider) {
		isColliding = true;
		Debug.Log("isColliding = TRUE");
	}

	void OnTriggerExit(Collider _collider) {
		isColliding = false;
		Debug.Log("isColliding = FALSE");
	}

	void DetectInput() {
		if (Input.GetKeyDown(KeyCode.E)) {
			// TODO: check player power first.
			switch (interactionType) {
				case InteractionTypes.ActivateTheObject:
					ActivateObject();
					break;
				case InteractionTypes.UnlockTheBuilding:
					UnlockBuilding();
					break;
				case InteractionTypes.OpenNewArea:
					OpenArea();
					break;
			}
		}
	}

	void ActivateObject() {
		//TODO: Subtract cost from player power.
		gameObject.SetActive(false);
	}

	void UnlockBuilding() {
		// TODO: Subtract cost from player power.
		gameObject.SetActive(false);
	}

	void OpenArea() {
		TileGenerator.Instance.GenerateTile(transform, isGoingDown);
		gameObject.SetActive(false);
		// TODO: Subtract cost from player power.
	}
}