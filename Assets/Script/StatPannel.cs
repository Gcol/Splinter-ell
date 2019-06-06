using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPannel : MonoBehaviour {

	public GameLogicManager	game_logic;
	public Text 			nb_move_text;
	public Text 			time_play_text;
	public Text [] 			contract_text;
	public GameObject [] 	contract_stamp;

	private int 	state;
	private int		time;
	private int 	time_play;
	private int 	nb_move_total;
	private int 	nb_move;
	private int 	nb_contract;
	private int[] 	nb_contract_done;
	private float	vitesse;
	private float	last_execution;
	private int		motif;
	private int 	previous_state;

	// Use this for initialization
	void Start () {
		time_play = (int)game_logic.GetTimePlay();
		nb_move_total = game_logic.GetNbMove();
		nb_contract_done = game_logic.GetNbContractDone ();
		nb_move = -1;
		time = -1;
		//nb_move_text = this.transform.Find ("Move_value").GetComponent<Text>();
		//time_play_text = this.transform.Find ("Time_value").GetComponent<Text>();
		time_play_text.text = "0";
		nb_move_text.text = "0";
		nb_contract = 0;
		state = 6;
		motif = 0;
		previous_state = 0;
		vitesse = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (state < 5) {
			if (Time.fixedTime > last_execution + vitesse) {
				if (state == 0) {
					time += 1;
					time_play_text.text = time.ToString ();
					if (time >= time_play)
						state = 1;
				} else if (state == 1) {
					nb_move += 1;
					nb_move_text.text = nb_move.ToString ();
					if (nb_move >= nb_move_total)
						state = 2;
				}
				else if (state == 2) {
					contract_text [motif].text = nb_contract.ToString ();
					nb_contract += 1;
					if (nb_contract >  nb_contract_done [motif])
						state = 3;
				} else if (state == 3) {
					contract_stamp [motif].SetActive (true);
					Camera.main.gameObject.GetComponent<SoundManager> ().StampSound ();
					nb_contract = 0;
					motif += 1;
					if (motif >= 5)
						state = 5;
					else
						state = 2;
				}
				last_execution = Time.fixedTime;
				if (state == previous_state)
					vitesse -= vitesse / 10;
				else {
					previous_state = state;
					vitesse = 0.1f;
				}
			}
		}
		
	}

	public void LaunchStat(){
		state = 0;
	}
}
