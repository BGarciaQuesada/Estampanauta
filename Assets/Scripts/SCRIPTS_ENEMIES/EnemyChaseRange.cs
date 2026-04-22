using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseRange : MonoBehaviour
{
    EnemyStandardNPC npc;

    private void Start()
    {
        npc = GetComponentInParent<EnemyStandardNPC>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && npc.isChasingTarget)
        {
            npc.targetToChase = null;
            npc.isChasingTarget = false;
            Debug.Log(other.ToString() + " OUT OF CHASING RANGE. BACK TO PATROL.");
        }
    }
}
