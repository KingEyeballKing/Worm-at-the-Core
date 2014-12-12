using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundControl : MonoBehaviour {

	public static SoundControl Instance;
	public AudioClip Note;

	void Awake() {
		Instance = this;
	}

	void Start() {
		audio.pitch = 1.25f;
	}

	public void PlayNote() {
		// audio.clip = Note;
		// audio.pitch = (float)UnityEngine.Random.Range(0.5f, 1.5f);
		audio.PlayOneShot(Note, 1f);
	}
}