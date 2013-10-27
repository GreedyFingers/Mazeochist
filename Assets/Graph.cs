using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <Basic Description>
/// Graph: a graph of waypoints (nodes) and edges between waypoints referenced by an object to determine pathfinding
/// </Basic Description>
/// <Dependencies>
/// Node and Edge
/// </Dependencies>
/// <Interfaces>
/// This class interfaces with the playerAIScript in such a way that playerAIScript uses this class to determine
/// where the player object needs to go.
/// </Interfaces>
/// <Processes>
/// This class is used each time playerAIScript needs to create a new path from a point A to a point B
/// </Processes> 
/// <FSM Dependencies>
/// (not an FSM)
/// </FSM Dependencies>
public class Graph
{
	List<Edge>	edges = new List<Edge>();
	List<Node>	nodes = new List<Node>();
	List<Node>  pathList = new List<Node>();
	
	public Graph(){}

	///Input: 
	///-object whose transform will be the location of the waypoint,
	///-bool to remove the object's renderer,
	///-bool to remove its collider
	///Output: (none)
	///Called From: playerAIScript
	///Calls: (none)
	///Description: adds a node to the graph of waypoints
	public void AddNode(GameObject id, bool removeRenderer, bool removeCollider)
	{
		Node node = new Node(id);
		nodes.Add(node);
		
		//remove colliders and mesh renderer
		if(removeCollider)
			GameObject.Destroy(id.collider);
		if(removeRenderer)
			GameObject.Destroy(id.renderer);
	}

	///Input:  -Node from which the beginning of the edge will be
	///Output: -Node from which the end of the edge will be
	///Called From: playerAIScript
	///Calls: (none)	
	///Description: creates a unidirectional edge between waypoints
	public void AddEdge(GameObject fromNode, GameObject toNode)
	{
		Node from = findNode(fromNode);
		Node to = findNode(toNode);
		
		if(from != null && to != null)
		{
			Edge e = new Edge(from, to);
			edges.Add(e);
			from.edgelist.Add(e);
		}	
	}

	///Input:  ID of gameobject
	///Output: Node in graph
	///Called From: AStar()
	///Calls: (none)	
	///Description: Gets node in graph by gameobject
	Node findNode(GameObject id)
	{
		foreach (Node n in nodes) 
		{
			if(n.getId() == id)
				return n;
		}
		return null;
	}
	
	///Input:  (none)
	///Output: Number of nodes in path
	///Called From: AStar()
	///Calls: (none)	
	///Description: Gets length of path
	public int getPathLength()
	{
		return pathList.Count;	
	}

	///(not used)
	public GameObject getPathPoint(int index)
	{
		return pathList[index].id;
	}

	///(not used)
	public void printPath()
	{
		foreach(Node n in pathList)
		{	
			Debug.Log(n.id.name);	
		}
	}

	///Input:  
	///-Node of graph which is the starting point for the moving gameobject
	///-Node of graph which is the destination for the moving gameobject
	///Called From: playerAIScript
	///Calls: findNode(), getPathLength()	
	///Description: Uses the A* algorithm to find the shortest path between to waypoints
	public bool AStar(GameObject startId, GameObject endId)
	{
	  	Node start = findNode(startId);
	  	Node end = findNode(endId);
	  
	  	if(start == null || end == null)
	  	{
	  		return false;	
	  	}
	  	
	  	List<Node>	open = new List<Node>();
	  	List<Node>	closed = new List<Node>();
	  	float tentative_g_score= 0;
	  	bool tentative_is_better;
	  	
	  	start.g = 0;
		start.h = distance(start,end);
	  	start.f = start.h;
	  	open.Add(start);
	  	
	  	while(open.Count > 0)
	  	{
	  		int i = lowestF(open);
			Node thisnode = open[i];
			if(thisnode.id == endId)  //path found
			{
				reconstructPath(start,end);
				return true;	
			} 	
			
			open.RemoveAt(i);
			closed.Add(thisnode);
			
			Node neighbour;
			foreach(Edge e in thisnode.edgelist)
			{
				neighbour = e.endNode;
				neighbour.g = thisnode.g + distance(thisnode,neighbour);
				
				if (closed.IndexOf(neighbour) > -1)
					continue;
				
				tentative_g_score = thisnode.g + distance(thisnode, neighbour);
				
				if( open.IndexOf(neighbour) == -1 )
				{
					open.Add(neighbour);
					tentative_is_better = true;	
				}
				else if (tentative_g_score < neighbour.g)
				{
					tentative_is_better = true;	
				}
				else
					tentative_is_better = false;
					
				if(tentative_is_better)
				{
					neighbour.cameFrom = thisnode;
					neighbour.g = tentative_g_score;
					neighbour.h = distance(thisnode,neighbour);
					neighbour.f = neighbour.g + neighbour.h;	
				}
			}
  	
	  	}
		
		return false;	
	}

	///Input: -start node
	///-end node
	///Output:(none)
	///Called From: AStar()
	public void reconstructPath(Node startId, Node endId)
	{
		pathList.Clear();
		pathList.Add(endId);
		
		var p = endId.cameFrom;
		while(p != startId && p != null)
		{
			pathList.Insert(0,p);
			p = p.cameFrom;	
		}
		pathList.Insert(0,startId);
	}

	///Input: -beginning node
	///-ending node
	///Output: distance between nodes
	///Called From: AStar()
	///Calls: (none)
    float distance(Node a, Node b)
    {
	  float dx = a.xPos - b.xPos;
	  float dy = a.yPos - b.yPos;
	  float dz = a.zPos - b.zPos;
	  float dist = dx*dx + dy*dy + dz*dz;
	  return( dist );
    }
	
	//Input: List of nodes
	//Output: Lowest "f" value in current list of nodes (refer to AStar algorithm for explanation of "f" and "g")
	//Called From: AStar()
	//Calls:	(none)
    int lowestF(List<Node> l)
    {
	  float lowestf = 0;
	  int count = 0;
	  int iteratorCount = 0;
	  	  
	  for (int i = 0; i < l.Count; i++)
	  {
	  	if(i == 0)
	  	{	
	  		lowestf = l[i].f;
	  		iteratorCount = count;
	  	}
	  	else if( l[i].f <= lowestf )
	  	{
	  		lowestf = l[i].f;
	  		iteratorCount = count;	
	  	}
	  	count++;
	  }
	  return iteratorCount;
    }

	//Input: List of nodes
	//Output: Lowest "g" value in current list of nodes (refer to AStar algorithm for explanation of "f" and "g")
	//Called From: AStar()
	//Calls: (none)	
	int lowestG(List<Node> l)
    {
	  float lowestg = 0;
	  int count = 0;
	  int iteratorCount = 0;
	  	  
	  for (int i = 0; i < l.Count; i++)
	  {
	  	if(i == 0)
	  	{	
	  		lowestg = l[i].g;
	  		iteratorCount = count;
	  	}
	  	else if( l[i].g <= lowestg )
	  	{
	  		lowestg = l[i].g;
	  		iteratorCount = count;	
	  	}
	  	count++;
	  }
	  return iteratorCount;
    }
 
	//Input: (none)
	//Output: (none)
	//Called From: playerAIScript()
	//Calls: (none)
	//Description: paints waypoints and their edges onto the screen in "scene" view for debugging purposes.
    public void debugDraw()
    {
      	//draw edges
    	for (int i = 0; i < edges.Count; i++)
	  	{			
    		Debug.DrawLine(edges[i].startNode.id.transform.position, edges[i].endNode.id.transform.position, Color.red,10);

	  	}	
    }
	
}