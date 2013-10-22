using UnityEngine;
using System.Collections;
/// <Basic Description>
/// Controls various aspects of the player object
/// </Basic Description>
/// <Dependencies>
/// FSMLevel, playerAIScript
/// </Dependencies>
/// <Interfaces>
/// This player interfaces with this class when he/she pauses or collides with another object
/// </Interfaces>
/// <Processes>
/// Runs concurrently with FSMLevel
/// </Processes> 
/// <FSM Dependencies>
/// Once FSMLevel switches to state "SETUP PLAYER", this class's start method is run.
/// </FSM Dependencies>
public class FSMPlayer : MonoBehaviour {

#region Class Level Attributes	
	
	private enum STATE {PLAYING, PAUSED, WON, LOST};
	private STATE currentState;	
	private playerAIScript playerAI;
	private ArrayList objaRooms = new ArrayList();
#endregion
	
#region Public Properties
	
	public ArrayList Rooms
	{
		get{return objaRooms;}
		set{objaRooms = value;}
	}	
	
#endregion
	
#region FSM Methods

	///Input: (none)
	///Output: (none)
	///Called From: (none)
	///Calls: playerAIScript's constructor	
	void Start () 
	{	
		currentState = STATE.PLAYING;
		this.pause += paused_EnterState;
		playerAI = new playerAIScript(objaRooms);					
	}
	
	///Input: (none)
	///Output: (none)
	///Called From: (none)
	///Calls: Graph::debugDraw()
	void Update () 
	{	
		switch(currentState)
		{
			case(STATE.PLAYING):
			playerAI.graph.debugDraw();				
				break;
			case(STATE.PAUSED):	
				break;
		}
	}

	/// <Event handler>
	/// Handles "pause" event
	/// </Event handler>	
	void paused_EnterState(GameObject sender)
	{		
		this.gameObject.GetComponent<MouseLook>().enabled = false;
		this.gameObject.GetComponent<CharacterMotor>().enabled = false;
		this.gameObject.GetComponent<FPSInputController>().enabled = false;
		Camera.main.GetComponent<MouseLook>().enabled = false;		
	}
	
#endregion
	
#region Public Events

	/// <Event handler>
	/// Handles all of the player object's possible collisions
	/// </Event handler>	
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
