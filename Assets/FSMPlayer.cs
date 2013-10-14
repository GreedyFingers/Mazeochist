using UnityEngine;
using System.Collections;

public class FSMPlayer : MonoBehaviour {

#region Class Level Attributes	
	
	private enum state {playing, paused, won, lost};
	private state currentState;	
#endregion
	
#region FSM Methods
	// Use this for initialization
	void Start () 
	{
		currentState = state.playing;
		this.pause += paused_EnterState;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(currentState)
		{
			case(state.playing):
				break;
			case(state.paused):	
				break;
		}
	}
	
	void paused_EnterState(GameObject sender)
	{
		this.gameObject.GetComponent<MouseLook>().enabled = false;
		this.gameObject.GetComponent<CharacterMotor>().enabled = false;
		this.gameObject.GetComponent<FPSInputController>().enabled = false;
		Camera.main.GetComponent<MouseLook>().enabled = false;		
	}
	
#endregion
	
#region Public Events

	void OnTriggerEnter(Collider other)
	{
	
		switch(other.name)
		{
		case("objRoom(Clone)"):	
			this.GetComponent<InGameMenu>().enabled = true;
			pause(this.gameObject);
			break;
		}
		
	}		
	
#endregion
	
#region Custom Events
	public delegate void EventHandler(GameObject e);
	
	public event EventHandler pause; 
#endregion
}
