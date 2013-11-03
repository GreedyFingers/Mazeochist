using UnityEngine;
using System.Collections;

/// <Basic Description>
/// AI: Moves the player based on waypoints and edges and guides the player to the end of the dungeon
/// by exhaustive search
/// </Basic Description>
/// <Dependencies>
/// (none)
/// </Dependencies>
/// <Interfaces>
/// This class interfaces with the player object and causes the player to move through the dungeon.
/// </Interfaces>
/// <Processes>
/// FSMPlayer's update method calls this class's movement method to make the player move at a fixed rate
/// </Processes> 
/// <FSM Dependencies>
/// (not an FSM)
/// </FSM Dependencies>
public class AI{
	
	private ArrayList _objaRooms = new ArrayList();	
	GameObject objCurrentRoom;
	
	private Transform target;
	private int speed = 8;	
		
	private GameObject[] waypoints;
	private GameObject objLevel;
		
	public Graph graph = new Graph();
	public int currentWP = 0;
	public int startWP = 0;
	GameObject currentNode;
	RaycastHit hit;
	float accuracy = 3;	
	Vector3 direction;
	int intGridSize;
	private float delay = 0;	

	//Input: ArrayList of rooms
	//Output: (none)
	//Called From: (constructor method)
	//Calls: (none)
	// Use this for initialization
	public AI(ArrayList objaRooms)
	{
		_objaRooms = objaRooms;
		waypoints = new GameObject[_objaRooms.Count];
		int intGridSize = (int)Mathf.Sqrt(_objaRooms.Count);
		for(int index = 0; index < objaRooms.Count; index++)
		{
			objCurrentRoom = (GameObject)objaRooms[index];
			waypoints[index] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			waypoints[index].renderer.enabled = true;	
        	waypoints[index].transform.position = new Vector3(objCurrentRoom.transform.position.x,
															  objCurrentRoom.transform.position.y,
															  objCurrentRoom.transform.position.z);
			graph.AddNode(waypoints[index],true,true);	
		}
		for(int i = 0; i < objaRooms.Count-1; i++)
			{			
					direction = waypoints[i+1].transform.position 
								- waypoints[i].transform.position;
    				if (Physics.Raycast(waypoints[i].transform.position, direction,out hit)&&((i+1)%intGridSize!=0)) 
						if(hit.transform.name!="topWall"&&hit.transform.name!="rightWall")		
						{				
							graph.AddEdge(waypoints[i],waypoints[i+1]);
							graph.AddEdge(waypoints[i+1],waypoints[i]);
						}
					if(i>objaRooms.Count-(intGridSize+1))
						continue;
					direction = waypoints[i+intGridSize].transform.position 
								- waypoints[i].transform.position;
    				if (Physics.Raycast(waypoints[i].transform.position, direction,out hit)) 
						if(hit.transform.name!="topWall"&&hit.transform.name!="rightWall")					
						{				
							graph.AddEdge(waypoints[i],waypoints[i+intGridSize]);
							graph.AddEdge(waypoints[i+intGridSize],waypoints[i]);
						}			
				
			}		
	}
	
	public void moveAI(GameObject AIobject)
	{
		
		if (graph.getPathLength() == 0 || currentWP == graph.getPathLength())
        {
            return;
        }

        currentNode = graph.getPathPoint(currentWP);

        // If we are close enough to the current waypoint, start moving toward the next
        if (Vector3.Distance(graph.getPathPoint(currentWP).transform.position, AIobject.transform.position) < accuracy)
        {

			if(currentWP<graph.getPathLength()-1)
            currentWP++;
        }

        // If we are not at the end of the path
        if (currentWP < graph.getPathLength())
        {

            direction = graph.getPathPoint(currentWP).transform.position - AIobject.transform.position;
            AIobject.rigidbody.AddForce(direction.x*9,0,direction.z*9);
			if (AIobject.rigidbody.velocity.magnitude > speed)
    			AIobject.rigidbody.velocity = AIobject.rigidbody.velocity.normalized * speed;
        }
		
	}
	
	public GameObject getClosestWP(GameObject character)
	{
		int temp = 0;
		for(int wp = 1; wp < waypoints.Length; wp++)
		{
			if((waypoints[wp].transform.position - character.transform.position).magnitude <
					(waypoints[temp].transform.position - character.transform.position).magnitude)
					temp = wp;
		}
		return waypoints[temp];
	}	
	public GameObject getCurrentWP()
	{
		return graph.getPathPoint(startWP);	
	}
	
}
