using UnityEngine;
using System.Collections;

public class DeathVolume : MonoBehaviour {

	private Collider _playerCollider;

	void Awake() {
		_playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
	}

	void OnTriggerExit(Collider _playerCollider){
		GameControl.Instance.TriggerDeath();
	}
}
