using UnityEngine;
using System.Collections;

public class MainLight : MonoBehaviour {

	public static MainLight Instance;
	public Transform _transform;

	void Awake() {
		Instance = this;
		_transform = transform;
	}

	void Update() {
		ChangeTimeOfDay();
	}

	public void ChangeTimeOfDay() {
		
	}

	void UpdateRotation() {

	}
}
