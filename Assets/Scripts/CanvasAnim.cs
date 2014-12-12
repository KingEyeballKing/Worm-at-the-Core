using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class CanvasAnim : MonoBehaviour {
	public bool hasValue;
	private Transform _transform;
	private CanvasGroup _canvas;

	void Awake() {		
		_transform = transform;
		_canvas = gameObject.GetComponent<CanvasGroup>();
	}

	void Start() {
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		FadeIn(1f);
	}

	void Update() {
		if(hasValue) {
			if(Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("Fire1")) {
				FadeOut(1f);
				LevelLoader.Instance.WaitAndLoadNewGame(1f);
			}
		}
	}

	public void FadeIn(float duration) {
		_canvas.DOFade(1f, duration).SetEase(Ease.Linear);
	}

	public void FadeOut(float duration) {
		_canvas.DOFade(0f, duration).SetEase(Ease.Linear);
	}
 }
