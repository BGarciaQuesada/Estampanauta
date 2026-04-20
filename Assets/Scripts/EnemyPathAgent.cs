using UnityEngine;
using UnityEngine.AI;

public class EnemyPathAgent : MonoBehaviour
{

    private NavMeshAgent nmAgent;
    public Transform destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        nmAgent.updateUpAxis = false;
        nmAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        nmAgent.SetDestination(destination.position);
    }
}
