using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Glyph : MonoBehaviour {
	
	public List<Texture> glyphTextures = new List<Texture>();

	void Awake() {
		if (glyphTextures.Count > 0) {
			int i = UnityEngine.Random.Range(0, glyphTextures.Count);
			gameObject.renderer.material.SetTexture("_Texture", glyphTextures[i]);
		} else {
			Debug.LogError("Missing Glyph textures!");
		}
	}
}
