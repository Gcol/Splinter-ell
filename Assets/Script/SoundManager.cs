using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public	AudioClip block_spawn_audio;
	public	AudioClip block_move_audio;
	public	AudioClip[] button_audio;
	public	AudioClip sucess_one_contract_audio;
	public	AudioClip sucess_contract_audio;
	public	AudioClip game_over_audio;
	public	AudioClip game_complete_audio;
	public	AudioClip stamp_audio;

	float	spawn_block_time;
	float	move_block_time;
	float	press_button_time;
	float	sucess_one_contract_time;
	float	sucess_contract_time;
	float	game_over_time;
	float	game_complete_time;
	float	stamp_time;

	public void SpawnBlockSound()
	{

		if (Time.fixedTime > spawn_block_time + 0.1f) {
			MakeSound (block_spawn_audio);
			spawn_block_time = Time.fixedTime;
		}
	}

	public  void MoveBlockSound()
	{
		if (Time.fixedTime > move_block_time + 0.1f) {
			MakeSound(block_move_audio);
			move_block_time = Time.fixedTime;
		}
	}

	public void PressButtonSound(){
		if (Time.fixedTime > press_button_time + 0.1f) {
			MakeSound(button_audio [Random.Range (0, 2)]);
			press_button_time = Time.fixedTime;
		}
	}


	public void SuccedContratSound(){
		if (Time.fixedTime > sucess_one_contract_time + 0.1f) {
			MakeSound(sucess_one_contract_audio);
			sucess_one_contract_time = Time.fixedTime;
		}
	}

	public void SuceedRowContractSound(){
		if (Time.fixedTime > sucess_contract_time + 0.1f) {
			MakeSound(sucess_contract_audio);
			sucess_contract_time = Time.fixedTime;
		}
	}

	public void GameOverSound(){
		if (Time.fixedTime > game_over_time + 0.1f) {
			MakeSound(game_over_audio);
			game_over_time = Time.fixedTime;
		}
	}

	public void GameCompleteSound(){
		if (Time.fixedTime > game_complete_time + 0.1f) {
			MakeSound(game_complete_audio);
			game_complete_time = Time.fixedTime;
		}
	}

	public void StampSound(){
		if (Time.fixedTime > stamp_time + 0.1f) {
			MakeSound(stamp_audio);
			stamp_time = Time.fixedTime;
		}
	}

	private void MakeSound(AudioClip son)
	{
		AudioSource.PlayClipAtPoint(son, GetComponent<Camera>().transform.position);
		GetComponent<AudioSource>().clip = son;
		GetComponent<AudioSource>().Play();

	}
}
