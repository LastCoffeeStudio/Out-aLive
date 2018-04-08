using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedWaypoint : MonoBehaviour {

	[SerializeField]
	protected float neighboringRadius = 50.0f;

	[SerializeField]
	protected float gizmoRadius = 1.0f;

	List<AdvancedWaypoint> neighbors;

	public void Start()
	{
		GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Waypoint");

		neighbors = new List<AdvancedWaypoint>();

		for (int i = 0; i < allNodes.Length; i++) 
		{
			AdvancedWaypoint nextNode = allNodes [i].GetComponent<AdvancedWaypoint> ();

			if (nextNode != null) 
			{
				if(Vector3.Distance(this.transform.position, nextNode.transform.position) <= neighboringRadius && nextNode != this){
					neighbors.Add (nextNode);
				}
			}
		}

	}

	public AdvancedWaypoint GetNextNode(AdvancedWaypoint previousNode)
	{
		if (neighbors.Count == 0) 
		{
			Debug.LogError ("Insuficcient waypoints in node " + gameObject.name);
			return null;
		} 
		else if (neighbors.Count == 1 && neighbors.Contains (previousNode)) 
		{
			return previousNode;
		} 
		else 
		{
			AdvancedWaypoint nextNode;
			int nextIndex = 0;

			do 
			{
				nextIndex = UnityEngine.Random.Range(0, neighbors.Count);
				nextNode = neighbors[nextIndex];
			} 
			while(nextNode == previousNode);

			return nextNode;
		}
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, gizmoRadius);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, neighboringRadius);
	}
}
