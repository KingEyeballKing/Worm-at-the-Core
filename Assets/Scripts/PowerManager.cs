using UnityEngine;
using System.Collections;

public class PowerManager : MonoBehaviour
{
	public static PowerManager Instance;
	public static float currentPower;
	// Determines whether power is growing or shrinking at the moment.
	public static bool isDecaying = true;

	public GUIText CurrentPowerIndicator;
	private Transform _myTransform, _myLight;
	private float startingPower = 100f;
	// Power decay per second.
	private float normalDecay = 0.1f;
	private float powerMagnitude = 1.0f;
	
	// These variables deal with changing the player color based on power (TEMP).
	private MeshRenderer MRenderer;
	private Color lerpedColor, targetColor;
	
	void Awake()
	{
		_myTransform = transform;
		Instance = this;
	}
	
	void Start()
	{
		GameEventManager.KillPlayer += KillPlayer;
		GameEventManager.RevivePlayer += RevivePlayer;
		
		currentPower = startingPower;
		
		MRenderer = GetComponent<MeshRenderer>();
		
		// Instantiate and parent light.
		InstantiateLight();
	}
	
	void Update()
	{		
		// Checks to see if there's any power left, if not player dies, exit function.
		if (currentPower <= 0f)
		{
			currentPower = 0f;
			GameEventManager.TriggerPlayerDeath();
		}
		
		// Normal decay.
		if (currentPower >= normalDecay)
			currentPower -= normalDecay * Time.deltaTime;
		else if (currentPower < normalDecay)
			currentPower = 0f;
		
		// Controls speed, light, and player color based on current power.
		ChangeAppearanceBasedOnPower();
		ChangeAbilitiesBasedOnPower();
		
		_myLight.position = new Vector3(_myTransform.position.x, _myTransform.localScale.y + 5, _myTransform.position.z);
		
		// Temp GUI Text to show power level.
		CurrentPowerIndicator.text = "currentPower: " + ((int)currentPower).ToString();
		CurrentPowerIndicator.text += "\n isDecaying: " + isDecaying.ToString();
		
		HandleInput();
	}
	
	void HandleInput()
	{
		// Temporary key to instakill player.
		if (Input.GetKeyDown(KeyCode.K))
			GameEventManager.TriggerPlayerDeath();
		// Temporary key to insta-revive player.
		if (Input.GetKeyDown(KeyCode.L))
			GameEventManager.TriggerPlayerLife();
	}
	
	private void InstantiateLight()
	{
		GameObject myLight = new GameObject("Player Light");
		myLight.AddComponent<Light>();
		myLight.light.type = LightType.Point;
		myLight.light.shadows = LightShadows.None;
		myLight.light.intensity = currentPower / 100 * powerMagnitude;
		myLight.light.range = _myTransform.localScale.y * 20;
		myLight.light.color = Color.white;
		_myLight = myLight.transform;
		_myLight.parent = _myTransform;
		_myLight.position = _myTransform.position;
		_myLight.localPosition = new Vector3(0, _myTransform.localScale.y + 5, 0);
	}
	
	private void ChangeAppearanceBasedOnPower()
	{
		if (isDecaying)
		{
			targetColor = new Color(currentPower / 100, currentPower / 100, currentPower / 100);
			if (_myLight.light.intensity > currentPower / 100 * powerMagnitude)
				_myLight.light.intensity -= Time.deltaTime;
		} else
		{
			targetColor = new Color(currentPower / 100, currentPower / 100, currentPower / 100);
			if (_myLight.light.intensity < currentPower / 100 * powerMagnitude)
				_myLight.light.intensity += Time.deltaTime;
			else
				isDecaying = true;
		}
		lerpedColor = Color.Lerp(MRenderer.material.color, targetColor, Time.deltaTime);
		MRenderer.material.color = lerpedColor;
	}
	
	private void ChangeAbilitiesBasedOnPower()
	{
		// Sets TP_Motor's power modifier which will determine player speed.
		TP_Motor.Instance.CurrentPowerModifier = currentPower / startingPower;
	}

	// This is an GameEvent type function that gets added to GameEventManager.
	private void KillPlayer()
	{
		isDecaying = true;
		currentPower = 0f;
		CurrentPowerIndicator.text = "Dead";
		GetComponent<TP_Controller>().enabled = false;
	}

	// This is an GameEvent type function that gets added to GameEventManager.
	private void RevivePlayer()
	{
		isDecaying = false;
		currentPower += 100f;
		GetComponent<TP_Controller>().enabled = true;
	}
}