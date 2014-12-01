using UnityEngine;
using System.Collections;

public class ThePlayer : MonoBehaviour {

	public static ThePlayer Instance;
	public Collider _collider;

	void Awake() {
		Instance = this;
		_collider = this.GetComponent<CharacterController>().collider;
	}
}