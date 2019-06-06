using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		spawnTimer = spawnInterval / downSpeed;
		InitSpawn ();
		enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateGen (Time.fixedDeltaTime);
	}

	void UpdateGen(float deltaTime){
		
	}

	void InitSpawn(){
		
	}

	float ConvertColumnToPos(int x){
		float a = 0;
		x = 0;
		return a;

	}

	public float spawnInterval=1;
	public float spawnTimer;
	public float downSpeed=0.2f;

	public float downSpeedIncrement=0.001f;


	public int lastSizeSpawned=0;
	public int lastColumnSpawned=0;

	public float emptyLaneWeight=0.4f;
	public bool isLastEmpty;

	//Block Weights
	public float BlockWeightShort = 0.2f;
	public float BlockWeightMedium = 0.45f;
	public float BlockWeightLong = 0.35f;
}
