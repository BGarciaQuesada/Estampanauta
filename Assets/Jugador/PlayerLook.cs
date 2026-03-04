using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Referencias")] public Transform cameraTransform;

    [Header("Mirar (ratón)")] public float mouseSensitivity = 120f;
    public float minPitch = -40f;
    public float maxPitch = 40f;

    private Vector2 lookInput;
    private float cameraPitch;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float delaySeconds = 2f;

    private Renderer[] renderers; // Array para almacenar los renderers del jugador

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null) // Asignar la cámara principal si no se ha asignado ninguna
            cameraTransform = Camera.main.transform;
        if (playerInput == null) // Obtener el componente PlayerInput si no se ha asignado ninguno
            playerInput = GetComponent<PlayerInput>();
        renderers = GetComponentsInChildren<Renderer>(); // Obtener todos los renderers del jugador y sus hijos

        Ocultar();
    }

    private void Ocultar()
    {
        foreach (var r in renderers) // Desactivar la visibilidad de cada renderer
        {
            r.enabled = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float yaw = transform.eulerAngles.y; // Guardamos el valor actual de yaw (rotación en Y)
        transform.rotation = Quaternion.Euler(0, yaw, 0); // Reseteamos la rotación en X y Z, manteniendo Y
        cameraPitch = 0f; // Reseteamos el pitch de la cámara
        lookInput = Vector2.zero; // Reseteamos la entrada de mirada
        if (cameraTransform != null) // Reseteamos la rotación local de la cámara
            cameraTransform.localRotation = Quaternion.identity;

        StartCoroutine("StartInput");
    }

    IEnumerator StartInput()
    {
        yield return new WaitForSeconds(delaySeconds);

        Mostrar();

        if (playerInput != null)
            playerInput.ActivateInput();
    }

    private void Mostrar()
    {
        foreach (var r in renderers) // Activar la visibilidad de cada renderer
        {
            r.enabled = true;
        }
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform == null)
            return;
        HandleLook();
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime; // Movimiento horizontal del ratón

        transform.Rotate(0f, mouseX, 0f); // Rotamos el jugador en Y (yaw)

        cameraTransform.localRotation = Quaternion.identity; // Reseteamos la rotación local de la cámara
    }
}