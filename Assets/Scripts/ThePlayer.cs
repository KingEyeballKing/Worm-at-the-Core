using UnityEngine;
using System.Collections;

public class ThePlayer : MonoBehaviour {
	// Cache fields.
	public static ThePlayer Instance;
	public Collider _collider;
	public string _name = "";
	public float currentPower;
	public bool isDecaying = true;

	private Transform _transform;
	private float startingPower = 100f;
	private float decayPerSecond = 0.1f;
	private float _maxSpeed = 0f;
	private float _speedPerPower = 0f;
	// Duration of player life in minutes;
	private float lifeDuration = 10f;

	void Awake() {
		Instance = this;

		_collider = gameObject.GetComponent<CharacterController>().collider;
		_name = "Name";
		_transform = transform;

		_maxSpeed = gameObject.GetComponent<PlayerMotor>().movement.maxSpeed;
		_speedPerPower = _maxSpeed / startingPower;

		decayPerSecond = startingPower / (lifeDuration * 60f);
	}

	void Start() {
		currentPower = startingPower;
	}

	void Update() {
		// Checks to see if there's any power left, if not player dies, exit function.
		if (currentPower <= 0f) {
			currentPower = 0f;
			KillPlayer();
		}
		
		// Normal decay.
		if (currentPower >= decayPerSecond)
			currentPower -= decayPerSecond * Time.deltaTime;
		else if (currentPower < decayPerSecond)
			currentPower = 0f;

		ChangeAbilitiesBasedOnPower(currentPower);
	}

	private void ChangeAbilitiesBasedOnPower(float p) {
		gameObject.GetComponent<PlayerMotor>().movement.maxSpeed = _speedPerPower * p;
		// TODO: Connect with motor.
	}

	private void KillPlayer() {
		// TODO: Stop input.
		// TODO: Fade screen to black.
		// TODO: Store position.
		// TODO: Store game world state.
	}
}