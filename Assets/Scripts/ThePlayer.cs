using UnityEngine;
using System.Collections;

public class ThePlayer : MonoBehaviour {

	public static ThePlayer Instance;
	public Collider _collider;
	public string _name = "";
	public float currentPower;
	public bool isDecaying = true;
	public GameObject TimeOfDayPrefab;

	private Transform _transform;
	private float startingPower = 100f;
	private float decayPerSecond = 0.1f;
	private float _maxSpeed = 0f;
	private float _speedPerPower = 0f;
	// private float _startTime = 0f;
	// Duration of player life in minutes;
	private float lifeDuration = 10f;

	void Awake() {
		Instance = this;

		_name = "Name";
		_collider = gameObject.GetComponent<CharacterController>().collider;
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
			Die();
		}

		// Normal decay.
		if (currentPower >= decayPerSecond)
			currentPower -= decayPerSecond * Time.deltaTime;
		else if (currentPower < decayPerSecond)
			currentPower = 0f;

		ChangeAbilitiesBasedOnPower(currentPower);
		ChangeTODBasedOnPower(currentPower);
	}

	private void ChangeAbilitiesBasedOnPower(float p) {
		gameObject.GetComponent<PlayerMotor>().movement.maxSpeed = _speedPerPower * p;
	}

	private void ChangeTODBasedOnPower(float p) {

	}

	private void GenerateSoul(Vector3 pos) {
		var newSoul = GameObject.Instantiate(Resources.Load("Soul_src"), pos, Quaternion.identity) as GameObject;
		GameControl.Instance.SoulsList.Add(newSoul);
	}

	private void Die() {
		StartCoroutine("CoDie");
	}

	private IEnumerator CoDie() {
		// TODO: Stop input.
		
		// TODO: Fade screen to black.

		// TODO: Check if player is grounded and then generate soul.
		// If not, add them to the Wall of the Fallen.
		GenerateSoul(_transform.position);

		GameControl.Instance.SaveWorld();
		yield break;
	}
}
