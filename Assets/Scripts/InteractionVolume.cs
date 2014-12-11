using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class InteractionVolume : MonoBehaviour {

	public static InteractionVolume Instance;

	public enum InteractionTypes {
		ActivateTheObject,
		UnlockTheBuilding,
		OpenNewArea
	}
	public InteractionTypes InteractionType { 
		get { return this.interactionType; } 
	}
	public InteractionTypes interactionType = InteractionTypes.OpenNewArea;
	public bool isGoingDown = false;

	private Transform _transform;
	private Collider _collider;
	private GameObject _glyph;
	private Transform _glyphTransform;
	private bool isColliding = false;

	void Awake() {
		Instance = this;
		_transform = transform;

		DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
		DOTween.defaultEaseType = Ease.InOutExpo;
	}

	void Start() {
		_collider = GameObject.FindWithTag("Player").GetComponent<Collider>();
		_glyph = Instantiate(Resources.Load("Glyph_src"), 
		                         _transform.position + new Vector3(0f, 1f, 0f), 
		                         _transform.rotation) as GameObject;
		_glyph.GetComponent<Transform>().parent = _transform;
		_glyphTransform = _glyph.GetComponent<Transform>();
		_glyphTransform.parent = _transform;
	}
	
	void Update() {
		// Start detecting input when player is in the vicinity.
		if (isColliding) {
			DetectInput();
		}
	}

	void OnDrawGizmos() {
		SphereCollider myCollider = this.GetComponent<SphereCollider>();
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, myCollider.radius);
	}

	void OnTriggerEnter(Collider _collider) {
		isColliding = true;
		// Debug.Log("isColliding = TRUE");
		GameUI.Instance.ShowButtonPrompt("Fire1");
	}

	void OnTriggerExit(Collider _collider) {
		isColliding = false;
		// Debug.Log("isColliding = FALSE");
		GameUI.Instance.HideButtonPrompt();
	}

	void DetectInput() {
		if (Input.GetKeyDown(KeyCode.E)) {
			GameUI.Instance.HideButtonPrompt();
			// TODO: check player power first.
			switch (interactionType) {
				case InteractionTypes.ActivateTheObject:
					ActivateObject();
					break;
				case InteractionTypes.UnlockTheBuilding:
					UnlockBuilding();
					break;
				case InteractionTypes.OpenNewArea:
					OpenArea();
					break;
			}
		}
	}

	void ActivateObject() {
		//TODO: Subtract cost from player power.
		gameObject.SetActive(false);
	}

	void UnlockBuilding() {
		// TODO: Subtract cost from player power.
		gameObject.SetActive(false);
	}

	void OpenArea() {
		StartCoroutine("CoOpenArea");
	}

	IEnumerator CoOpenArea() {
		var duration = 1f;
		// var punch = 0.2f;
		// _glyphTransform.DOPunchScale(new Vector3(punch, punch, punch), 0.5f, 50, 0.5f);

		_glyph.GetComponent<ParticleSystem>().Stop();
		_glyphTransform.DOScale(_glyph.transform.localScale * 2f, duration * 2f);
		_glyph.renderer.material.DOColor(Color.black, duration * 2f);

		yield return new WaitForSeconds(duration);
		
		TileGenerator.Instance.GenerateTile(_transform);

		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
		// TODO: Subtract cost from player power.
	}
}