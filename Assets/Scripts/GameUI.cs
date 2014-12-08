using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameUI : MonoBehaviour {

	public static GameUI Instance;
	public Text myText;

	private float fadeInDuration = 1f;
	private float fadeOutDuration = 3f;

	void Awake() {
		Instance = this;

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.InOutExpo;

		myText.text = ThePlayer.Instance._name;
	}

	public void ShowFounderName(string newName, string tileType) {
		myText.color -= new Color(0f, 0f, 0f, 1f);
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
		myText.text += "\n" + newName + "'s " + t;
		FadeTextIn(fadeInDuration);
	}

	public void HideFounderName() {
		FadeTextOut(fadeOutDuration);
	}

	public void ShowSoulName(string soulName) {
		myText.color -= new Color(0f, 0f, 0f, 1f);
		myText.text += "\n" + soulName + " died here.";
		FadeTextIn(fadeInDuration);
	}

	public void HideSoulName() {
		FadeTextOut(fadeOutDuration);
	}

	public void FadeTextIn(float d) {
		myText.DOFade(1f, d);
	}

	public void FadeTextOut(float d) {
		myText.DOFade(0f, d);
	}
}
