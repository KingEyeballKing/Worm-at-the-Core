using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;

	public string Name = "Nameless Human";
	public string Age = "";

	void Awake() {
		// SINGLETON
		if (Instance == null) {
			DontDestroyOnLoad(Instance);
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		Screen.lockCursor = true;
	}

	public void EnterName(Text name) {
		Name = name.text;
	}

	public void EnterAge(Text age) {
		Age = age.text;
	}
}
