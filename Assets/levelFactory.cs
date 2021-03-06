﻿using UnityEngine;
using System.Collections;

public class levelFactory : MonoBehaviour {
	
	public GameObject levelObect;
	private int _intGridSize;
	private float _timeElapsed;
	private int _enemyStartTime;	
	private int _enemySpeed;
	
	private GameObject currentLevel;
	// Use this for initialization
	void Start () 
	{
	_intGridSize = 	playerPrefs._intGridSize;
    _enemyStartTime = playerPrefs._enemyStartTime;
	_enemySpeed =	 playerPrefs._enemySpeed;	
	
		//	_enemyStartTime = GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._enemyStartTime;
		//_enemySpeed = GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._enemySpeed;
		currentLevel = (GameObject)Instantiate(levelObect,new Vector3(0,0,0),Quaternion.identity);
		currentLevel.GetComponent<FSMLevel>().intGridSize = _intGridSize;
		currentLevel.GetComponent<FSMLevel>().EnemyStartTime = _enemyStartTime;
		currentLevel.GetComponent<FSMLevel>().EnemySpeed = _enemySpeed;		
		//currentLevel.GetComponent<FSMLevel>().gameWon += level_gameWon;	
		//	currentLevel.GetComponent<FSMLevel>().gameLost += level_gameLost;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	private void level_gameWon(GameObject sender)
	{
		_timeElapsed = currentLevel.GetComponent<FSMLevel>().Player.GetComponent<FSMPlayer>().TotalTimeElapsed;							
		if((_enemyStartTime/_timeElapsed)>.25)
		//	GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed--;
		playerPrefs._enemySpeed--;
			
			else
			//GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed++;			
		//GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemyStartTime--;	
		playerPrefs._enemySpeed++;
		playerPrefs._enemyStartTime--;
		
		if(_timeElapsed<30)
			//GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._intGridSize++;
		
				playerPrefs._intGridSize++;

			else
		//	GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._intGridSize--;		
		
				playerPrefs._intGridSize--;

			Application.LoadLevel(Application.loadedLevel);
	}
	
	private void level_gameLost(GameObject sender)
	{
		_timeElapsed = currentLevel.GetComponent<FSMLevel>().Player.GetComponent<FSMPlayer>().TotalTimeElapsed;			
	
		if((_enemyStartTime/_timeElapsed)>.25)
			//GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed--;
			playerPrefs._enemySpeed--;
		else
		
		//	GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed++;			
	//	GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._enemyStartTime++;		
		playerPrefs._enemySpeed++;
		playerPrefs._enemyStartTime++;
		Application.LoadLevel(Application.loadedLevel);
	}	
	
}
