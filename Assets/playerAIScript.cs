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
	float accuracy = 2f;
	Vector3 direction;	
	
	// Use this for initialization
	public playerAIScript(ArrayList objaRooms)
	{
		_objaRooms = objaRooms;
		waypoints = new GameObject[_objaRooms.Count];
		for(int index = 0; index < objaRooms.Count; index++)
		{
			objCurrentRoom = (GameObject)objaRooms[index];
			waypoints[index] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			waypoints[index].renderer.enabled = true;	
        	waypoints[index].transform.position = new Vector3(objCurrentRoom.transform.position.x,
															  objCurrentRoom.transform.position.y+1,
															  objCurrentRoom.transform.position.z+5);
			graph.AddNode(waypoints[index],false,true);	
		}
		 graph.debugDraw();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
