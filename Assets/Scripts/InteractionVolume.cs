using UnityEngine;
using System.Collections;

public class InteractionVolume : MonoBehaviour {

	public GameObject TargetObject;

	public enum InteractionTypes {
		ActivateTheObject,
		UnlockTheBuilding,
		OpenTheArea
	}

	public InteractionTypes InteractionType { get { return this.interactionType; } }
	public InteractionTypes interactionType = InteractionTypes.ActivateTheObject;

	private Collider playerCollider;
	private bool isColliding = false;

	void Start() {
		playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
	}
	
	void Update() {
		// Start detecting input when player is in the vicinity.
		if (isColliding) {
			DetectInput();
		}
	}

	void OnTriggerEnter(Collider playerCollider) {
		isColliding = true;
		Debug.Log("isColliding = " + isColliding.ToString());
	}

	void OnTriggerExit(Collider playerCollider) {
		isColliding = false;
		Debug.Log("isColliding = " + isColliding.ToString());
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
				case InteractionTypes.OpenTheArea:
					OpenArea();
					break;
			}
		}
	}

	void ActivateObject() {
		TargetObject.SetActive(true);
		//TODO: Subtract cost from player power.
	}

	void UnlockBuilding() {
		TargetObject.Unlock();
		// TODO: Subtract cost from player power.
	}

	void OpenArea() {
		TargetObject.SetActive(false);
		// TODO: Subtract cost from player power.
	}
}
