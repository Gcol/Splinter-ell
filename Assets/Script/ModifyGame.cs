using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyGame : MonoBehaviour {

	public GridManager other;
	public GameLogicManager GameLogic;
	public int row_count = 2;
	public int column_count = 2;
	public int value = 1;
	public string action_button = "None";

	private bool need_add_column = false;
	private bool change = false;


	public void IncreaseStock()
	{
		//Output this to console when the Button is clicked
	
		if (need_add_column == true && column_count < 7) {
			column_count += value;
			change = true;
		} else if (row_count < 7) {
			row_count += value;
			change = true;
		}
		if (change == true) 
			GameLogic.UpdateGrid ();
		need_add_column = !need_add_column;
	}

	void IncreaseContractDone()
	{
		GameLogic.Finish_contract (value);
	}

	// Use this for initialization
	void Start () {
		Button btn1 = this.GetComponent<Button>();

		//Calls the TaskOnClick/TaskWithParameters method when you click the Button
		if (action_button == "IncreaseStock")
			btn1.onClick.AddListener(IncreaseStock);
		if (action_button == "IncreaseContractDone")
			btn1.onClick.AddListener(IncreaseContractDone);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
