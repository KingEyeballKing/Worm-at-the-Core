using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NameField : MonoBehaviour {
	
	private InputField inputField;

	void Awake() {
		inputField = gameObject.GetComponent<InputField>();
	}

	void Update () {
		if(inputField.isFocused == false) {
			EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
			inputField.OnPointerClick(new PointerEventData(EventSystem.current));
		}
	}
}
