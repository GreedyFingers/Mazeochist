using UnityEngine;
using System.Collections;

public class FSMEnemy : MonoBehaviour {
	
	private enum NEIGHBOR_RELATIVE_POSITIONS {UNASSIGED, LEFT, RIGHT, BELOW, ABOVE};	
	private enum STATE {CREATE_GRAPH, PLAYING, PAUSED, WON, LOST};
	private STATE currentState;	
	private AI enemyAI;
	private ArrayList objaRooms = new ArrayList();
	private GameObject _endRoom;
	private GameObject _startRoom;
	private GameObject _lastRoom;	
	private GameObject _currentRoom;
	public bool buildNextGraph = true;	
	private GameObject _player;
	
	private GameObject _enemyLoc;
	private GameObject _playerLoc;
	private GameObject _endLoc;
	private Vector3 direction;
	private RaycastHit hit;
	
	public ArrayList Rooms
	{
		get{return objaRooms;}
		set{objaRooms = value;}		
	}	
	public GameObject StartRoom
	{
		get{return _startRoom;}
		set{_startRoom = value;}		
	}
	public GameObject EndRoom
	{
		get{return _endRoom;}
		set{_endRoom = value;}		
	}	
	// Use this for initialization
	void Start () {
		currentState = STATE.CREATE_GRAPH;
		this.pause += paused_EnterState;
		_player = GameObject.Find("Player(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState)
		{
			case(STATE.CREATE_GRAPH):
				if(objaRooms.Count!=0)
				{
					enemyAI = new AI(objaRooms);			
					currentState = STATE.PLAYING;
				}
				break;
			case(STATE.PLAYING):
				if(buildNextGraph == true)
				{
					_enemyLoc = enemyAI.getClosestWP(this.gameObject);			
					_endLoc = enemyAI.getClosestWP(_player);					
					enemyAI.graph.AStar(_enemyLoc,_endLoc);
					buildNextGraph = false;
				}
				enemyAI.moveAI(this.gameObject);				
				break;			
			case(STATE.PAUSED):	
				break;
		}
	}
		
	void paused_EnterState(GameObject sender)
	{				
	}		
	
	public void recalculatePath()
	{
		direction = _player.transform.position - this.transform.position;
    	if (Physics.Raycast(this.transform.position, direction,out hit)) 
		{
        	if (hit.transform == _player) 
        		return;
		}
		_enemyLoc = enemyAI.getCurrentWP();			
		_playerLoc = enemyAI.getClosestWP(_player);			
		enemyAI.graph.AStar(_enemyLoc,_playerLoc);		
	}
		
#region Custom Events
	public delegate void EventHandler(GameObject e);
	
	public event EventHandler pause; 
#endregion		
}
