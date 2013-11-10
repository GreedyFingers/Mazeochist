using UnityEngine;
using System.Collections;

public class levelFactory : MonoBehaviour {
	
	public GameObject levelObect;
	private int _intGridSize;
	private float _timeElapsed;
	
	private GameObject currentLevel;
	// Use this for initialization
	void Start () 
	{
		_intGridSize = GameObject.Find ("playerPrefs").GetComponent<playerPrefs>()._intGridSize;		
		currentLevel = (GameObject)Instantiate(levelObect,new Vector3(0,0,0),Quaternion.identity);
		currentLevel.GetComponent<FSMLevel>().intGridSize = _intGridSize;
		currentLevel.GetComponent<FSMLevel>().gameWon += level_gameWon;	
		currentLevel.GetComponent<FSMLevel>().gameLost += level_gameWon;
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
          		    sw.WriteLine(_intGridSize);
          		}					
        }				
		print (_timeElapsed);
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
          		    sw.WriteLine("Lost" + "," + _intGridSize);
          		}					
        }				
		print (_timeElapsed);		
		Application.LoadLevel(Application.loadedLevel);
	}	
	
}
