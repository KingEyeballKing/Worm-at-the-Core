using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainCamera : MonoBehaviour {

	// Values lower than 0.06f don't really work.
	public float ShakeMagnitude = 0.07f;
	public int Vibrato = 25;
	public float Randomness = 50f;

	public void ShakeCamera(float duration) {
		gameObject.GetComponent<Camera>().DOShakePosition(duration, ShakeMagnitude, Vibrato, Randomness);
	}
}
