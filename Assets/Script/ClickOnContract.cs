using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickOnContract : MonoBehaviour, IPointerClickHandler{ 

	private GameLogicManager game_logic;
	private Sprite light_image;
	private Sprite current_image;
	private int motif;
	private int form;
	private int color;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchImage(){
		if (this.gameObject.GetComponent<Image> ().sprite == current_image)
			this.gameObject.GetComponent<Image> ().sprite = light_image;
		else
			this.gameObject.GetComponent<Image> ().sprite = current_image;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		game_logic.ManageContrat ("ActiveContrat", this);
	}

	public void SetMotifFormColor(int current_motif, int current_form, int current_color){
		motif = current_motif;
		form = current_form;
		color = current_color;
	}

	public void SetSprite(Sprite current, Sprite light){
		current_image = current;
		light_image = light;
	}

	public void SetGameLogic(GameLogicManager game){
		game_logic = game;
	}

	public int GetMotif(){
		return motif;
	}

	public int GetForm(){
		return form;
	}

	public int GetColor(){
		return color;
	}
}
