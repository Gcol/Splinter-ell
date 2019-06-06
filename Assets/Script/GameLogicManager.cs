using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour {
	public int state;

	private float timer;

	public GameObject		ressource_blue;
	public GameObject 		ressource_on_red;
	public GameObject 		ressource_off_red;
	public GameObject 		ressource_on_yellow;
	public GameObject 		ressource_off_yellow;
	public GameObject 		tile;
	public GameObject 		pause_pannel;
	public GameObject 		game_over_pannel;
	public GameObject 		game_complete_pannel;
	public GameObject[] 	list_of_timer;
	private ClickOnContract	actif_contrat;

	public int[] form_contract_by_difficulty;
	public int[] difficulty_by_contract;
	public GridManager grid;
	public Transform parent_tile;
	public Transform parent_contract;
	public int taille_max_row;
	public int taille_max_column;
	public Sprite sprite_background;
	public Sprite othet_sprite_background;
	public Sprite[] list_contrat;
	public Sprite[] list_contrat_block;
	public Sprite[] list_contrat_light;
	public Sprite[] default_sprite_tile;
	public Text contrat_done_Text;
	public int max_column_tetri;
	public int  nb_generateur_total = 3;


	private int contract_done;
	private Timer tmp_time;
	private int nb_current_generateur;
	private int size_grid;
	private int tmp_color;
	private Image current_contract_background;
	private Image[] list_tile;
	private int[] list_index;
	private int nb_tile;
	private int nb_tile_selected;
	private int nb_tile_used;
	private Vector2Int[] list_selected_tile;
	private ClickOnTile[] list_selected_tile_object;
	private Sprite contrat_sprite;
	private ClickOnTile saved_move_tile;
	private ClickOnTile tmp_move_tile;
	private Sprite[] tmp_selected_sprite;
	private ClickOnContract goScript;
	private Vector3 tile_position;
	private bool on_pause;


	// Utiliser pour les statistiques.

	private int[]	nb_contract_done;
	private	int		nb_move;
	private float	time_play;

	//public GameObject endPanel;
	//public GameObject levelManager;

	// Use this for initialization
	void Start () {
		time_play = Time.time;
		Reset ();
	}

	void Reset(){
		ressource_blue.SetActive (true);
		ressource_off_red.SetActive (true);
		ressource_off_yellow.SetActive (true);
		ressource_on_red.SetActive (false);
		ressource_on_yellow.SetActive (false);
		UpdateContractDone(0);
		list_tile = new Image[taille_max_row * taille_max_column];
		list_selected_tile = new Vector2Int[taille_max_row * taille_max_column + 1];
		list_selected_tile_object = new ClickOnTile[taille_max_row * taille_max_column];
		size_grid = 0;// 0
		nb_tile = 0;
		nb_move = 0;
		UpdateGrid ();
		nb_tile_used = 0;
		nb_current_generateur = 1;
		GenerateContrat ();
		on_pause = false;
		nb_contract_done = new int[5];
	}

	public void SetPause()
	{
		for (int i = 0; i < list_of_timer.Length; i++) {
			tmp_time = (Timer)list_of_timer [i].GetComponent (typeof(Timer));
			tmp_time.SetActif (on_pause);
		}
		on_pause = !on_pause;
		pause_pannel.SetActive (on_pause);	
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SetPause ();
		}
	}

	void BeginGame(){
		Reset ();
	}

	void UpdateContractDone(int add_contract){
		if (add_contract == 0)
			contract_done = 0;
		else
			contract_done += add_contract;
		contrat_done_Text.text =  contract_done.ToString();
	}

	public void Finish_contract(int value = 1){
		int difficulty = 1;

		UpdateContractDone (value);
		if (contract_done < 20 && contract_done % 5 == 0 ) {
			tmp_time = (Timer)list_of_timer [0].GetComponent (typeof(Timer));
			tmp_time.ModifyVitesse (0.005f);
		}
		else if (contract_done == 20) {
			tmp_time = (Timer)list_of_timer [0].GetComponent (typeof(Timer));
			tmp_time.SetVitesse (0.01f);
			nb_current_generateur += 1;
			ressource_off_red.SetActive (false);
			ressource_on_red.SetActive (true);
		} else if (contract_done == 40) {
			nb_current_generateur += 1;
			ressource_off_yellow.SetActive (false);
			ressource_on_yellow.SetActive (true);
		} else if (contract_done == 55) {
			time_play = Time.time - time_play;
			Camera.main.gameObject.GetComponent<SoundManager> ().GameCompleteSound ();
			game_complete_pannel.SetActive (true);
			this.gameObject.SetActive (false);	
		}
		if (contract_done % 5 == 0) {
			Camera.main.gameObject.GetComponent<SoundManager> ().SuceedRowContractSound ();
			GenerateContrat ();
			if (contract_done < 20 || contract_done % 10 == 0)
				UpdateGrid ();
		}
	}

	private void AddTile(){
		list_tile [nb_tile] = Instantiate (tile, parent_tile).GetComponent<Image> ();
		list_tile [nb_tile].sprite = default_sprite_tile[0];
		tmp_move_tile = (ClickOnTile)list_tile [nb_tile].GetComponent (typeof(ClickOnTile));
		tmp_move_tile.SetGameLogic (this);
		tmp_move_tile.SetInfo (default_sprite_tile, 4);
		tmp_move_tile.SetIndex (nb_tile);
		nb_tile += 1;
	}
	public void UpdateGrid()
	{
		//if (add_column == true) {
		//	for (int i = 0; i < current_nb_row; i++) {
		//		list_tile [nb_tile] = Instantiate (tile, parent_tile).GetComponent<Image> ();
		//		list_tile [nb_tile].sprite = default_sprite_tile[0];
		//		tmp_move_tile = (ClickOnTile)list_tile [nb_tile].GetComponent (typeof(ClickOnTile));
		//		tmp_move_tile.SetGameLogic (this);
		//		tmp_move_tile.SetInfo (default_sprite_tile, 4);
		//		tmp_move_tile.SetIndex (nb_tile);
		//		nb_tile += 1;
		//	}
		// else {
		//
		//	for (int i = 0; i < current_nb_column; i++) {
		//		list_tile [nb_tile] = Instantiate (tile, parent_tile).GetComponent<Image> ();
		//		list_tile [nb_tile].sprite = default_sprite_tile[0];
		//		tmp_move_tile = (ClickOnTile)list_tile [nb_tile].GetComponent (typeof(ClickOnTile));
		//		tmp_move_tile.SetGameLogic (this);
		//		tmp_move_tile.SetInfo (default_sprite_tile, 4);
		//		tmp_move_tile.SetIndex (nb_tile);
		//		nb_tile += 1;
		//	}
		//}
		if (size_grid < 7){
			size_grid += 1;
			for (int i = nb_tile; i < size_grid * size_grid; i++) 
				AddTile ();
			tmp_move_tile = null;
			grid.UpdateGrid (size_grid);
		}
	}

	public void GenerateRessource(Sprite[] select_sprite, int color)
	{
		if (nb_tile_used >= nb_tile) {
			time_play = Time.time - time_play;
			Camera.main.gameObject.GetComponent<SoundManager> ().GameOverSound ();
			game_over_pannel.SetActive (true);
			this.gameObject.SetActive (false);
		}
		else{
			int temp;
			int randomIndex;
			list_index = new int[taille_max_row * taille_max_column];

			for (int i = 0; i < nb_tile; i++) {
				list_index [i] = i;
			}
			for (int i = 0; i < nb_tile; i++) {
				temp = list_index[i];
				randomIndex = Random.Range(i, nb_tile);
				list_index[i] = list_index[randomIndex];
				list_index[randomIndex] = temp;
			}
			for (int i = 0; i < list_index.Length; i++)
			{
				if (list_tile [list_index[i]].sprite == default_sprite_tile[0] || list_tile [list_index[i]].sprite == default_sprite_tile[1]){
					if (list_tile [list_index [i]].sprite == default_sprite_tile [1])
						saved_move_tile = null;
					tmp_move_tile = (ClickOnTile)list_tile [list_index[i]].GetComponent(typeof(ClickOnTile));
					tmp_move_tile.SetImage(select_sprite, color);
					nb_tile_used += 1;
					break;
				}
			}
		}
	}

	private void InitTask(Transform current_task, Sprite block, bool action){
		current_task.gameObject.SetActive (action);
		if (action) 
			current_task.GetComponent<Image> ().sprite = block;
	}
		
	private void MultiInitTask(GameObject current_contract, Sprite block, bool action, int calque_model, int taille, int a = 0, int b = 0, int c = 0, int d= 0)
	{
		if (taille > 0)
			InitTask (current_contract.transform.GetChild(calque_model).GetChild(a), block, action);
		if (taille > 1)
			InitTask (current_contract.transform.GetChild(calque_model).GetChild(b), block, action);
		if (taille > 2)
			InitTask (current_contract.transform.GetChild(calque_model).GetChild(c), block, action);
		if (taille > 3)
			InitTask (current_contract.transform.GetChild(calque_model).GetChild(d), block, action);
	}

	public void GenerateTask (GameObject current_contract, int choice_form, Sprite block, int difficulty_contrat, bool action){
		int calque_model = 0;

		if (difficulty_contrat == 1) {
			calque_model = 0;
			current_contract.transform.GetChild (calque_model).gameObject.SetActive (action);
			MultiInitTask (current_contract, block, action, calque_model, 1, 4);
		} else if (difficulty_contrat == 2) {
			if (choice_form == 0) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 2, 2, 5);
			}
			else if (choice_form == 1) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 2, 3, 4);
			}
			current_contract.transform.GetChild (calque_model).gameObject.SetActive (action);
		
		} else if (difficulty_contrat == 3) {
			if (choice_form == 0) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 3, 3, 7, 8);
			}
			else if (choice_form == 1) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 3, 1, 4, 7);
			}
			else if (choice_form == 2) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 3, 3, 4, 5);
			}
			else if (choice_form == 3) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 3, 4, 7, 8);
			}
			else if (choice_form == 4) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 3, 3, 4, 8);
			}
			else if (choice_form == 5) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 3, 3, 4, 7);
			}
			current_contract.transform.GetChild (calque_model).gameObject.SetActive (action);
		} else if (difficulty_contrat == 4) {
			if (choice_form == 0) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 2, 5, 7);
			}
			else if (choice_form == 1) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 1, 3, 6);
			}
			else if (choice_form == 2) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 1, 4, 7);
			}
			else if (choice_form == 3) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 3, 4, 7);
			}
			else if (choice_form == 4) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 3, 4, 6);
			}
			else if (choice_form == 5) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 4, 6, 7);
			}
			else if (choice_form == 6) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 3, 6, 7);
			}
			else if (choice_form == 7) {
				calque_model = 3;
				MultiInitTask (current_contract, block, action, calque_model, 4, 2, 3, 4, 5);
			}
			else if (choice_form == 8) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 2, 3, 6);
			}
			else if (choice_form == 9) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 3, 4, 5, 6);
			}
			else if (choice_form == 10) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 2, 3, 5);
			}
			else if (choice_form == 11) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 2, 4, 5, 6);
			}
			else if (choice_form == 12) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 2, 3, 4);
			}
			else if (choice_form == 13) {
				calque_model = 2;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 4, 5, 6);
			}
			current_contract.transform.GetChild (calque_model).gameObject.SetActive (action);
		} else if (difficulty_contrat == 5) {
			if (choice_form == 0) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 3, 7, 11);
			}
			else if (choice_form == 1) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 4, 8, 10);
			}
			else if (choice_form == 2) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 3, 7, 11);
			}
			else if (choice_form == 3) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 4, 8, 10);
			}
			else if (choice_form == 4) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 3, 7, 10);
			}
			else if (choice_form == 5) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 4, 8, 11);
			}
			else if (choice_form == 6) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 3, 5, 7);
			}
			else if (choice_form == 7) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 4, 1, 2, 3, 6);
			}
			else if (choice_form == 8) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 1, 5, 8);
			}
			else if (choice_form == 9) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 4, 2, 5, 6, 7);
			}
			else if (choice_form == 10) {
				calque_model = 0;
				MultiInitTask (current_contract, block, action, calque_model, 4, 0, 3, 7, 8);
			}
			else if (choice_form == 11) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 5, 6, 7, 8);
			}
			else if (choice_form == 12) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 2, 7, 8, 9);
			}
			else if (choice_form == 13) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 3, 4, 5, 6);
			}
			else if (choice_form == 14) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 2, 3, 4, 9);
			}
			else if (choice_form == 15) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 3, 4, 6, 9);
			}
			else if (choice_form == 16) {
				calque_model = 1;
				MultiInitTask (current_contract, block, action, calque_model, 4, 2, 5, 7, 8);
			}
			current_contract.transform.GetChild (calque_model).gameObject.SetActive (action);
		}
	}


	public void GenerateContrat()
	{
		GameObject current_contract;
		int difficulty_contrat_max;
		int difficulty_contrat;
		int nb_form;
		int color_choice = 0;
		int choice_form = 0;


		if (contract_done >= 55)
			difficulty_contrat_max = 5;
		else
			difficulty_contrat_max = difficulty_by_contract[(int)contract_done / 5];
		for (int i = 0; i < 5; i++) {
			if (difficulty_contrat_max > 1) {
				difficulty_contrat = Random.Range (1, difficulty_contrat_max + 1);
				choice_form = Random.Range (0, form_contract_by_difficulty [difficulty_contrat - 1]);
			}
			else
				difficulty_contrat = difficulty_contrat_max;
			current_contract = parent_contract.GetChild (i).gameObject;
			current_contract.SetActive (true);
			current_contract.GetComponent<Image>().sprite =  list_contrat [difficulty_contrat - 1];
			
			if (nb_current_generateur > 1)
				color_choice = Random.Range (0, nb_current_generateur);
			GenerateTask (current_contract, choice_form, list_contrat_block[color_choice], difficulty_contrat, true);
			goScript = (ClickOnContract)current_contract.GetComponent(typeof(ClickOnContract));
			goScript.SetSprite (list_contrat [difficulty_contrat - 1], list_contrat_light [difficulty_contrat - 1]);
			goScript.SetGameLogic (this);
			goScript.SetMotifFormColor (difficulty_contrat, choice_form, color_choice);
		}
	}

	public void ManageContrat (string manage_action_contrat, ClickOnContract new_contrat){
		if (manage_action_contrat == "ActiveContrat") {
			if (actif_contrat) {
				if (actif_contrat != new_contrat)
					actif_contrat.SwitchImage ();
				new_contrat.SwitchImage ();
			} 
			else 
				new_contrat.SwitchImage ();
			if (actif_contrat != new_contrat)
				actif_contrat = new_contrat;
			else
				actif_contrat = null;
		}
		if (actif_contrat)
			CheckSelectedForm (null, 0, 0, false); 
	}

	public bool SwapTile(ClickOnTile current_tile){
		if (saved_move_tile != null && saved_move_tile != current_tile) {
			tmp_color = current_tile.GetColor ();
			tmp_selected_sprite = current_tile.GetInfo ();
			Camera.main.gameObject.GetComponent<SoundManager> ().MoveBlockSound ();
			current_tile.SetInfo (saved_move_tile.GetInfo (), saved_move_tile.GetColor ());
			saved_move_tile.SetInfo (tmp_selected_sprite, tmp_color);
			saved_move_tile = null;
			nb_move += 1;
			return true;
		} else if (saved_move_tile == current_tile)
			saved_move_tile = null;
		else 
			saved_move_tile = current_tile;
		return false;
	}

	private bool CheckForm(Vector2Int[] tetri, int taille, int[] form, int []color){
		int modificateur;
		bool touch = false;
		Vector2Int tmp_sort;
		int first_case_tetri = -1;

		for (int i = 1; i < taille ; i++) {
			if (tetri [i].x < tetri [i - 1].x) {
				tmp_sort = tetri [i];
				tetri [i] = tetri [i - 1];
				tetri [i - 1] = tmp_sort;
				i = 0;
			}
		}
		while (touch == false) {
			first_case_tetri += 1;
			for (int i = 0; i < taille; i++)
			if ((tetri [i].x - first_case_tetri) % size_grid == 0)
					touch = true;
		}
	first_case_tetri += ((int)tetri [0].x / size_grid) * size_grid;
		for (int j = 0; j < taille; j++) {
			if (form [j] >= max_column_tetri) 
			modificateur = (size_grid - max_column_tetri ) * (int)(form[j] / max_column_tetri);
			else
				modificateur = 0;
			if (tetri [j].x - modificateur - first_case_tetri != form [j] || tetri [j].y != color [j]) 
				break;
			if (j == taille - 1) 
				return true;
		}
		return false;
	}

	private int[] CreatePatern(int taille, int a, int b, int c, int d){
		int[] result;

		result = new int[taille];

		if (taille > 0)
			result [0] = a;
		if (taille > 1)
			result [1] = b;
		if (taille > 2)
			result [2] = c;
		if (taille > 3)
			result [3] = d;
		return result;
	}

	public bool CheckSelectedForm(ClickOnTile current_tile, int index, int color, bool already_see){
		if (already_see && current_tile) {
			for (int i = current_tile.GetNbTileSelected (); i < nb_tile_selected - 1; i++) {
				list_selected_tile [i] = list_selected_tile [i + 1];
				list_selected_tile_object[i] = list_selected_tile_object[i + 1] ;
				list_selected_tile_object [i].SetNbTileSelected (i);
			}
			nb_tile_selected -= 1;
		}
		else if (current_tile != null && color != 4) {
			current_tile.SetNbTileSelected (nb_tile_selected);
			list_selected_tile [nb_tile_selected].x = index;
			list_selected_tile [nb_tile_selected].y = color;
			list_selected_tile_object[nb_tile_selected] = current_tile;
			nb_tile_selected += 1;
		}
		if (actif_contrat) {
			int motif, form, nb_block_asked;
			int[] color_pattern, pattern;

			motif = actif_contrat.GetMotif ();
			form = actif_contrat.GetForm ();
			color = actif_contrat.GetColor ();
			if (motif >= 5)
				nb_block_asked = 4;
			else
				nb_block_asked = actif_contrat.GetMotif ();
			if (nb_tile_selected == nb_block_asked) {
				pattern = new int[nb_block_asked];
				color_pattern = new int[nb_block_asked];
				if (motif == 1) {
					pattern = CreatePatern (1, 0, 0, 0, 0);
				}
				else if (motif == 2) {
					if (form == 0) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, 0, 0);
					else if (form == 1) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 0, 0);
				}
				else if (motif == 3) {
					if (form == 0) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri + 1, 0);
					else if (form == 1) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri * 2, 0);
					else if (form == 2) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 2, 0);
					else if (form == 3) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri, max_column_tetri + 1, 0);
					else if (form == 4) 
						pattern = CreatePatern (nb_block_asked, 0, 1, max_column_tetri + 1, 0);
					else if (form == 5) 
						pattern = CreatePatern (nb_block_asked, 0, 1, max_column_tetri, 0);
				}
				else if (motif == 4) {
					if (form == 0) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri * 2, max_column_tetri * 3);
					else if (form == 1) 
						pattern = CreatePatern (nb_block_asked, 0, 1, max_column_tetri, max_column_tetri * 2);
					else if (form == 2) 
						pattern = CreatePatern (nb_block_asked, 0, 1, max_column_tetri + 1, max_column_tetri * 2 + 1);
					else if (form == 3) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri, max_column_tetri + 1, max_column_tetri * 2 + 1);
					else if (form == 4) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri + 1, max_column_tetri * 2);
					else if (form == 5) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri + 1, max_column_tetri * 2, max_column_tetri * 2 + 1);
					else if (form == 6) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri * 2, max_column_tetri * 2 + 1);
					else if (form == 7) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 2, 3);
					else if (form == 8) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 2, max_column_tetri + 2);
					else if (form == 9) 
						pattern = CreatePatern (nb_block_asked, 2, max_column_tetri, max_column_tetri + 1, max_column_tetri + 2);
					else if (form == 10) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 2, max_column_tetri + 1);
					else if (form == 11) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri, max_column_tetri + 1, max_column_tetri + 2);
					else if (form == 12) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 2, max_column_tetri);
					else if (form == 13) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri + 1, max_column_tetri + 2);
				}else if (motif == 5) {
					if (form == 0) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri, max_column_tetri * 2 , max_column_tetri * 3 + 1);
					else if (form == 1) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri + 1, max_column_tetri * 2 + 1, max_column_tetri * 3);
					else if (form == 2) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri * 2, max_column_tetri * 3 + 1);
					else if (form == 3) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri + 1, max_column_tetri * 2 + 1, max_column_tetri * 3);
					else if (form == 4) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri, max_column_tetri * 2, max_column_tetri * 3);
					else if (form == 5) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri + 1, max_column_tetri * 2 + 1, max_column_tetri * 3 + 1);
					else if (form == 6) 
						pattern = CreatePatern (nb_block_asked, 1, max_column_tetri, max_column_tetri + 2 , max_column_tetri * 2 + 1);
					else if (form == 7) 
						pattern = CreatePatern (nb_block_asked, 1, 2, max_column_tetri, max_column_tetri * 2);
					else if (form == 8) 
						pattern = CreatePatern (nb_block_asked, 0, 1, max_column_tetri + 2, max_column_tetri * 2+ 2);
					else if (form == 9) 
						pattern = CreatePatern (nb_block_asked, 2, max_column_tetri  + 2, max_column_tetri * 2, max_column_tetri * 2 + 1);
					else if (form == 10) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri, max_column_tetri * 2 + 1, max_column_tetri * 2 + 2);
					else if (form == 11) 
						pattern = CreatePatern (nb_block_asked, 3, max_column_tetri, max_column_tetri + 1, max_column_tetri + 2);
					else if (form == 12) 
						pattern = CreatePatern (nb_block_asked, 0, max_column_tetri + 1, max_column_tetri + 2, max_column_tetri + 3);
					else if (form == 13) 
						pattern = CreatePatern (nb_block_asked, 1, 2, 3 , max_column_tetri);
					else if (form == 14) 
						pattern = CreatePatern (nb_block_asked, 0, 1, 2 , max_column_tetri + 3);
					else if (form == 15) 
						pattern = CreatePatern (nb_block_asked, 1, 2, max_column_tetri , max_column_tetri + 3);
					else if (form == 16) 
						pattern = CreatePatern (nb_block_asked, 0, 3, max_column_tetri + 1, max_column_tetri + 2 );
				}
				color_pattern = CreatePatern (nb_block_asked, color, color, color, color);
				if (CheckForm (list_selected_tile, nb_block_asked, pattern, color_pattern)) {
					for (int i = 0; i < nb_tile_selected; i++)
						list_selected_tile_object [i].Reset (default_sprite_tile);
					nb_tile_used -= nb_tile_selected;
					nb_tile_selected = 0;
					GenerateTask(actif_contrat.gameObject,form, null, motif, false);
					actif_contrat.gameObject.SetActive (false);
					Camera.main.gameObject.GetComponent<SoundManager> ().SuccedContratSound ();
					actif_contrat = null;
					Finish_contract ();
					nb_contract_done [motif - 1] += 1;
					return true;
				}
			}
					
		}
		return false;
	}

	public int GetNbMove(){
		return nb_move;
	}

	public float GetTimePlay(){
		return time_play;
	}

	public int[] GetNbContractDone(){
		return nb_contract_done;
	}
}
