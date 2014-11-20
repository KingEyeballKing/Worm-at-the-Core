using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Beacon : MonoBehaviour
{
	public float MinRange = 4.0f;
	public float MaxRange = 15.0f;
	public float MinLightIntensity = 0f;
	public float MaxLightIntensity = 1.0f;
	// Determines how much power to purify this beacon.
	public float MyDarknessLevel = 5.0f;
	public Color MyLightColor = Color.white;
	public Material MyMaterial;

	private bool isDark;
	private float sqrDistance, sqrExtent, initialDarkness;
	private Transform _myTransform, _myLight, playerTransform;
	private Bounds myBounds;
	private Vector3 myExtent;
	
	void Awake()
	{
		_myTransform = transform;
	}

	void Start()
	{
		initialDarkness = MyDarknessLevel;
		if (renderer.material == null)
			renderer.material = MyMaterial;
		isDark = MyDarknessLevel > 0f;

		// Calculates the maxmium bounding box of the object.
		myBounds = this.collider.bounds;
		myExtent = myBounds.extents;
		sqrExtent = myExtent.magnitude * myExtent.magnitude;

		// Squares range values to make it faster to compare.
		MinRange *= MinRange;
		MaxRange *= MaxRange;
		
		playerTransform = GameObject.FindWithTag("Player").transform;
		InstantiateAndParentLight();
		StartCoroutine("BrightenOrDarken");
	}

	void Update()
	{
		sqrDistance = (playerTransform.position - _myTransform.position).sqrMagnitude;
		// If player is in range and beacon has darkness or brightness, make it active.
		if (Helper.InRange(sqrExtent, MinRange, MaxRange, sqrDistance) && (MyDarknessLevel != 0f))
			ActivateBeacon();
	}
	
	private void InstantiateAndParentLight()
	{
		GameObject myLight = new GameObject("My Light");
		myLight.AddComponent<Light>();
		myLight.light.type = LightType.Point;
		myLight.light.color = MyLightColor;
		myLight.light.shadows = LightShadows.None;
		myLight.light.range = MaxLightIntensity * 15;
		_myLight = myLight.transform;
		_myLight.parent = _myTransform;
		if (isDark)
			_myLight.light.intensity = MinLightIntensity;
		else
			_myLight.light.intensity = MaxLightIntensity;
		_myLight.position = _myTransform.position + new Vector3(0, _myTransform.localScale.y, 0);
	}

	void UpdateLightPosition()
	{
		// Light's new Y position equals to parent height plus darkness growth factor.
		var newYPos = _myTransform.localScale.y + (Mathf.Abs(initialDarkness) / 10);
		Vector3 newPos = _myTransform.position + new Vector3(0, newYPos, 0);
		TweenParms parms = new TweenParms();
		parms.Prop("position", newPos);
		HOTween.To(_myLight, 1, parms);
	}
	
	// Activates object and checks for mouse input if player is in range.
	void ActivateBeacon()
	{		
		// Reduces player power and self darkness level on mouse click.
		if (Input.GetMouseButtonDown(0))
		{
			PowerManager.currentPower -= MyDarknessLevel;
			PowerManager.isDecaying = true;
			MyDarknessLevel = 0f;
			StartCoroutine("BrightenOrDarken");
		}
	}

	// Controls how much light beacon emits based on darkness level.
	IEnumerator BrightenOrDarken()
	{
		// Only execute if beacon has been interacted with.
		if (MyDarknessLevel != 0)
			yield break;

		MeshRenderer MRenderer = GetComponent<MeshRenderer>();
		// sizeDiff is the darkness growth factor.
		float sizeDiff = Mathf.Abs(initialDarkness) / 10;
		float newYPos = _myTransform.localScale.y / 2;
		TweenParms parms = new TweenParms();
		// Set ease type here for both bright and dark beacons.
		parms.Ease(EaseType.EaseInOutExpo);
		Color lerpedColor;
		var lerpT = 0f;
		var animDuration = 1.0f;

		if (isDark)
		{
			sizeDiff += _myTransform.localScale.x;
			parms.Prop("position", new Vector3(_myTransform.position.x, newYPos, _myTransform.position.z));
			parms.Prop("localScale", new Vector3(sizeDiff, sizeDiff, sizeDiff));
			HOTween.To(_myTransform, animDuration, parms);
			UpdateLightPosition();

			while (_myLight.light.intensity < MaxLightIntensity)
			{
				// Light up beacon.
				_myLight.light.intensity += Time.deltaTime;
				lerpedColor = Color.Lerp(MRenderer.material.color, Color.white, lerpT);
				MRenderer.material.color = lerpedColor;
				lerpT += Time.deltaTime / animDuration;
				yield return null;
			}
		} else
		{
			sizeDiff = Mathf.Abs(sizeDiff - _myTransform.localScale.x);
			parms.Prop("position", new Vector3(_myTransform.position.x, newYPos, _myTransform.position.z));
			parms.Prop("localScale", new Vector3(sizeDiff, sizeDiff, sizeDiff));
			HOTween.To(_myTransform, animDuration, parms);
			UpdateLightPosition();

			while (_myLight.light.intensity > MinLightIntensity)
			{
				// Darken up beacon.
				_myLight.light.intensity -= Time.deltaTime;
				lerpedColor = Color.Lerp(MRenderer.material.color, Color.black, lerpT);
				MRenderer.material.color = lerpedColor;
				lerpT += Time.deltaTime / animDuration;
				yield return null;
			}
		}
		yield return new WaitForSeconds(animDuration);
	}
}