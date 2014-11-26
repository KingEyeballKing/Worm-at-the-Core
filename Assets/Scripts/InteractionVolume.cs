using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionVolume : MonoBehaviour {

	public enum InteractionTypes {
		ActivateTheObject,
		UnlockTheBuilding,
		OpenTheArea
	}
	public InteractionTypes InteractionType { 
		get { return this.interactionType; } 
	}
	public InteractionTypes interactionType = InteractionTypes.OpenTheArea;
	public bool isGoingDown = false;

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
	}

	void OnTriggerExit(Collider playerCollider) {
		isColliding = false;
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
		//TODO: Subtract cost from player power.
	}

	void UnlockBuilding() {
		// TODO: Subtract cost from player power.
	}

	void OpenArea() {
		TileGenerator.Instance.GenerateTile(transform, isGoingDown);
		// TODO: Subtract cost from player power.
	}
}
