using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class MoveToPoint : NetworkBehaviour {

	NavMeshAgent agent;
    public Vector3 spawnPoint;

    void Start() {
		agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
	}

    public void MoveTo(Vector3 point)
    {
        if (!GetComponent<PlayerBehaviour>().IsAlive())
        {
            return;
        }
        agent.destination = point;
    }

    public void Hide()
    {
        agent.enabled = false;
        transform.position = new Vector3(0, -1000);
    }

    public void Spawn()
    {
        transform.position = spawnPoint;
        agent.enabled = true;
        agent.destination = spawnPoint;
    }

    public void StopMovement()
    {
        agent.destination = transform.position;
    }
}