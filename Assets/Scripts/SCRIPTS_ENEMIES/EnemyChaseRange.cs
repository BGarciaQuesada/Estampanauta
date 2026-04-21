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
        if (other.CompareTag("Player"))
        {
            npc.targetToChase = null;
            npc.SetChasing(false);
            Debug.Log(other.ToString() + " OUT OF CHASING RANGE. BACK TO PATROL.");
        }
    }
}
