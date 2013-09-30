using UnityEngine;
using System.Collections;

public class FSMPlayer : MonoBehaviour {
	
	private enum state {playing, won, lost};
	private state currentState;	
	
	private GameObject levelHandle;
	
	// Use this for initialization
	void Start () 
	{
		currentState = state.playing;
		levelHandle = GameObject.Find("objLevel");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(currentState)
		{
			case(state.playing):
				break;
		}
	}
}
