using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetKey : MonoBehaviour {

	string Key;

	bool modifying;

	// Use this for initialization
	void Start () {
		Key = this.gameObject.transform.GetChild (1).GetComponent<Text>().text;
		modifying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (modifying == true) {
			KeyCode newKey = KeyCode.None;
			foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown (kcode)) {
					newKey = kcode;
				}
			}
			if (newKey != KeyCode.None) {
				InputManager.getInstance ().bindKey(Key, newKey);
				modifying = false;
			}
		}
		this.gameObject.transform.GetChild (0).GetComponent<Text> ().text = InputManager.getInstance ().keyBinds [Key].ToString ();
	}

	// Rebind on click
	public void Rebind(){
		modifying = true;
	}
}
