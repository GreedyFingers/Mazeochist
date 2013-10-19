using UnityEngine;
using System.Collections;

public class FSMLevelDepthFirst : MonoBehaviour {

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
	// Use this for initialization
	void Start () 
	{
		currentState = STATE.SETUP_LEVEL;	
	}
		
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
							_startingRoom.transform.position.z+5),
					Quaternion.identity);		
				_player.GetComponent<FSMPlayer>().Rooms = objaRooms;
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
	
	//generate level
	void CreateGrid()
	{		
		InitializeGrid();
		GetAllNeighboringRooms();
        //choose starting room
		_startingRoom = ChooseEdgeRoom();
		_startingRoom.GetComponent<roomScript>().visited = true;
		_endingRoom = ChooseEdgeRoom();
		EndingRoom.collider.enabled = true;
		//begin Recursive Depth-First Search for creating maze from grid of rooms
        VisitNeighbors(NEIGHBOR_RELATIVE_POSITIONS.UNASSIGED, _startingRoom);	
		return;
	}
	
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
	
	//Recursive function to remove walls until all neighbors are visited
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
	
	//remove a wall between two rooms
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
		return neighborPosition;
	}
	
	//see if room is surrounded by visited rooms
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
				objCurrentTorch.transform.localPosition = new Vector3(0,(float)+0.7,(float)0.25);
			}
		}
		
	}
	#endregion
	
#endregion	
	
#region Public Events
	
	
#endregion	
	
#region Event Methods
	private void pause(GameObject sender)
	{
		currentState = STATE.PAUSED;	
	}
#endregion

}