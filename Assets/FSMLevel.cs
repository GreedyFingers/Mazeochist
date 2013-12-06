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
	public GameObject enemyObject;
	public GameObject doorWallObject;
	public GameObject endCorridorObject;
	public GameObject enemyGateObject;

	private ArrayList objaRooms = new ArrayList();
	private ArrayList objaWalls = new ArrayList();

	private float fltRoomSize = 12f;
	public int intGridSize;
	
	private enum DIRECTION {UNASSIGED, LEFT, RIGHT, BELOW, ABOVE};
	private enum STATE {SETUP_LEVEL, SETUP_PLAYER,SETUP_ENEMY, PLAYING, PAUSED, GAME_WON, GAME_LOST};	
	private STATE currentState;
	
	private GameObject _startingRoom;
	private GameObject _endingRoom;
	private GameObject _enemyGate;
	private GameObject _endWall;	
	private GameObject _startWall;
	private GameObject _player;
	private GameObject _enemy;	
	
	private int _enemyStartTime;
	private int _enemySpeed;
	
	private int timeFallingDelay = 2;
	
#endregion
	
#region Public Properties	
	
	public GameObject Player
	{
		get {return _player;}
	}
	
	public int EnemyStartTime
	{
		get {return _enemyStartTime;}
		set {_enemyStartTime = value;}
	}
	
	public int EnemySpeed
	{
		get {return _enemySpeed;}
		set {_enemySpeed = value;}
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
				_player.GetComponent<FSMPlayer>().gameWon += player_gameWon;
				_player.GetComponent<FSMPlayer>().playerEnteredNewRoom += emptyEventMethod;
				_player.GetComponent<FSMPlayer>().gameLost += player_gameLost;	
				_player.GetComponent<FSMPlayer>()._endGate = _endWall;
				InsertTorches();						
				currentState = STATE.SETUP_ENEMY;				
				break;
			}
			case(STATE.SETUP_ENEMY):
			{
				if(Time.timeSinceLevelLoad < _enemyStartTime-timeFallingDelay)
					break;
				else
				{
				audio.Play ();

				
					_enemy = (GameObject)Instantiate(enemyObject,
					new Vector3(_enemyGate.transform.position.x,
								_enemyGate.transform.position.y+enemyObject.renderer.bounds.size.y,
								_enemyGate.transform.position.z),
					Quaternion.identity);
					_enemy.GetComponent<FSMEnemy>().timeFallingDelay = timeFallingDelay;				
					_enemy.GetComponent<FSMEnemy>().Rooms = objaRooms;
					_enemy.GetComponent<FSMEnemy>().EndRoom = _endingRoom;
					_enemy.GetComponent<FSMEnemy>().Speed = _enemySpeed;
					_player.GetComponent<FSMPlayer>().playerEnteredNewRoom -= emptyEventMethod;					
					_player.GetComponent<FSMPlayer>().playerEnteredNewRoom += playerEnteredNewRoom;		
					_enemyGate.transform.FindChild("gate").animation.Play();				
					currentState = STATE.PLAYING;	
				}
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
		_startWall = ReplaceWallWithDoorWall(_startingRoom);
		_endingRoom = ChooseEdgeRoom();
		_endingRoom.GetComponent<roomScript>().endRoom = true;
		_endWall = ReplaceWallWithDoorWall(_endingRoom);		
		PlaceEndHallway(_endWall);
		PlaceEnemyGate();
		//begin Recursive Depth-First Search for creating maze from grid of rooms
        VisitNeighbors(_startingRoom);	
		return;
	}

	GameObject ReplaceWallWithDoorWall(GameObject objCurrentRoom)
	{
		Vector3 wallLocation = new Vector3(0,0,0);		
		DIRECTION wallToReplace = ChooseWallToReplace(objCurrentRoom);
		GameObject wallWithDoor;
		wallWithDoor = (GameObject)Instantiate(doorWallObject,new Vector3(0,0,0),Quaternion.identity);		
		switch(wallToReplace)
		{
			case(DIRECTION.LEFT):
				wallLocation = objCurrentRoom.transform.FindChild("leftWall").transform.position;
				wallWithDoor.transform.Rotate(new Vector3(0,180,0));
				break;
			case(DIRECTION.RIGHT):
				wallLocation = objCurrentRoom.transform.FindChild("rightWall").transform.position;
				wallWithDoor.transform.Rotate(new Vector3(0,0,0));			
				break;
			case(DIRECTION.ABOVE):
					wallLocation = objCurrentRoom.transform.FindChild("topWall").transform.position;	
				wallWithDoor.transform.Rotate(new Vector3(0,270,0));			
				break;
			case(DIRECTION.BELOW):
				wallLocation = objCurrentRoom.transform.FindChild("bottomWall").transform.position;	
				wallWithDoor.transform.Rotate(new Vector3(0,90,0));			
				break;		
		}
		removeWall(objCurrentRoom,wallToReplace);
		wallWithDoor.transform.position = wallLocation;
		return wallWithDoor;
	}
	
	void PlaceEndHallway(GameObject wallWithDoor)
	{
		GameObject endHallway = (GameObject)Instantiate(endCorridorObject,wallWithDoor.transform.position,Quaternion.identity);	
		endHallway.transform.rotation = wallWithDoor.transform.rotation;
	}
	
	DIRECTION ChooseWallToReplace(GameObject objCurrentRoom)
	{
		DIRECTION wallToBeReplaced = DIRECTION.UNASSIGED;
		if(objCurrentRoom.GetComponent<roomScript>().leftRoom == true)
			wallToBeReplaced = DIRECTION.LEFT;
		else if(objCurrentRoom.GetComponent<roomScript>().topRoom == true)
			wallToBeReplaced = DIRECTION.ABOVE;
		else if(objCurrentRoom.GetComponent<roomScript>().bottomRoom == true)
			wallToBeReplaced = DIRECTION.BELOW;			
		else if(objCurrentRoom.GetComponent<roomScript>().rightRoom == true)
			wallToBeReplaced = DIRECTION.RIGHT;	
		return wallToBeReplaced;
	}
	
	void PlaceEnemyGate()
	{
		_enemyGate = (GameObject)Instantiate(enemyGateObject,new Vector3(_startingRoom.transform.position.x,
												   _startingRoom.transform.position.y + 
													_startingRoom.transform.FindChild("topWall").renderer.bounds.size.y*.99f,
												   _startingRoom.transform.position.z),Quaternion.identity);
		_enemyGate.transform.Rotate(new Vector3(0,_startWall.transform.rotation.y+270,0));
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
    void VisitNeighbors(GameObject objCurrentRoom)
    {
        int intNeighborCount = objCurrentRoom.GetComponent<roomScript>().objaNeighboringRooms.Count;
		int intUnvisitedCount = intNeighborCount;
        int intRandom;	
        GameObject objNeighbor;
		DIRECTION neighborPosition;
		
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
				neighborPosition = GetNeighborRelativePosition(objCurrentRoom,objNeighbor);					
                removeWall(objCurrentRoom, neighborPosition,objNeighbor);
				objNeighbor.GetComponent<roomScript>().visited = true;	
				//recurse
				VisitNeighbors(objNeighbor);
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
    private void removeWall(GameObject objCurrentRoom, DIRECTION wallLocation, GameObject objNeighbor = null )
    {
		GameObject objCurrentWall;
		bool deleteNeighborWall = true;
			
		if(objNeighbor == null)
			deleteNeighborWall = false;
		
		
		switch(wallLocation)
		{
			case(DIRECTION.LEFT):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("leftWall").gameObject;
				objaWalls.Remove (objCurrentWall);
				Destroy(objCurrentWall);
				if(deleteNeighborWall)
				{
					objCurrentWall = objNeighbor.transform.FindChild("rightWall").gameObject;
					Destroy(objCurrentWall);
					objaWalls.Remove (objCurrentWall);	
				}
				break;
			}
			case(DIRECTION.RIGHT):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("rightWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				if(deleteNeighborWall)
				{
					objCurrentWall = objNeighbor.transform.FindChild("leftWall").gameObject;
					Destroy(objCurrentWall);
					objaWalls.Remove (objCurrentWall);	
				}
				break;
			}
			case(DIRECTION.BELOW):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("bottomWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				if(deleteNeighborWall)
				{
					objCurrentWall = objNeighbor.transform.FindChild("topWall").gameObject;
					Destroy(objCurrentWall);
					objaWalls.Remove (objCurrentWall);	
				};
				break;
			}
			case(DIRECTION.ABOVE):
			{
				objCurrentWall = objCurrentRoom.transform.FindChild("topWall").gameObject;
				objaWalls.Remove (objCurrentWall);			
				Destroy(objCurrentWall);
				if(deleteNeighborWall)
				{
					objCurrentWall = objNeighbor.transform.FindChild("bottomWall").gameObject;
					Destroy(objCurrentWall);
					objaWalls.Remove (objCurrentWall);	
				}
				break;
			}
			default:
				break;			
		}//switch
		if(deleteNeighborWall)
			objCurrentRoom.GetComponent<roomScript>().objaAccessibleNeighbors.Add(objNeighbor);		
		return;
	}
		
	DIRECTION GetNeighborRelativePosition(GameObject objCurrentRoom, GameObject objCurrentNeighbor)
	{
		DIRECTION neighborPosition = DIRECTION.UNASSIGED;
			
        if(objCurrentRoom.transform.position.x < objCurrentNeighbor.transform.position.x)
			neighborPosition = DIRECTION.RIGHT;
		else if(objCurrentRoom.transform.position.x > objCurrentNeighbor.transform.position.x)
			neighborPosition = DIRECTION.LEFT;	
		else if(objCurrentRoom.transform.position.z < objCurrentNeighbor.transform.position.z)
			neighborPosition = DIRECTION.ABOVE;		
		else if(objCurrentRoom.transform.position.z > objCurrentNeighbor.transform.position.z)
			neighborPosition = DIRECTION.BELOW;	
			
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
	
#region Custom Events
	
	public delegate void EventHandler(GameObject e);	
	public event EventHandler gameWon;	
	public event EventHandler gameLost;		
	
#endregion	
	
#region Event Methods
	
	/// <Event handler>
	/// Handles FSMPlayer's "pause" event
	/// </Event handler>
	private void player_gameWon(GameObject sender)
	{
		if(gameWon!= null)
			gameWon(this.gameObject);	
	}
	
	private void emptyEventMethod(GameObject sender){}
	
	private void playerEnteredNewRoom(GameObject sender)
	{
		_enemy.GetComponent<FSMEnemy>().recalculatePath();	
	}	
	
	private void player_gameLost(GameObject sender)
	{
		if(gameWon!= null)		
			gameLost(this.gameObject);				
	}	
#endregion

}