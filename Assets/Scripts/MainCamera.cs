using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainCamera : MonoBehaviour {

	// Values lower than 0.06f don't really work.
	[HideInInspector]
	public float ShakeMagnitude = 0.07f;
	[HideInInspector]
	public int Vibrato = 50;
	[HideInInspector]
	public float Randomness = 50f;

	private Transform _transform;
	private Vector3 _origin;

	void Awake() {
		_transform = transform;
	}

	public void ShakeCamera(float duration) {
		StartCoroutine("CoShakeCamera", duration);
	}

	IEnumerator CoShakeCamera(float d) {
		gameObject.GetComponent<Camera>().DOShakePosition(d, ShakeMagnitude, Vibrato, Randomness);
		yield return new WaitForSeconds(d);
		// ResetCamera();
		yield break;
	}

	void ResetCamera() {
		_transform.position = _origin;
	}
}
