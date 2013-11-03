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
		this.pause += paused_EnterState;
		floatBeginningTime = Time.timeSinceLevelLoad;
		//playerAI = new playerAIScript(objaRooms);					
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
				if(Vector3.Distance(transform.position, _nextRoom.transform.position) < accuracy)		
				{
					GetNextRoom();			
				}
				movePlayer();							
				break;			
			case(STATE.PAUSED):	
				break;
			case(STATE.WON):		
				floatEndingTime = Time.timeSinceLevelLoad;	
				gameWon(this.gameObject);
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
				if(other.GetComponent<roomScript>().endRoom == false)
				{
					playerEnteredNewRoom(this.gameObject);
					break;
				}
				this.GetComponent<InGameMenu>().enabled = true;
				pause(this.gameObject);
				this.currentState = STATE.WON;
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
            //transform.position.x += speed*direction.normalized.x*Time.deltaTime;		
            //transform.position.z += speed*direction.normalized.z*Time.deltaTime;		
	}	

#endregion
	
#region Custom Events
	public delegate void EventHandler(GameObject e);
	
	public event EventHandler pause; 
	public event EventHandler playerEnteredNewRoom;
	public event EventHandler gameWon;
#endregion
}
