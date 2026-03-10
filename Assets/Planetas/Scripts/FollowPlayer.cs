using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Range(.1f,1f)]
    public float followDamping; //Cuanto más bajo, más rápido sigue al jugador. Cuanto más alto, más lento lo hace.
    public Transform playerTransform;
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, 1/followDamping * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, 1/followDamping * Time.fixedDeltaTime);
    }
}
