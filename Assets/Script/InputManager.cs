using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager {

	private static InputManager instance;

	public static InputManager getInstance()
	{
		if (instance == null) {
			instance = new InputManager();
		}
		return instance;
	}

	public Dictionary<string, KeyCode> keyBinds{get; set;}

	// Use this for initialization
	private InputManager () {
		keyBinds = new Dictionary<string, KeyCode> ();
		keyBinds.Add ("Ship_fire", KeyCode.UpArrow);
		keyBinds.Add ("Ship_left", KeyCode.LeftArrow);
		keyBinds.Add ("Ship_right", KeyCode.RightArrow);
		keyBinds.Add ("Ship_power", KeyCode.RightShift);
		keyBinds.Add ("Fish_right", KeyCode.D);
		keyBinds.Add ("Fish_left", KeyCode.Q);
		keyBinds.Add ("Fish_jump", KeyCode.Z);
		keyBinds.Add ("Fish_build", KeyCode.S);
		keyBinds.Add ("Fish_power", KeyCode.LeftShift);
		keyBinds.Add ("Escape", KeyCode.Escape);
	}
	
	//Redéfinition d'un keyBind 
	public void bindKey(string key, KeyCode keyBind){
		if (!keyBinds.ContainsValue(keyBind)) {
			keyBinds [key] = keyBind;
		}else{
			string tmpK = keyBinds.FirstOrDefault(x => x.Value == keyBind).Key;
			KeyCode tmpKB = keyBinds [key];
			keyBinds [key] = keyBind;
			keyBinds [tmpK] = tmpKB;
		}
	}
}
