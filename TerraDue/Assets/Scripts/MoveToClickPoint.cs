using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class MoveToClickPoint : NetworkBehaviour {

	NavMeshAgent agent;
	public Transform spawnPoint;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
    {
        if (!isLocalPlayer || !GetComponent<PlayerBehaviour>().IsAlive())
        {
            return;
        }
		if (Input.GetMouseButtonDown(0))
        {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
				agent.destination = hit.point;
			}
		}
	}

    public void MoveToSpawnPoint()
    {
        transform.position = spawnPoint.position;
        agent.enabled = true;
        agent.destination = spawnPoint.position;
    }

    public void MoveToHiddenPoint()
    {
        agent.enabled = false;
        transform.position = new Vector3(0f, -1000f, 0f);
    }
}