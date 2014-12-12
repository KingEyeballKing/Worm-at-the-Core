using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public static LevelLoader Instance;

	public string LevelToLoad = "World";

	void Awake() {
		Instance = this;
	}

	public void LoadLevel(string level) {
		Application.LoadLevelAsync(level);
	}

	public void WaitAndLoadNewGame(float waitTime) {
		StartCoroutine(CoWaitAndLoad(waitTime));
	}

	IEnumerator CoWaitAndLoad(float t) {
		yield return new WaitForSeconds(t);
		LoadLevel(LevelToLoad);
	}
}
