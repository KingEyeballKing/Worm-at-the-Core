using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ThePlayer : MonoBehaviour {

	public static ThePlayer Instance;
	public Collider _collider;
	public string _name = "Nameless Human";
	// public int _age = 0;
	public float currentPower;
	public bool isDecaying = true;
	public bool isAlive = true;
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

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.OutExpo;

		if(PlayerData.Instance != null)
			_name = PlayerData.Instance.Name;

		// _age = int.Parse((string)PlayerData.Instance.Age);
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
		// If the player is dead, do nothing.
		if (!isAlive) return;

		// Checks to see if there's any power left, if not player dies.
		if (currentPower <= 0f) {
			currentPower = 0f;
			Die();
		}

		// Normal decay.
		// if (currentPower >= decayPerSecond)
		// 	currentPower -= decayPerSecond * Time.deltaTime;
		// else if (currentPower < decayPerSecond)
		// 	currentPower = 0f;

		currentPower = ChangePowerBasedOnTOD();
		ChangeAbilitiesBasedOnPower(currentPower);
	}

	private void ChangeAbilitiesBasedOnPower(float p) {
		gameObject.GetComponent<PlayerMotor>().movement.maxSpeed = _speedPerPower * p;
	}

	private float ChangePowerBasedOnTOD() {
		var c = 100f - (TimeOfDayPrefab.GetComponent<TimeOfDay>().slider - TimeOfDayPrefab.GetComponent<TimeOfDay>()._startSlider) * 100f;
		return c;
	}

	private void Die() {
		GameControl.Instance.TriggerDeath();
		isAlive = false;
	}
}
