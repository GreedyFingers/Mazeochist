using UnityEngine;
using System.Collections;

/// <Basic Description>
/// FSM Level: Creates dungeon and keeps a handle on all objects created within it
/// </Basic Description>
/// <Dependencies>
/// Player, Enemy, Items and all other objects are dependent on this class for creation
/// </Dependencies>
/// <Interfaces>
/// This class interfaces with the classes that make up the game world by directing communication between them.
/// </Interfaces>
/// <Processes>
/// FSMLevel, FSMPlayer and InGameMenu all operate simultaneously via their "Update" methods.
/// FSMLevel first builds all of the gameobjects for the game, then it builds the player which begins
/// the FSMPlayer's Start and Update processes. It then does nothing while it's in its "Playing" state,
/// and simply waits, handling occasional events.
/// FSMPlayer begins by creating the InGameMenu class, and then waits to handle collision events.
/// InGameMenu creates a GUI and waits until the player has won or lost or paused the game to display it.
/// </Processes> 
/// <FSM Dependencies>
/// FSMPlayer enters its "PLAYING" state as a result of FSMLevel entering its "SETUP_PLAYER" state
/// When FSMPlayer enters its "PAUSED" state, every other game script should enter its respective "PAUSED" state
/// </FSM Dependencies>
public class FSMLevel : MonoBehaviour {

#region Class-Level Attributes
	
	public GameObject columnObject;
	public GameObject roomObject;
	public GameObject wallObject;
	public GameObject playerObject;	
	public GameObject torchObject;

	private ArrayList objaRooms = new ArrayList();
	private ArrayList objaWalls = new ArrayList();

	private float fltRoomSize = 12f;
	public int intGridSize = 5;
	
	private enum NEIGHBOR_RELATIVE_POSITIONS {UNASSIGED, LEFT, RIGHT, BELOW, ABOVE};
	private enum STATE {SETUP_LEVEL, SETUP_PLAYER, PLAYING, PAUSED, GAME_WON, GAME_LOST};	
	private STATE currentState;
	
	private GameObject _startingRoom;
	private GameObject _endingRoom;
	private GameObject _player;
	
#endregion
	
#region Public Properties	
	
	public GameObject StartingRoom
	{
		get {return _startingRoom;}
		set {_startingRoom = value;}
	}
	private GameObject EndingRoom
	{
		get {return _endingRoom;}
		set {_endingRoom = value;}
	}
	
#endregion
	
#region FSM Methods
	///Input: (none)
	///Output: (none)
	///Called From: Called upon instantiation of the object
	///Calls: (none)
	void Start () 
	{
		currentState = STATE.SETUP_LEVEL;	
	}

	///Input: (none)
	///Output: (none)
	///Called From: repeatedly called throughout the course of the game
	///Calls: (none)
	void Update () 
	{
		switch(currentState)
		{
			case(STATE.SETUP_LEVEL):
			{
				CreateGrid();		
				currentState = STATE.SETUP_PLAYER;		
				break;
			}	
			case(STATE.SETUP_PLAYER):
			{
				//create player			
				_player = (GameObject)Instantiate(playerObject,
					new Vector3(_startingRoom.transform.position.x,
							_startingRoom.transform.position.y+1,
							_startingRoom.transform.position.z),
					Quaternion.identity);
				_player.GetComponent<FSMPlayer>().CurrentRoom = _startingRoom;
				_player.GetComponent<FSMPlayer>().LastRoom = _startingRoom;
				_player.GetComponent<FSMPlayer>().StartRoom = _startingRoom;			
				_player.GetComponent<FSMPlayer>().pause += pause;
				InsertTorches();			
				currentState = STATE.PLAYING;				
				break;
			}
			case(STATE.PLAYING):
				break;
			case(STATE.PAUSED):
			{
				break;
			}
		}//switch
	}//Update
	
#endregion
	
#region Class Methods
	
	#region Grid Creation
	
	///Input: (none)
	///Output: (none)
	///Called From: Update()
	///Calls: InitializeGrid(), GetAllNeighboringRooms(),ChooseEdgeRoom(),VisitNeighbors()
	///Description: Creates grid and chooses a starting and ending room in which to place player,
	///then calls recursive function to remove walls and carve out the actual maze
	void CreateGrid()
	{		
		InitializeGrid();
		GetAllNeighboringRooms();
        //choose starting room
		_startingRoom = ChooseEdgeRoom();
		_startingRoom.GetComponent<roomScript>().visited = true;
		_endingRoom = ChooseEdgeRoom();
		_endingRoom.collider.enabled = true;
		//begin Recursive Depth-First Search for creating maze from grid of rooms
        VisitNeighbors(NEIGHBOR_RELATIVE_POSITIONS.UNASSIGED, _startingRoom);	
		return;
	}
	
	///Input: (none)
	///Output: (none)
	///Called From: CreateGrid()
	///Calls: (none)
	void InitializeGrid()
	{
	    GameObject objCurrentRoom;

        //create grid of rooms
		int intCurrentZ;
		int intCurrentX;
		for (intCurrentX = 0; intCurrentX < intGridSize; intCurrentX++)
		{
			for(intCurrentZ = 0; intCurrentZ < intGridSize; intCurrentZ++)
			{
                //current location in grid
				float fltXCoord = intCurrentX*fltRoomSize;
				float fltZCoord = intCurrentZ*fltRoomSize;
				objCurrentRoom = (GameObject)Instantiate(roomObject,new Vector3(fltXCoord,0,fltZCoord),Quaternion.identity);
				objaRooms.Add(objCurrentRoom);
				objaWalls.Add(objCurrentRoom.transform.FindChild("leftWall").gameObject);
				objaWalls.Add(objCurrentRoom.transform.FindChild("rightWall").gameObject);
				objaWalls.Add(objCurrentRoom.transform.FindChild("topWall").gameObject);
				objaWalls.Add(objCurrentRoom.transform.FindChild("bottomWall").gameObject);
				objCurrentRoom.GetComponent<roomScript>().visited = false;
                //is room on border?
				if(intCurrentX == 0)
				{
					objCurrentRoom.GetComponent<roomScript>().leftRoom = true;
					objCurrentRoom.GetComponent<roomScript>().edgeRoom = true;	
				}
				if(intCurrentZ == 0)
				{
					objCurrentRoom.GetComponent<roomScript>().bottomRoom = true;
					objCurrentRoom.GetComponent<roomScript>().edgeRoom = true;	
				}
				if(intCurrentX == intGridSize-1)
				{
					objCurrentRoom.GetComponent<roomScript>().rightRoom = true;
					objCurrentRoom.GetComponent<roomScript>().edgeRoom = true;	
				}
				if(intCurrentZ == intGridSize-1)
				{
					objCurrentRoom.GetComponent<roomScript>().topRoom = true;
					objCurrentRoom.GetComponent<roomScript>().edgeRoom = true;	
				}
			}//for
		}//for
	}

	///Input: (none)
	///Output: (none)
	///Called From: CreateGrid()
	///Calls: (none)
	///Description: Gets all surrounding rooms and add them to current room's array of neighbors
	void GetAllNeighboringRooms()
	{	
	    GameObject objCurrentRoom;
		
		for (int intRow = 0; intRow < intGridSize; intRow++)
		{
			for(int intColumn = 0; intColumn < intGridSize; intColumn++)
			{
				int intCurrentIndex = (intRow*intGridSize)+intColumn;
				objCurrentRoom = (GameObject)objaRooms[intCurrentIndex];
                //add neighboring rooms to this room's adjacent rooms arraylist
				if(objCurrentRoom.GetComponent<roomScript>().topRoom != true)
					objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Add(objaRooms[intCurrentIndex+1]);
				if(objCurrentRoom.GetComponent<roomScript>().bottomRoom != true)
					objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Add(objaRooms[intCurrentIndex-1]);							
				if(objCurrentRoom.GetComponent<roomScript>().leftRoom != true)
					objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Add(objaRooms[intCurrentIndex-intGridSize]);									
				if(objCurrentRoom.GetComponent<roomScript>().rightRoom != true)
					objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Add(objaRooms[intCurrentIndex+intGridSize]);											
			}//for
		}//for		
	}

	///Input: (none)
	///Output: objRoom
	///Called From: CreateGrid()
	///Calls: (none)
	///Description: returns a random edge room from the grid
	GameObject ChooseEdgeRoom()
	{
		GameObject objCurrentRoom;
		int intRandom;
		do
		{
			intRandom = Random.Range (0,objaRooms.Count);
			objCurrentRoom = (GameObject)objaRooms[intRandom];
			objCurrentRoom = objCurrentRoom.gameObject;		
		} while (objCurrentRoom.GetComponent<roomScript>().edgeRoom == false);
							
		return objCurrentRoom;
	}
		
	///Input:(none)
	///Output:(none)
	///Called From: CreateGrid()
	///Calls: (itself), removeWall()
	///Recursive function to remove walls until all neighbors are visited	
    void VisitNeighbors(NEIGHBOR_RELATIVE_POSITIONS lastRelativePosition, GameObject objCurrentRoom)
    {
        int intNeighborCount = objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Count;
		int intUnvisitedCount = intNeighborCount;
        int intRandom;	
        GameObject objNeighbor;
		NEIGHBOR_RELATIVE_POSITIONS thisRelativePosition = NEIGHBOR_RELATIVE_POSITIONS.UNASSIGED;
		
		//are all neighbors visited yet?
		intUnvisitedCount = CheckIfNeighborsAreVisited(objCurrentRoom,intNeighborCount);
        while (intUnvisitedCount != 0)
        {
            intRandom = Random.Range(0, objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Count);
            objNeighbor = (GameObject)objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms[intRandom];
            if (objNeighbor.GetComponent<roomScript>().visited == true)				
                continue;
            else
            {
                thisRelativePosition = removeWall(objCurrentRoom, objNeighbor);
				objNeighbor.GetComponent<roomScript>().visited = true;	
				//recurse
				VisitNeighbors(thisRelativePosition,objNeighbor);
				intUnvisitedCount = CheckIfNeighborsAreVisited(objCurrentRoom,intNeighborCount);
            }
        }
		return;
    }
	
	///Input: current room, neighbor of current room
	///Output: relative position of neighbor to current room
	///Called From: VisitNeighbors()
	///Calls: (none)
	///remove a wall between two rooms	
    NEIGHBOR_RELATIVE_POSITIONS removeWall(GameObject objCurrentRoom, GameObject objNeighbor)
    {
		GameObject objCurrentWall;
		
		NEIGHBOR_RELATIVE_POSITIONS neighborPosition = NEIGHBOR_RELATIVE_POSITIONS.UNASSIGED;
        if(objCurrentRoom.transform.position.x < objNeighbor.transform.position.x)
			neighborPosition = NEIGHBOR_RELATIVE_POSITIONS.RIGHT;
		else if(objCurrentRoom.transform.position.x > objNeighbor.transform.position.x)
			neighborPosition = NEIGHBOR_RELATIVE_POSITIONS.LEFT;	
		else if(objCurrentRoom.transform.position.z < objNeighbor.transform.position.z)
			neighborPosition = NEIGHBOR_RELATIVE_POSITIONS.ABOVE;		
		else if(objCurrentRoom.transform.position.z > objNeighbor.transform.position.z)
			neighborPosition = NEIGHBOR_RELATIVE_POSITIONS.BELOW;		
		
		switch(neighborPosition)
		{
			case(NEIGHBOR_RELATIVE_POSITIONS.LEFT):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("leftWall").gameObject;
				objaWalls.Remove (objCurrentWall);
				Destroy(objCurrentWall);
				objCurrentWall = objNeighbor.transform.FindChild("rightWall").gameObject;
				Destroy(objCurrentWall);
				objaWalls.Remove (objCurrentWall);	
				break;
			}
			case(NEIGHBOR_RELATIVE_POSITIONS.RIGHT):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("rightWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				objCurrentWall = objNeighbor.transform.FindChild("leftWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				break;
			}
			case(NEIGHBOR_RELATIVE_POSITIONS.BELOW):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("bottomWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				objCurrentWall = objNeighbor.transform.FindChild("topWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				break;
			}
			case(NEIGHBOR_RELATIVE_POSITIONS.ABOVE):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("topWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				objCurrentWall = objNeighbor.transform.FindChild("bottomWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				break;
			}
			default:
				break;			
		}//switch
		objCurrentRoom.GetComponent<roomScript>().objaAccessibleNeighbors.Add(objNeighbor);		
		return neighborPosition;
	}
	
	///Input: current room, and also number of neighbors
	///Output: number of unvisited rooms.
	///Called From: VisitNeighbors()
	///Calls: (none)
	///see if room is surrounded by visited rooms	
	int CheckIfNeighborsAreVisited(GameObject objCurrentRoom,int intNeighborCount)
	{
		int intUnvisitedCount = intNeighborCount;
		GameObject objNeighbor;
        for (int i = 0; i < intNeighborCount; i++)
		{
			objNeighbor = (GameObject)objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms[i];
            if (objNeighbor.GetComponent<roomScript>().visited == true)
                intUnvisitedCount--;
		}		
		return intUnvisitedCount;
	}
	
	#endregion
	
	#region Create Static Models
	///Input: (none)
	///Output: (none)
	///Called From: CreateGrid()
	///Calls: (none)
	///Description: Goes through all remaining walls, and places torches on them with a 25 percent chance each.	
	void InsertTorches()
	{
		GameObject objCurrentTorch;
		int intRandom;
		foreach(GameObject wall in objaWalls)
		{
			intRandom = Random.Range(0, 4);
			if(intRandom==0)
			{
				objCurrentTorch = (GameObject)Instantiate(torchObject,new Vector3(0,0,0),Quaternion.identity);
				objCurrentTorch.transform.localEulerAngles = wall.transform.localEulerAngles;
				objCurrentTorch.transform.parent = wall.transform;
				objCurrentTorch.transform.localPosition = new Vector3(0,(float)+0.7,(float)-0.15);
			}
		}
		
	}
	#endregion
	
#endregion	
	
#region Public Events
	
	
#endregion	
	
#region Event Methods
	
	/// <Event handler>
	/// Handles FSMPlayer's "pause" event
	/// </Event handler>
	private void pause(GameObject sender)
	{
		currentState = STATE.PAUSED;	
	}
#endregion

}