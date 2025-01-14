﻿using UnityEngine;
using System.Collections;

public class InteractionGraphic : MonoBehaviour {

	public float scrollSpeed = 1f;

    void Update() {
        float offset = Time.time * scrollSpeed;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, -offset);
    }
}
