using UnityEngine;
using System.Collections;

// This class is just a position marker where the Interaction Zone will be loaded.

public class InteractonMarker : MonoBehaviour {

	[HideInInspector]
	public GameObject InteractionZone;

	private Transform _transform;

	void Awake() {
		_transform = transform;
	}

	void Start() {
		InteractionZone = Instantiate(Resources.Load("InteractionVolume_src"), 
		                              _transform.position, 
		                              _transform.rotation) as GameObject;
		InteractionZone.transform.parent = _transform;
	}
}
