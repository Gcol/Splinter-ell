using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour {

	public Sprite[] animation;
	private int i = 0;
	private int length;
	private float timer;
	private float vitesse;

	// Use this for initialization
	void Start () {
		i = 0;
		vitesse = 0.07f;
		timer = Time.fixedTime;
		length = animation.Length;
	}
	
	// Update is called once per frame
	public void UpdateSprite(int i){
		if (i >= length)
			i = 0;
		this.GetComponent<Image> ().sprite = animation[i];
	}
}
