using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameUI : MonoBehaviour {

	public static GameUI Instance;
	public Text nameText, buttonPrompt;

	private float fadeInDuration = 0.5f;
	private float fadeOutDuration = 0.5f;

	void Awake() {
		Instance = this;
	}

	void Start() {
		nameText.text = ThePlayer.Instance._name + ", you have one life.";
		buttonPrompt.text = "";

		FadeTextOut(nameText, 15f);
	}

	public void ShowFounderName(string newName, string tileType) {
		string t = "";
		switch (tileType) {
			case "pyramid":
				t = "Pyramid";
				break;
			case "mountain":
				t = "Mountain";
				break;
			default:
				t = "Legacy";
				break;
		}
		buttonPrompt.text = newName + "'s " + t;
		FadeTextIn(buttonPrompt, fadeInDuration);
	}

	public void HideFounderName() {
		FadeTextOut(buttonPrompt, fadeOutDuration);
	}

	public void ShowSoulName(string soulName) {
		FadeTextIn(buttonPrompt, fadeInDuration);
		buttonPrompt.text = soulName + " died here.";
	}

	public void HideSoulName() {
		FadeTextOut(buttonPrompt, fadeOutDuration);
	}

	public void ShowButtonPrompt(string buttonType) {
		switch (buttonType) {
			case "Fire1":
				buttonPrompt.text = "Press E to interact";
				break;
			case "Jump":
				buttonPrompt.text = "Press Space to jump";
				break;
			case "Sprint":
				buttonPrompt.text = "Hold Shift to sprint";
				break;
			default:
				buttonPrompt.text = "";
				break;
		}
		FadeTextIn(buttonPrompt, 0.5f);
	}

	public void HideButtonPrompt() {
		FadeTextOut(buttonPrompt, 0.5f);
	}

	public void HideAll() {
		FadeTextOut(buttonPrompt, 0.5f);
		FadeTextOut(nameText, 0.5f);
	}

	public void FadeTextIn(Text t, float d) {
		t.DOFade(1f, d);
	}

	public void FadeTextOut(Text t, float d) {
		t.DOFade(0f, d);
	}
}
