using System.Collections;
using UnityEngine;

public class EnemyStandardNPC : MonoBehaviour
{
    private Rigidbody rb;

    [Header("NORMAL ATTRIBUTES")]
    public int hitpoints = 3;
    public float speed = 2.5F;
    public float rotationSpeed = 5.0F;

    [Header("PATROLLING")]
    public Transform[] destinationList;
    public float patrolStopDistance = 1F;
    public float secondsPauseInBetween = 1.5F;

    private int currentPatrolIndex = 0;
    private bool doWait = false;
    private Coroutine patrolWait;

    [Header("CHASING")]
    public Transform targetToChase;
    public float chaseStopDistance = 1F;
    private float chasingSpeed = 4.5F;

    [SerializeField] private bool isChasingTarget = false;
    [SerializeField] private bool isInVisionRange = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChasingTarget)
            Chase(destinationList[currentPatrolIndex]);
        else if (!doWait)
            Patrol();
    }

    void Patrol()
    {
        if (destinationList.Length <= 0) return;

        Transform target = destinationList[currentPatrolIndex];

        MoveTo(target.position, patrolStopDistance);

        if (Vector3.Distance(transform.position, target.position) < patrolStopDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % destinationList.Length;
            doWait = true;
            patrolWait = StartCoroutine(DoPatrolWait());
        }
    }
    private IEnumerator DoPatrolWait()
    {
        yield return new WaitForSecondsRealtime(secondsPauseInBetween);
        doWait = false;
    }

    void Chase(Transform target)
    {
        if (patrolWait != null)
        {
            StopCoroutine(patrolWait);
            patrolWait = null;
        }
        if (Vector3.Distance(transform.position, target.position) < chaseStopDistance)
            isChasingTarget = false;

        MoveTo(target.position, chasingSpeed);
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

    public void SetChasing(bool chasing)
    {
        isChasingTarget = chasing;
    }

}
