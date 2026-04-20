using UnityEngine;

public class EnemyStandardNPC : MonoBehaviour
{
    private Rigidbody rb;
    private bool isChasingTarget = false;

    [Header("NORMAL ATTRIBUTES")]
    public int hitpoints = 3;
    public float speed = 5.0F;
    public float rotationSpeed = 5.0F;
    public float stopDistance = 1.5F;

    [Header("PATROLLING")]
    public Transform[] destinationList;
    public float patrolStopDistance = 1F;
    private int currentPatrolIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChasingTarget)
            MoveTo(destinationList[currentPatrolIndex].position, stopDistance);
        else Patrol();
    }

    void Patrol()
    {
        if (destinationList.Length <= 0) return;

        Transform target = destinationList[currentPatrolIndex];

        MoveTo(target.position, patrolStopDistance);

        if (Vector3.Distance(transform.position, target.position) < patrolStopDistance)
            currentPatrolIndex = (currentPatrolIndex + 1) % destinationList.Length;
    }

    void MoveTo(Vector3 position, float distanceToStop)
    {
        if (destinationList == null || destinationList.Length <= 0) return;
        if (Vector3.Distance(transform.position, destinationList[currentPatrolIndex].position) < distanceToStop) return;

        Vector3 dirGlobal = destinationList[currentPatrolIndex].position - transform.position;
        Vector3 dirLocal = transform.InverseTransformDirection(dirGlobal);
        Vector3 movimiento = new Vector3(dirLocal.x, 0, dirLocal.z).normalized;

        if (movimiento != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.TransformDirection(movimiento), transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        Vector3 velocidadVertical = Vector3.Project(rb.linearVelocity, transform.up);
        rb.linearVelocity = transform.TransformDirection(movimiento) * speed + velocidadVertical;

        // dirLocal.x y dirLocal.z ya son relativos al NPC
    }
}
