using UnityEngine;
using System.Collections;

public class FSMLevel : MonoBehaviour {

	public GameObject roomObject;
	public GameObject wallObject;

	private ArrayList rooms = new ArrayList();
	private ArrayList wallList = new ArrayList();

	private float roomSize = 9f;
	public int gridSize = 25;

	// Use this for initialization
	void Start () {

	    GameObject currentRoom;
	    GameObject currentWall;
	    GameObject adjacentRoom;
	    GameObject adjacentWall;

        //create grid of rooms
		int currentZ;
		int currentX;
		for (currentX = 0; currentX < gridSize; currentX++)
		{
			for(currentZ = 0; currentZ < gridSize; currentZ++)
			{
                //current location in grid
				float xCoord = currentX*roomSize;
				float zCoord = currentZ*roomSize;
				currentRoom = (GameObject)Instantiate(roomObject,new Vector3(xCoord,0,zCoord),Quaternion.identity);
				rooms.Add(currentRoom);
				currentRoom.GetComponent<roomScript>().visited = false;
                //is room on border?
				if(currentX == 0)
					currentRoom.GetComponent<roomScript>().leftRoom = true;
				if(currentZ == 0)
					currentRoom.GetComponent<roomScript>().bottomRoom = true;
				if(currentX == gridSize-1)
					currentRoom.GetComponent<roomScript>().rightRoom = true;
				if(currentZ == gridSize-1)
					currentRoom.GetComponent<roomScript>().topRoom = true;
			}
		}
		
        //start removing walls
		int random = 0;
		bool wallDestroyed = false;
        //pick room along left wall to be starting point
		random = Random.Range (0,gridSize);
		currentRoom = (GameObject)rooms[random];
		currentRoom = currentRoom.gameObject;
		currentRoom.GetComponent<roomScript>().visited = true;
        //recurse(currentRoom)
		if(currentRoom.GetComponent<roomScript>().topRoom != true)
			wallList.Add(currentRoom.transform.FindChild("topWall").gameObject);
		if(currentRoom.GetComponent<roomScript>().rightRoom != true)
			wallList.Add(currentRoom.transform.FindChild("rightWall").gameObject);
		if(currentRoom.GetComponent<roomScript>().leftRoom != true)
			wallList.Add(currentRoom.transform.FindChild("leftWall").gameObject);
		if(currentRoom.GetComponent<roomScript>().bottomRoom != true)
			wallList.Add(currentRoom.transform.FindChild("bottomWall").gameObject);
		Destroy (currentRoom.transform.FindChild("leftWall").gameObject);
		while(!wallDestroyed)
		{
			random = Random.Range (0,3);
			switch(random)
			{
				case 0:
				{
					currentWall = currentRoom.transform.FindChild("topWall").gameObject;
					if(currentRoom.GetComponent<roomScript>().topRoom == false)
					{						
						wallList.Remove(currentWall);
						Destroy(currentWall);
						wallDestroyed = true;
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)+1];
						adjacentRoom.GetComponent<roomScript>().visited = true;
						adjacentWall = adjacentRoom.transform.FindChild("bottomWall").gameObject;
						Destroy (adjacentWall);
						if(adjacentRoom.GetComponent<roomScript>().topRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("topWall").gameObject);
						if(adjacentRoom.GetComponent<roomScript>().rightRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("rightWall").gameObject);
						if(adjacentRoom.GetComponent<roomScript>().leftRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("leftWall").gameObject);
					}
					break;
				}
				case 1:
				{
					currentWall = currentRoom.transform.FindChild("rightWall").gameObject;
					if(currentRoom.GetComponent<roomScript>().rightRoom == false)
					{
						Destroy(currentWall);
						wallDestroyed = true;
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)+gridSize];
						adjacentRoom.GetComponent<roomScript>().visited = true;				
						adjacentWall = adjacentRoom.transform.FindChild("leftWall").gameObject;
						Destroy (adjacentWall);
						if(adjacentRoom.GetComponent<roomScript>().topRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("topWall").gameObject);
						if(adjacentRoom.GetComponent<roomScript>().rightRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("rightWall").gameObject);
						if(adjacentRoom.GetComponent<roomScript>().bottomRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("bottomWall").gameObject);
					}
					break;
				}
				case 2:
				{
					currentWall = currentRoom.transform.FindChild("bottomWall").gameObject;
					if(currentRoom.GetComponent<roomScript>().bottomRoom == false)
					{
						Destroy(currentWall);
						wallDestroyed = true;
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)-1];
						adjacentRoom.GetComponent<roomScript>().visited = true;
						adjacentWall = adjacentRoom.transform.FindChild("topWall").gameObject;
						Destroy (adjacentWall);		
						if(adjacentRoom.GetComponent<roomScript>().bottomRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("bottomWall").gameObject);
						if(adjacentRoom.GetComponent<roomScript>().rightRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("rightWall").gameObject);
						if(adjacentRoom.GetComponent<roomScript>().leftRoom != true)
							wallList.Add(adjacentRoom.transform.FindChild("leftWall").gameObject);
					}
					break;
				}
				default:{break;}
			}
		}
		
		random = Random.Range (gridSize*(gridSize-1),gridSize*gridSize);
		//Destroy((GameObject)listOfWalls[random]);
		currentRoom = (GameObject)rooms[random];
		Destroy(currentRoom.transform.FindChild("rightWall").gameObject);
		
		while(wallList.Count>0)
		{
				random = Random.Range (0,wallList.Count);
				currentWall = (GameObject)wallList[random];
				currentRoom = currentWall.transform.parent.gameObject;
				switch(currentWall.name)
				{
					case("topWall"):
					{
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)+1];
						if(adjacentRoom.GetComponent<roomScript>().visited == true)
						{
							wallList.Remove(currentWall);
						}
						else
						{
							wallList.Remove(currentWall);
							Destroy(currentWall);	
							adjacentWall = adjacentRoom.transform.FindChild("bottomWall").gameObject;								
							Destroy (adjacentWall);
							wallDestroyed = true;
							if(adjacentRoom.GetComponent<roomScript>().topRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("topWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().rightRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("rightWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().leftRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("leftWall").gameObject);
							adjacentRoom.GetComponent<roomScript>().visited = true;
						}
						break;
					}
					case("rightWall"):
					{
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)+gridSize];
						if(adjacentRoom.GetComponent<roomScript>().visited == true)
						{
							wallList.Remove(currentWall);
						}
						else
						{
							wallList.Remove(currentWall);
							Destroy(currentWall);	
							adjacentWall = adjacentRoom.transform.FindChild("leftWall").gameObject;								
							Destroy (adjacentWall);
							wallDestroyed = true;
							if(adjacentRoom.GetComponent<roomScript>().topRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("topWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().rightRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("rightWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().bottomRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("bottomWall").gameObject);
							adjacentRoom.GetComponent<roomScript>().visited = true;
						}
						break;
					}
					case("bottomWall"):
					{
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)-1];
						if(adjacentRoom.GetComponent<roomScript>().visited == true)
						{
							wallList.Remove(currentWall);
						}
						else
						{
							wallList.Remove(currentWall);
							Destroy(currentWall);	
							adjacentWall = adjacentRoom.transform.FindChild("topWall").gameObject;								
							Destroy (adjacentWall);
							wallDestroyed = true;
							if(adjacentRoom.GetComponent<roomScript>().bottomRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("bottomWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().rightRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("rightWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().leftRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("leftWall").gameObject);
							adjacentRoom.GetComponent<roomScript>().visited = true;
						}
						break;
					}
					case("leftWall"):
					{
						adjacentRoom = (GameObject)rooms[rooms.IndexOf(currentRoom)-gridSize];
						if(adjacentRoom.GetComponent<roomScript>().visited == true)
						{
							wallList.Remove(currentWall);
						}
						else
						{
							wallList.Remove(currentWall);
							Destroy(currentWall);	
							adjacentWall = adjacentRoom.transform.FindChild("rightWall").gameObject;								
							Destroy (adjacentWall);
							wallDestroyed = true;
							if(adjacentRoom.GetComponent<roomScript>().topRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("topWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().bottomRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("bottomWall").gameObject);
							if(adjacentRoom.GetComponent<roomScript>().leftRoom != true)
								wallList.Add(adjacentRoom.transform.FindChild("leftWall").gameObject);
							adjacentRoom.GetComponent<roomScript>().visited = true;
						}
						break;
					}
				}	
		}
	}
	void Update () {
	
	}
}
