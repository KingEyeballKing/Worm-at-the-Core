using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

	public void LoadGameWorld() {
		Application.LoadLevelAsync("World");
	}
}
