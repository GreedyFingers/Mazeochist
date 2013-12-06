using UnityEngine;
using System.Collections;

public class playerPrefs : MonoBehaviour {
	
	public static int _intGridSize =3;
	public int _enemyStartTime;
	public int _enemySpeed;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
