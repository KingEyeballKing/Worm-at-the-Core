using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameUI : MonoBehaviour {

	public static GameUI Instance;
	public Text nameText, buttonPrompt;

	private float fadeInDuration = 0.5f;
	private float fadeOutDuration = 4f;

	void Awake() {
		Instance = this;

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.InOutExpo;

		nameText.text = ThePlayer.Instance._name;
		buttonPrompt.text = "";
	}

	void Start() {
		FadeTextOut(nameText, 5f);
	}

	public void ShowFounderName(string newName, string tileType) {
		nameText.color -= new Color(0f, 0f, 0f, 1f);
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
		nameText.text = newName + "'s " + t;
		FadeTextIn(nameText, fadeInDuration);
	}

	public void HideFounderName() {
		FadeTextOut(nameText, fadeOutDuration);
	}

	public void ShowSoulName(string soulName) {
		nameText.color -= new Color(0f, 0f, 0f, 1f);
		nameText.text = soulName + " died here.";
		FadeTextIn(nameText, fadeInDuration);
	}

	public void HideSoulName() {
		FadeTextOut(nameText, fadeOutDuration);
	}

	public void ShowButtonPrompt(string buttonType) {
		switch (buttonType) {
			case "Fire1":
				buttonPrompt.text = "Press A to interact";
				break;
			case "Jump":
				buttonPrompt.text = "Press B to jump";
				break;
			case "Sprint":
				buttonPrompt.text = "Hold Right Trigger to sprint";
				break;
			default:
				buttonPrompt.text = "";
				break;
		}
		FadeTextIn(buttonPrompt, 0.5f);
	}

	public void HideButtonPrompt() {
		FadeTextOut(buttonPrompt, 1f);
	}

	public void FadeTextIn(Text t, float d) {
		t.DOFade(1f, d);
	}

	public void FadeTextOut(Text t, float d) {
		t.DOFade(0f, d);
	}
}
