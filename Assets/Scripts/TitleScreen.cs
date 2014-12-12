using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TitleScreen : MonoBehaviour {

	public GameObject NextCanvas;
	private CanvasGroup _canvas;

	void Awake() {
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		_canvas = gameObject.GetComponent<CanvasGroup>();
	}

	void Update() {
		if(Input.GetKeyUp(KeyCode.KeypadEnter) 
			   || Input.GetKeyUp(KeyCode.Return) 
			   || Input.GetKeyUp(KeyCode.E)
			   || Input.GetButtonUp("Fire1") 
			   || Input.GetButtonUp("Submit")) {
			StartCoroutine(FadeOutAndLoadNext(NextCanvas));
		}
	}

	void Start() {
		_canvas.alpha = 0f;
		FadeIn(1f);
	}

	public void FadeIn(float duration) {
		_canvas.DOFade(1f, duration).SetEase(Ease.Linear);
	}

	public void FadeOut(float duration) {
		_canvas.DOFade(0f, duration).SetEase(Ease.Linear);
	}

	IEnumerator FadeOutAndLoadNext(GameObject next) {
		FadeOut(1f);
		yield return new WaitForSeconds(1f);
		next.SetActive(true);
		gameObject.SetActive(false);
	}
}
