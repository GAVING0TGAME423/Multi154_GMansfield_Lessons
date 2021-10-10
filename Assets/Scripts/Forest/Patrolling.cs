using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    public List<GameObject> waypoints;
    private NavMeshAgent agent;
    private const float WP_Threshold = 7.0f;
    private GameObject currentwaypoint;
    private int currentwaypointindex = -1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentwaypoint = GetNextWaypoint();
    }

    GameObject GetNextWaypoint()
    {
        currentwaypointindex++;
        if (currentwaypointindex == waypoints.Count)
        {
            currentwaypointindex = 0;
        }
   
        return waypoints[currentwaypointindex];
    }
    
   public void PatrolWaypoints()
    {
        if(Vector3.Distance (transform.position, currentwaypoint.transform.position)< WP_Threshold)
        {
            currentwaypoint = GetNextWaypoint();
            agent.SetDestination(currentwaypoint.transform.position );
        }
        
    }
}
