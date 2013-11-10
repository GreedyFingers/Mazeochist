using UnityEngine;
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
		_intGridSize = GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._intGridSize;
		_enemyStartTime = GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._enemyStartTime;
		_enemySpeed = GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._enemySpeed;
		currentLevel = (GameObject)Instantiate(levelObect,new Vector3(0,0,0),Quaternion.identity);
		currentLevel.GetComponent<FSMLevel>().intGridSize = _intGridSize;
		currentLevel.GetComponent<FSMLevel>().EnemyStartTime = _enemyStartTime;
		currentLevel.GetComponent<FSMLevel>().EnemySpeed = _enemySpeed;		
		currentLevel.GetComponent<FSMLevel>().gameWon += level_gameWon;	
		currentLevel.GetComponent<FSMLevel>().gameLost += level_gameLost;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	private void level_gameWon(GameObject sender)
	{
		_timeElapsed = currentLevel.GetComponent<FSMLevel>().Player.GetComponent<FSMPlayer>().TotalTimeElapsed;			
        if (System.IO.File.Exists("C:/Users/Squee/Documents/stats.csv")) 
        {
          		// Create a file to write to. 
          		using (System.IO.StreamWriter sw = System.IO.File.AppendText("C:/Users/Squee/Documents/stats.csv")) 
          		{
          		    sw.WriteLine("Won" + "," + _enemyStartTime + "," + _enemySpeed + "," + _intGridSize);
          		}					
        }				
		if((_enemyStartTime/_timeElapsed)>.25)
			GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed--;
		else
			GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed++;			
		GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemyStartTime--;	
		if(_timeElapsed<50)
			GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._intGridSize++;
		else
			GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._intGridSize--;			
		Application.LoadLevel(Application.loadedLevel);
	}
	
	private void level_gameLost(GameObject sender)
	{
		_timeElapsed = currentLevel.GetComponent<FSMLevel>().Player.GetComponent<FSMPlayer>().TotalTimeElapsed;			
        if (System.IO.File.Exists("C:/Users/Squee/Documents/stats.csv")) 
        {
          		// Create a file to write to. 
          		using (System.IO.StreamWriter sw = System.IO.File.AppendText("C:/Users/Squee/Documents/stats.csv")) 
          		{
          		    sw.WriteLine("Won" + "," + _enemyStartTime + "," + _enemySpeed + "," + _intGridSize);
          		}					
        }		
		if((_enemyStartTime/_timeElapsed)>.25)
			GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed--;
		else
			GameObject.Find("playerPrefs").GetComponent<playerPrefs>()._enemySpeed++;			
		GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._enemyStartTime++;		
		Application.LoadLevel(Application.loadedLevel);
	}	
	
}
