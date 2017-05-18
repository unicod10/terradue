using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class MoveToPoint : NetworkBehaviour {

	NavMeshAgent agent;
	public Transform spawnPoint;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}

    public void MoveTo(Vector3 point)
    {
        if (!GetComponent<PlayerBehaviour>().IsAlive())
        {
            return;
        }
        agent.destination = point;
    }

    public void Spawn()
    {
        transform.position = spawnPoint.position;
        agent.enabled = true;
        agent.destination = spawnPoint.position;
    }

    public void Hide()
    {
        agent.enabled = false;
        transform.position = new Vector3(0f, -1000f, 0f);
    }

    public void StopMovement()
    {
        agent.destination = transform.position;
    }
}