
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	public float remplisage = 0f;
	public Sprite[] select_sprite;
	public GameLogicManager game_logic;
	public ChangeSprite generateur;
	private Image timer;
	private float vitesse = 0.01f;
	private bool actif;
	public int color;

	// Use this for initialization
	void Start () {
		actif = true;
		timer = GetComponent<Image>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (actif) {
			remplisage += vitesse;
			timer.fillAmount = remplisage;
			generateur.UpdateSprite ((int)(21 * remplisage));
			if (remplisage >= 1) {
				remplisage = 0;
				game_logic.GenerateRessource (select_sprite, color);
				Camera.main.gameObject.GetComponent<SoundManager> ().SpawnBlockSound ();
			}
		}
	}

	public void SetActif(bool actif_value){
		actif = actif_value;
	}

	public void SetVitesse(float vitesse_value){
		vitesse = vitesse_value;
	}

	public void ModifyVitesse(float modificateur_value){
		vitesse += modificateur_value;
	}
}