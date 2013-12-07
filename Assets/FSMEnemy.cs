using UnityEngine;
using System.Collections;

public class FSMEnemy : MonoBehaviour {
	
	private enum NEIGHBOR_RELATIVE_POSITIONS {UNASSIGED, LEFT, RIGHT, BELOW, ABOVE};	
	private enum STATE {CREATE_AI, FALLING, PURSUING, PAUSED, WON, LOST};
	private STATE currentState;	
	private AI enemyAI;
	private ArrayList objaRooms = new ArrayList();
	private GameObject _endRoom;	
	public bool buildNextGraph = true;	
	private GameObject _player;
	
	private GameObject _enemyLoc;
	private GameObject _playerLoc;
	private GameObject _endLoc;
	private Vector3 direction;
	private RaycastHit hit;
	
	private float timeSpawned;
	public float timeFallingDelay = 2;
	private int _speed;
	
	public ArrayList Rooms
	{
		get{return objaRooms;}
		set{objaRooms = value;}		
	}	
	public GameObject EndRoom
	{
		get{return _endRoom;}
		set{_endRoom = value;}		
	}	
	public int Speed
	{
		get{return _speed;}
		set{_speed = value;}
	}
	// Use this for initialization
	void Start () 
	{
		currentState = STATE.CREATE_AI;
		_player = GameObject.Find("Player(Clone)");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(currentState)
		{
			case(STATE.CREATE_AI):
				if(objaRooms.Count!=0)
				{
					enemyAI = new AI(objaRooms);	
					enemyAI.speed = _speed;
					_enemyLoc = enemyAI.getClosestWP(this.gameObject);			
					_playerLoc = enemyAI.getClosestWP(_player);					
					enemyAI.graph.AStar(_enemyLoc,_playerLoc);				
					currentState = STATE.FALLING;
					timeSpawned = Time.timeSinceLevelLoad;
				}
				break;
			case(STATE.FALLING):
				if(Time.timeSinceLevelLoad>(timeSpawned+timeFallingDelay))
				{	
					currentState = STATE.PURSUING;
					audio.Play();			
				}
				 break;
			case(STATE.PURSUING):
				direction = _player.transform.position - this.transform.position;			
    			if (Physics.Raycast(this.transform.position, direction,out hit)) 
				{
        			if (hit.transform == _player.transform) 
					{
           				this.rigidbody.AddForce(direction.x*_speed,0,direction.z*_speed);
						if (this.rigidbody.velocity.magnitude > _speed)
    						this.rigidbody.velocity = this.rigidbody.velocity.normalized * _speed;
        			}
					else
						enemyAI.moveAI(this.gameObject);				
				}				
				break;			
			case(STATE.PAUSED):	
				break;
		}
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
}
