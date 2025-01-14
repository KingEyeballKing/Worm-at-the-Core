﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WorldTile : MonoBehaviour {

	public static WorldTile Instance;

	public enum TileTypes {
		Cube,
		Bridge,
		Stairs,
		StairsDown,
		Pyramid,
		Crossroads,
		Mountain
	}
	public TileTypes TileType { 
		get { return this.tileType; }
	}
	public TileTypes tileType = TileTypes.Cube;
	public string typeString {
		get {
			string r = "legacy";
			switch(this.tileType) {
				case TileTypes.Mountain:
					r = "mountain";
					break;
				case TileTypes.Pyramid:
					r = "pyramid";
					break;
				default:
					break;
			}
			return r;
		}
	}
	public bool isFirstTile = false;
	public bool isEpic = false;
	public float powerCost = 0.1f;

	private Transform _myTransform;
	private Material _myMaterial;
	// private Color _myColor;
	private Vector3 _initialPosition = Vector3.zero;
	private GameObject _timeOfDay;

	void Awake() {
		Instance = this;
		_myTransform = transform;
		_myMaterial = _myTransform.GetComponent<MeshRenderer>().material;
		// _myColor = _myMaterial.color;
		_initialPosition = _myTransform.position;
		_timeOfDay = GameObject.FindWithTag("TimeOfDay");

		if (isEpic) { powerCost = 0f - powerCost; }

		if (!isFirstTile) {
			if (isEpic) { PlayIntroAnimation(5f); } 
			else { PlayIntroAnimation(2f); }
		} else {
			if (GameControl.Instance != null)
				GameControl.Instance.TilesList.Add(gameObject);
		}
	}

	void Start() {
		// Subtract cost of this tile to player power. If it's EPIC, add to power instead.
		if (!isFirstTile) {
			var ToD = _timeOfDay.GetComponent<TimeOfDay>();
			if (ToD.slider > powerCost || isEpic) {
				ToD.slider += powerCost;
			}
		}
		SoundControl.Instance.PlayNote();
	}

	void PlayIntroAnimation(float d) {
		_myTransform.position = new Vector3(_initialPosition.x, _initialPosition.y - (d * 2f), _initialPosition.z);
		_myTransform.DOMoveY(_initialPosition.y, d);
		// _myMaterial.color -= new Color(0f, 0f, 0f, 1f);
		// _myMaterial.DOFade(1f, 1f);
		_myMaterial.DOColor(Color.white, d).From().SetEase(Ease.InExpo);
		Camera.main.GetComponent<MainCamera>().ShakeCamera(d);
	}
}
