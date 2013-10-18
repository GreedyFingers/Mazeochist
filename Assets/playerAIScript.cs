using UnityEngine;
using System.Collections;

public class playerAIScript{
	
	private ArrayList _objaRooms = new ArrayList();	
	GameObject objCurrentRoom;	
	
	private Transform target;
	private int speed = 10;
		
	private GameObject[] waypoints;
	private GameObject objLevel;
		
	public Graph graph = new Graph();
	public int currentWP = 0;
	public int startWP = 0;
	GameObject currentNode;
	RaycastHit hit;
	float accuracy = 2f;	
	Vector3 direction;
	int intGridSize;
	
	// Use this for initialization
	public playerAIScript(ArrayList objaRooms)
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
															  objCurrentRoom.transform.position.y+1,
															  objCurrentRoom.transform.position.z+5);
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
