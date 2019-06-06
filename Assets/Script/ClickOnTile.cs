using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickOnTile : MonoBehaviour, IPointerClickHandler{ 

	public GameLogicManager game_logic;

	private int nb_tile_selected;
	private int state;
	private int index_tile;
	private int color;
	private int previous_state;
	private Sprite[] select_sprite;

	void Start () {
	}

	public void Reset(Sprite[] new_sprite){
		color = 4;
		state = 0;
		previous_state = 0;
		nb_tile_selected = 0;
		select_sprite = new_sprite;
		this.GetComponent<Image> ().sprite = select_sprite[0];
	}

	public void SwitchSprite(Sprite sprite)
	{
		this.gameObject.GetComponent<Image> ().sprite = sprite;
	}

	public void OnPointerClick(PointerEventData eventData)
	{

		if (previous_state == 1) {
			if (game_logic.SwapTile (this)) {
				state = 0;
			}
		}
		else if (previous_state == 2) {
			if (game_logic.CheckSelectedForm (this, index_tile, color, true)) {
			}	
		}
		if (previous_state != 1 && eventData.button == PointerEventData.InputButton.Left) {
			state = 1;
			if (game_logic.SwapTile (this)) {
				state = 0;
			}
		} 
		if (previous_state != 2 && eventData.button == PointerEventData.InputButton.Right && color != 4) {
			state = 2;
			if (game_logic.CheckSelectedForm (this, index_tile, color, false)) {
				state = 0;
			}
		}
		if (state == previous_state)
			state = 0;
		previous_state = state;
		SwitchSprite (select_sprite [state]);
		// OnClick code goes here ...
	}

	public GameObject GetGameObject(){
		return this.gameObject;
	}

	public void SetGameLogic(GameLogicManager game){
		game_logic = game;
	}
	public void SetPosition(Vector3 position){
		this.gameObject.transform.position = position;
	}

	public void SetImage(Sprite[] image, int color_values){
		color = color_values;
		this.GetComponent<Image> ().sprite = image[0];
		select_sprite = image;
		state = 0;
		previous_state = 0;
	}

	public Sprite[] GetInfo(){
		return select_sprite;
	}
		
	public int GetColor(){
		return color;
	}

	public void SetInfo(Sprite[] new_select_sprite, int color_values){
		color = color_values;
		select_sprite = new_select_sprite;
		this.GetComponent<Image> ().sprite = select_sprite [0];
		state = 0;
		previous_state = 0;
	}

	public void SetIndex(int index){
		index_tile = index;
	}

	public void SetNbTileSelected(int index){
		nb_tile_selected = index;
	}

	public int GetNbTileSelected(){
		return nb_tile_selected;
	}
}
