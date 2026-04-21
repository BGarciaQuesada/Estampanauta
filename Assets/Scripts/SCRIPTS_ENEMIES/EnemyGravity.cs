using UnityEngine;
using UnityEngine.AI;

public class EnemyGravity : MonoBehaviour
{

    //public GameObject[] planets;
    public GameObject planet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = planet.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(transform.up, -Physics.gravity) * transform.rotation;
    }
}
