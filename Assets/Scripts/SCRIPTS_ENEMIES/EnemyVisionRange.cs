using UnityEngine;

public class EnemyVisionRange : MonoBehaviour
{
    EnemyStandardNPC npc;

    private void Start()
    {
        npc = GetComponentInParent<EnemyStandardNPC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npc.targetToChase = other.transform.gameObject;
            npc.isChasingTarget = true;
            Debug.Log(other.ToString() + "IN VISION RANGE. CHASE");
        }
    }
}
