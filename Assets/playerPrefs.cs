using UnityEngine;
using System.Collections;

public class playerPrefs : MonoBehaviour {
	
	public static int _intGridSize=17;
	public static int _enemyStartTime=25;
	public static int _enemySpeed=6;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


	