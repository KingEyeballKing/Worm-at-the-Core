using UnityEngine;
using System.Collections;

public class TimeOfDay : MonoBehaviour {
	
	public float slider = 0f;
	public float slider2 = 0f;
	public float _startSlider = 0f;
	public float Hour = 0f;
	private float ToD = 0f;

	public Light Sun;
	public GameObject Moon;

	public float speed = 60f;

	public Color NightFogColor;
	public Color DuskFogColor;
	public Color MorningFogColor;
	public Color MiddayFogColor;

	public Color NightAmbientLight;
	public Color DuskAmbientLight;
	public Color MorningAmbientLight;
	public Color MiddayAmbientLight;

	public Color NightTint;
	public Color DuskTint;
	public Color MorningTint;
	public Color MiddayTint;

	public Material SkyBoxMaterial1;
	public Material SkyBoxMaterial2;

	public Color SunNight;
	public Color SunDay;

	private ThePlayer _player;

	void Awake() {
		_startSlider = slider;
		_player = GameObject.FindWithTag("Player").GetComponent<ThePlayer>();
	}

	void OnGUI() {
		if (slider >= 1f) {
			slider = 0f;
		}
		// slider = GUI.HorizontalSlider(new Rect(0f, -200f, 200f, 30f), slider, 0f, 1f);
	}

	void Update() {
		Hour = slider * 24f;
		ToD = slider2 * 24f;
		Sun.transform.localEulerAngles = new Vector3((slider * 360f) - 90f, 0f, 0f);

		// Only animate slider if player is alive.
		if (_player.isAlive)
			slider = (Time.realtimeSinceStartup / speed) + _startSlider;
		else
			slider = 1f;

		Sun.color = Color.Lerp(SunNight, SunDay, slider * 2f);

		if (slider < 0.5f) { 
			slider2 = slider; 
		}
		if (slider > 0.5f) { 
			slider2 = (1f - slider); 
		}

		Sun.intensity = (slider2 - 0.2f) * 1.7f;

		// float targetAlpha = (ToD <= 4.5f && ToD >= 18f) ? 1f : 0f;
		// float curAlpha = Moon.renderer.material.GetFloat("_Alpha");
		// Moon.renderer.material.SetFloat("_Alpha", Mathf.Lerp(curAlpha, targetAlpha, Time.deltaTime * 0.2f));

		// SKYBOX
		if (ToD < 4f) {
			// NIGHT
			RenderSettings.skybox = SkyBoxMaterial1;
			RenderSettings.skybox.SetFloat("_Blend", 0f);
			SkyBoxMaterial1.SetColor ("_Tint", NightTint);
			RenderSettings.ambientLight = NightAmbientLight;
			RenderSettings.fogColor = NightFogColor;
		}
		if (ToD >= 4f && ToD < 6f) {
			// DUSK
			RenderSettings.skybox = SkyBoxMaterial1;
			RenderSettings.skybox.SetFloat("_Blend", 0f);
			RenderSettings.skybox.SetFloat("_Blend", (ToD / 2f) - 2f);
			SkyBoxMaterial1.SetColor("_Tint", Color.Lerp(NightTint, DuskTint, (ToD / 2f) - 2f));
			RenderSettings.ambientLight = Color.Lerp(NightAmbientLight, DuskAmbientLight, (ToD / 2f) - 2f);
			RenderSettings.fogColor = Color.Lerp(NightFogColor, DuskFogColor, (ToD / 2f) - 2f);
		}
		if (ToD >= 6f && ToD < 8f) {
			// MORNING
			RenderSettings.skybox = SkyBoxMaterial2;
			RenderSettings.skybox.SetFloat("_Blend", 0f);
			RenderSettings.skybox.SetFloat("_Blend", (ToD / 2f) - 3f);
			SkyBoxMaterial2.SetColor("_Tint", Color.Lerp(DuskTint, MorningTint, (ToD / 2f) - 3f));
			RenderSettings.ambientLight = Color.Lerp(DuskAmbientLight, MorningAmbientLight, (ToD / 2f) - 3f);
			RenderSettings.fogColor = Color.Lerp(DuskFogColor, MorningFogColor, (ToD / 2f) - 3f);
		}
		if (ToD >= 8f && ToD < 10f) {
			// NOON
			RenderSettings.ambientLight = MiddayAmbientLight;
			RenderSettings.skybox = SkyBoxMaterial2;
			RenderSettings.skybox.SetFloat("_Blend", 1f);
			SkyBoxMaterial2.SetColor("_Tint", Color.Lerp(MorningTint, MiddayTint,  (ToD / 2f) - 4f));
			RenderSettings.ambientLight = Color.Lerp(MorningAmbientLight, MiddayAmbientLight, (ToD / 2f) - 4f);
			RenderSettings.fogColor = Color.Lerp(MorningFogColor, MiddayFogColor, (ToD / 2f) - 4f);
		}
	}
}