using UnityEngine;
using System.Collections;

public class playerPrefs : MonoBehaviour {
	
	public int _intGridSize;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
