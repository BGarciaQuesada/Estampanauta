using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rayCastLength = 2f;
    public float rotationSpeed = 5f;
    private float tmpRotationSpeed;

    public float speed = 5f;
    public float gravity = -20f;
    private float tmpGravity;

    public float jumpForce = 10f;

    private Rigidbody rb;
    public Transform currentPlanet;
    public Transform playerVisual;

    RaycastHit[] hits;
    Vector3 normalVector;
    Vector3 input;

    public bool isTouchingPlanetSurface = false;
    private Transform MainCameraTransform;
    public Transform CameraArmTransform;

    bool CanJump = true;
    bool slowDown = false;

    private Animator animator;

    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        MainCameraTransform = Camera.main.transform;

        tmpGravity = gravity;
        tmpRotationSpeed = rotationSpeed;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // SALTO CON ESPACIO
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(canMove)
            Movement();
        ApplyGravity();
        ApplyPlanetRotation();
    }

    void Jump()
    {
        if (!CanJump || !isTouchingPlanetSurface) return;

        animator.SetTrigger("Jump");
        animator.SetBool("OnAir", true);

        // Eliminamos velocidad vertical
        rb.linearVelocity -= Vector3.Project(rb.linearVelocity, normalVector);

        // Impulso hacia arriba respecto al planeta
        rb.AddForce(normalVector * jumpForce, ForceMode.Impulse);

        gravity = tmpGravity / 2f;
        Invoke(nameof(RestoreGravity), 1f);

        CanJump = false;
        rotationSpeed = tmpRotationSpeed / 2f;
    }

    void RestoreGravity()
    {
        gravity = tmpGravity;
        CanJump = true;
        slowDown = false;
    }

    void Movement()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Vector3 cameraRotation = new Vector3(0, MainCameraTransform.eulerAngles.y + CameraArmTransform.eulerAngles.y, 0);
        //Vector3 Dir = Quaternion.Euler(cameraRotation) * input;

        //Vector3 movement_dir = (transform.forward * Dir.z + transform.right * Dir.x);

        Vector3 camForward = Vector3.ProjectOnPlane(MainCameraTransform.forward, normalVector).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(MainCameraTransform.right, normalVector).normalized;

        Vector3 Dir = camForward * input.z + camRight * input.x;

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, normalVector).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, normalVector).normalized;

        Vector3 movement_dir = Dir.normalized;

        // Separamos velocidad vertical (gravedad) de la horizontal
        Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, normalVector);
        Vector3 horizontalVelocity = movement_dir * speed;

        rb.linearVelocity = verticalVelocity + horizontalVelocity;
        if (horizontalVelocity != Vector3.zero)
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 0);
            animator.SetBool("run", true);
        }
        else
            animator.SetBool("run", false);
        //NO FUNIONA ESTO
        if(horizontalVelocity.x == 0 && horizontalVelocity.z != 0)
        {
            if(horizontalVelocity.z > 0)
                transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            else if (horizontalVelocity.z < 0)
                transform.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
        }


            Debug.Log(horizontalVelocity);
        if (movement_dir != Vector3.zero)
        {
            playerVisual.rotation = Quaternion.LookRotation(movement_dir, normalVector);
        }

        if (slowDown)
            rb.linearVelocity *= .5f;
    }

    void ApplyGravity()
    {
        if (currentPlanet == null) return;

        normalVector = (transform.position - currentPlanet.position).normalized;

        rb.AddForce(-normalVector * Mathf.Abs(gravity), ForceMode.Acceleration);
    }

    void ApplyPlanetRotation()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normalVector) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        if (isTouchingPlanetSurface && CanJump)
            rotationSpeed = tmpRotationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == currentPlanet)
        {
            isTouchingPlanetSurface = true;
            animator.SetBool("OnAir", false);
            CanJump = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == currentPlanet)
        {
            isTouchingPlanetSurface = false;
        }
    }

    public void EnterNewGravityField()
    {
        gravity = tmpGravity / 4f;
        rb.linearVelocity *= .5f;
        rotationSpeed = tmpRotationSpeed / 10f;
        slowDown = true;
        CanJump = false;
        Invoke(nameof(RestoreGravity), .5f);
    }
}