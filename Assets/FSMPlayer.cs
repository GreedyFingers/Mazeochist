using UnityEngine;
using System.Collections;

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
	// Use this for initialization
	void Start () 
	{	
		currentState = STATE.PLAYING;
		this.pause += paused_EnterState;
		playerAI = new playerAIScript(objaRooms);					
	}
	
	// Update is called once per frame
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
