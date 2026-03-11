using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;    // Velocidad de movimiento

    private CharacterController characterController; // Componente CharacterController
    [SerializeField] private Animator animator; // Componente Animator para controlar animaciones
    [SerializeField] private Vector2 moveInput; // Entrada de movimiento
     
    [SerializeField] private AudioSource pasos; // Sonido de pasos
    [SerializeField] private int minSpeedSound = 1; // Velocidad mínima para reproducir sonido de pasos

    public Rigidbody rb; // Componente Rigidbody para aplicar física (si es necesario)

    [Header("Salto")]
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    private float verticalVelocity; // Velocidad vertical para el salto
    private bool jumpRequested; // Indica si se ha solicitado un salto
    private bool isGrounded; // Indica si el jugador está en el suelo

    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Obtener el componente CharacterController
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody (si es necesario)
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue valor)
    {
        Debug.Log("se pulsa espacio");
        if(valor.isPressed)
        {
            jumpRequested = true; // Marcar que se ha solicitado un salto
        }

    }

    void Update()
    {
        if (characterController == null)
            return;

        ControlMovimiento();
        //ControlSalto();
        SonidoPasos();

        
        
        //rb.linearVelocity += Vector3.up * jumpForce; // Aplicar la velocidad al Rigidbody (si es necesario)
    }

    //private void ControlMovimiento() // Controlar movimiento del jugador
    //{
    //    Vector3 finalVelocity = Vector3.zero;

    //    //// Aplicar knockback si está activo
    //    //if (isKnockbackActive)
    //    //{
    //    //    knockbackTimeRemaining -= Time.deltaTime; // Reducir el tiempo restante del knockback
    //    //    finalVelocity += knockbackVelocity;
    //    //}
    //    //else
    //    //{
    //    // Movimiento local XZ
    //    Vector3 localMove = new Vector3(moveInput.x, 0, moveInput.y);
    //    Vector3 worldMove = transform.TransformDirection(localMove);

    //    if (worldMove.sqrMagnitude > 1f) // Normalizar para evitar velocidad mayor a la deseada
    //        worldMove.Normalize();

    //    finalVelocity += worldMove * moveSpeed; // Velocidad horizontal
    //    //}

    //    characterController.Move(finalVelocity * Time.deltaTime);
    //}

    private void ControlMovimiento()
    {
        isGrounded = characterController.isGrounded;
        //Reset vertical al tocar suelo
        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        //Movimiento local XZ
        Vector3 localMove = new Vector3(moveInput.x, 0, moveInput.y);

        //convertir de local a mundo
        Vector3 worldMove = transform.TransformDirection(localMove);

        if (worldMove.sqrMagnitude > 1f)
            worldMove.Normalize();

        Vector3 horizontalVelocity = worldMove * moveSpeed;
        //Salto
        if (isGrounded && jumpRequested)
        {
            animator.SetTrigger("Saltar");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }


        /////////Salto
        verticalVelocity += gravity * Time.deltaTime;
        //  Vector3 velocity = horizontalVelocity;
        // velocity.y = verticalVelocity;
        horizontalVelocity.y = verticalVelocity;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }

    //private void ControlSalto()
    //{
    //    Vector3 horizontalVelocity = characterController.velocity; // Obtener la velocidad actual del CharacterController
    //    isGrounded = characterController.isGrounded; // Comprobar si el jugador está en el suelo

    //    if (isGrounded && verticalVelocity < 0f)
    //    {
    //        verticalVelocity = -2f;
    //    }

    //    if(isGrounded && jumpRequested)
    //    {
    //        verticalVelocity = Mathf.Sqrt(jumpHeigt * -2f * gravity); // Calcular la velocidad de salto necesaria para alcanzar la altura deseada
    //        jumpRequested = false; // Reiniciar la solicitud de salto
    //    }
    //    verticalVelocity += gravity * Time.deltaTime; // Aplicar gravedad a la velocidad vertical
    //    horizontalVelocity.y = verticalVelocity; // Combinar la velocidad horizontal con la vertical para el movimiento final
    //    characterController.Move(horizontalVelocity * Time.deltaTime);

    //}

    private void SonidoPasos()
    {
        if (pasos == null)
            return;

        Vector3 v = characterController.velocity;
        v.y = 0;

        bool andando = characterController.isGrounded && v.magnitude > minSpeedSound; // Comprobar si el jugador está andando

        if (andando)
        {
            if (!pasos.isPlaying)
                pasos.Play();
        }
        else
        {
            if (pasos.isPlaying)
                pasos.Stop();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Ground")) // Comprobar si el jugador ha tocado el suelo para permitir saltar de nuevo
    //    {
    //        canJump = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground")) // Comprobar si el jugador ha tocado el suelo para permitir saltar de nuevo
    //    {
    //        canJump = false;
    //    }
    //}

    //public void ApplyKnockback(Vector3 knockbackDirection, float knockbackForce, float knockbackDuration)
    //{
    //    // Guardamos la velocidad de empuje
    //    knockbackVelocity = knockbackDirection.normalized * knockbackForce;

    //    // Tiempo que durará el empuje
    //    knockbackTimeRemaining = knockbackDuration;
    //}
}

