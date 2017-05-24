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
        transform.position = spawnPoint;
        agent.destination = spawnPoint;
        transform.forward = new Vector3(0, 0, tag == "Human" ? +1 : -1);
    }

    public void Spawn()
    {
    }

    public void StopMovement()
    {
        agent.destination = transform.position;
    }

    public bool IsMoving()
    {
        return agent.velocity.magnitude > 0;
    }
}