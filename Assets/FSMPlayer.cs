using UnityEngine;
using System.Collections;
using System.IO;
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
	
	private enum NEIGHBOR_RELATIVE_POSITIONS {UNASSIGED, LEFT, RIGHT, BELOW, ABOVE};	
	private enum STATE {PLAYING, PAUSED, WON, LOST};
	private STATE currentState;	
	private AI playerAI;
	private ArrayList objaRooms = new ArrayList();
	private GameObject _nextRoom;
	
	Vector3 direction;	
	private int speed = 60;
    float rotationSpeed = 5;	
	float accuracy = 3;		
	Stack lastStack = new Stack();
	
	private float floatBeginningTime;
	private float floatEndingTime;	
	
	public GameObject _endGate;
	private bool gateIsOpen;
#endregion
	
#region Public Properties
	
	public ArrayList Rooms
	{
		get{return objaRooms;}
		set{objaRooms = value;}
	}		
	
	public GameObject CurrentRoom
	{
		get{return _nextRoom;}
		set{_nextRoom = value;}		
	}	
	
	public float TotalTimeElapsed
	{
		get{return floatEndingTime-floatBeginningTime;}
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
		floatBeginningTime = Time.timeSinceLevelLoad;
		gateIsOpen = false;
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
  				if (Input.GetKeyDown("escape"))		
					paused_EnterState(this.gameObject);
				if(Vector3.Distance(transform.position, _nextRoom.transform.position) < accuracy)		
				{
					GetNextRoom();			
				}
				//movePlayer();						
				break;			
			case(STATE.PAUSED):	
				if(Input.GetKeyDown("escape"))
					paused_ExitState(this.gameObject);
				break;
		}
	}
	

	/// <Event handler>
	/// Handles "pause" event
	/// </Event handler>	
	void paused_EnterState(GameObject sender)
	{	
		this.currentState = STATE.PAUSED;	
		this.GetComponent<InGameMenu>().CurrentWindow = InGameMenu.WINDOW_TYPE.GAME_PAUSED;		
		this.GetComponent<InGameMenu>().enabled = true;		
		this.gameObject.GetComponent<MouseLook>().enabled = false;
		this.gameObject.GetComponent<CharacterMotor>().enabled = false;
		this.gameObject.GetComponent<FPSInputController>().enabled = false;
		Camera.main.GetComponent<MouseLook>().enabled = false;	
		Time.timeScale = 0;		
	}
	
	void lost_EnterState(GameObject sender)
	{
		floatEndingTime = Time.timeSinceLevelLoad;		
		this.currentState = STATE.LOST;		
		this.GetComponent<InGameMenu>().enabled = true;
		this.GetComponent<InGameMenu>().CurrentWindow = InGameMenu.WINDOW_TYPE.GAME_LOST;
		this.gameObject.GetComponent<MouseLook>().enabled = false;
		this.gameObject.GetComponent<CharacterMotor>().enabled = false;
		this.gameObject.GetComponent<FPSInputController>().enabled = false;
		Camera.main.GetComponent<MouseLook>().enabled = false;	
		gameLost(this.gameObject);			
	}
	
	void won_EnterState(GameObject sender)
	{
		floatEndingTime = Time.timeSinceLevelLoad;
		this.currentState = STATE.WON;		
		this.GetComponent<InGameMenu>().CurrentWindow = InGameMenu.WINDOW_TYPE.GAME_WON;		
		this.GetComponent<InGameMenu>().enabled = true;
		this.gameObject.GetComponent<MouseLook>().enabled = false;
		this.gameObject.GetComponent<CharacterMotor>().enabled = false;
		this.gameObject.GetComponent<FPSInputController>().enabled = false;
		Camera.main.GetComponent<MouseLook>().enabled = false;
		gameWon(this.gameObject);
	}	

	void paused_ExitState(GameObject sender)
	{	
		this.currentState = STATE.PLAYING;
		this.GetComponent<InGameMenu>().enabled = false;
		this.gameObject.GetComponent<MouseLook>().enabled = true;
		this.gameObject.GetComponent<CharacterMotor>().enabled = true;
		this.gameObject.GetComponent<FPSInputController>().enabled = true;
		Camera.main.GetComponent<MouseLook>().enabled = true;
		Time.timeScale = 1;		
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
				if(other.GetComponent<roomScript>().endRoom == false)
				{
					playerEnteredNewRoom(this.gameObject);
				}
				else
				{
					if(gateIsOpen == false)
					{
						_endGate.transform.FindChild("Door1").animation.Play();
						_endGate.transform.FindChild("Door2").animation.Play();
						gateIsOpen = true;
					}
				}
				
				break;
			case("endRoom"):
				won_EnterState(this.gameObject);
				break;
			case("enemyCollider"):
				lost_EnterState(this.gameObject);
				break;
		}
		
	}		
	
	void GetNextRoom()
	{		
		foreach(GameObject neighboringRoom in _nextRoom.GetComponent<roomScript>().objaAccessibleNeighbors)
		{
			if(neighboringRoom.GetComponent<roomScript>().playerVisited == false)
			{
				lastStack.Push(_nextRoom);
				_nextRoom = neighboringRoom;
				_nextRoom.GetComponent<roomScript>().playerVisited = true;				
				return;
			}	
		}
		_nextRoom = (GameObject)lastStack.Pop();	
		return;
	}
	
	void movePlayer()
	{
            direction = _nextRoom.transform.position - transform.position;
			direction.y = 0;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction),
										rotationSpeed * Time.deltaTime);	
            transform.position += speed*direction.normalized*Time.deltaTime;
	
	}	

#endregion
	
#region Custom Events
	public delegate void EventHandler(GameObject e);
	
	public event EventHandler gameWon; 
	public event EventHandler playerEnteredNewRoom;
	public event EventHandler gameLost;		
#endregion
}

