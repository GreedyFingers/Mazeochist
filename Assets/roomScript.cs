using UnityEngine;
using System.Collections;

public class roomScript : MonoBehaviour {
	public bool visited;
	public bool topRoom = false;
	public bool leftRoom = false;
	public bool rightRoom = false;
	public bool bottomRoom = false;
	public ArrayList objaNeighboringRooms = new ArrayList();
	
	void Start (){}
	// Update is called once per frame
	void Update () {
	
	}
}
