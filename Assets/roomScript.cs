using UnityEngine;
using System.Collections;

/// <Basic Description>
/// A script to keep track of various properties for the room object
/// </Basic Description>
/// <Dependencies>
/// FSMLevel
/// </Dependencies>
/// <Interfaces>
/// This class interfaces with FSMLevel to create the necessary logic to create the dungeon and its various
/// components.
/// </Interfaces>
/// <Processes>
/// After this class is created and used to create the dungeon, it does nothing else.
/// </Processes> 
/// <FSM Dependencies>
/// (not an FSM)
/// </FSM Dependencies>
public class roomScript : MonoBehaviour {
	public bool visited;
	public bool topRoom = false;
	public bool leftRoom = false;
	public bool rightRoom = false;
	public bool bottomRoom = false;
	public bool edgeRoom = false;
	public ArrayList objaNeighboringRooms = new ArrayList();
	
	void Start (){}
	void Update () {
	
	}
}
