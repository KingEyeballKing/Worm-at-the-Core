using UnityEngine;
using System.Collections;

public class ThePlayer : MonoBehaviour {
	// Cache fields.
	public static ThePlayer Instance;
	public Collider _collider;
	public string _name = "";
	private Transform _transform;

	public static float currentPower;
	// Determines whether power is growing or shrinking at the moment.
	public static bool isDecaying = true;

	private float startingPower = 100f;
	// Power decay per second.
	private float normalDecay = 0.1f;
	private float powerMagnitude = 1.0f;

	void Awake() {
		Instance = this;
		_collider = gameObject.GetComponent<CharacterController>().collider;
		_name = "Player Name";
		_transform = transform;
	}

	void Start() {
		currentPower = startingPower;
	}

	void Update() {
		// Checks to see if there's any power left, if not player dies, exit function.
		if (currentPower <= 0f)
		{
			currentPower = 0f;
			// GameEventManager.TriggerPlayerDeath();
		}
		
		// Normal decay.
		if (currentPower >= normalDecay)
			currentPower -= normalDecay * Time.deltaTime;
		else if (currentPower < normalDecay)
			currentPower = 0f;
	}

	private void ChangeAbilitiesBasedOnPower() {
		// TODO: Connect with motor.
	}
}