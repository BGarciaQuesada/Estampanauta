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
    public float secondsPausePatrol = 1.5F;

    private int currentPatrolIndex = 0;
    private bool doWait = false;
    private Coroutine patrolWait;

    [Header("CHASING")]
    public GameObject targetToChase;
    public float chaseStopDistance = 1F;
    public float chasingSpeed = 4.5F;

    [SerializeField] private bool _isChasingTarget = false;
    public GameObject hitFXPrefab;

    public bool isChasingTarget { get => _isChasingTarget; set => _isChasingTarget = value; }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChasingTarget)
            Chase(targetToChase);
        else if (!doWait)
            Patrol();
    }

    void Patrol()
    {
        if (destinationList.Length <= 0) return;

        Transform target = destinationList[currentPatrolIndex];

        MoveTo(target.position, speed, patrolStopDistance);

        if (Vector3.Distance(transform.position, target.position) < patrolStopDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % destinationList.Length;
            
            if (patrolWait != null)
            {
                StopCoroutine(DoWait(secondsPausePatrol));
                patrolWait = null;
            }
            patrolWait = StartCoroutine(DoWait(secondsPausePatrol));
        }
    }
    private IEnumerator DoWait(float seconds)
    {
        doWait = true;
        yield return new WaitForSecondsRealtime(seconds);
        doWait = false;
    }

    void Chase(GameObject target)
    {
        if (patrolWait != null)
        {
            Debug.Log("Stop patrol and Chasing");
            StopCoroutine(patrolWait);
            patrolWait = null;
        }

        MoveTo(target.transform.position, chasingSpeed, chaseStopDistance);
    }


    void MoveTo(Vector3 position, float speed, float distanceToStop)
    {
        if (Vector3.Distance(transform.position, position) < distanceToStop) return;

        Vector3 dirGlobal = position - transform.position;
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

    //void MoveToTarget(Vector3 position, float speed, float distanceToStop)
    //{
    //    if (Vector3.Distance(transform.position, destinationList[currentPatrolIndex].position) < distanceToStop) return;

    //    Vector3 dirGlobal = position - transform.position;
    //    Vector3 dirLocal = transform.InverseTransformDirection(dirGlobal);
    //    Vector3 movimiento = new Vector3(dirLocal.x, 0, dirLocal.z).normalized;

    //    if (movimiento != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(transform.TransformDirection(movimiento), transform.up);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    //    }

    //    Vector3 velocidadVertical = Vector3.Project(rb.linearVelocity, transform.up);
    //    rb.linearVelocity = transform.TransformDirection(movimiento) * speed + velocidadVertical;

    //    // dirLocal.x y dirLocal.z ya son relativos al NPC
    //}





    //public Player player;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        //player.GetDamage()

        Destroy(Instantiate(hitFXPrefab, gameObject.transform.position, Quaternion.identity),.7F);
    }

}
