using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Esta clase DEBE APLICARSE A PLAYER!!! Maneja el coger y usar objetos al mantener pulsado el botón de interacción.
// Se encarga de iniciar la acción, llamar a la barra de progreso y completar la acción cuando se alcanza el tiempo necesario.

public class PlayerInteraction : MonoBehaviour
{
    private IItem heldItem;                     // Esto se lo tiene que asignar PlayerController (llamando a GrabbableBehavior)
    private IItemReceiver currentReceiver;

    [SerializeField] private float holdDuration = 2f; // TIEMPO GLOBAL DE USO DE COSAS

    private float holdTimer = 0f;
    private bool isHolding = false;

    public UIProgressBar progressBar; // esto se asigna desde el inspector

    //ESTO ES NUEVO

    public GameObject objetoEnMano; // Variable para verificar si el jugador tiene un objeto en la mano
    [Header("Drop")]
    [SerializeField] private Vector3 dropOffset = new Vector3(2f, 0f, 0f);
    private InputAction soltarAction;

    private void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        if(playerInput != null)
        {
            soltarAction = playerInput.actions.FindAction("Soltar", throwIfNotFound: false);
            if(soltarAction != null)
            {
                soltarAction.performed += OnSoltarPerformed;
                soltarAction.Enable();
            }
        }
    }

    // --- TRIGGERS DE ZONAS ---

    private void OnTriggerEnter(Collider other)
    {
        IItemReceiver receiver = other.GetComponent<IItemReceiver>();

        if (receiver != null)
        {
            currentReceiver = receiver;
            Debug.Log("Dentro de zona");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IItemReceiver receiver = other.GetComponent<IItemReceiver>();

        if (receiver != null && receiver == currentReceiver)
        {
            currentReceiver = null;
            Debug.Log("Fuera de zona");
        }
    }

    // --- COGER OBJETO ---
    // Este método solo se ha creado para facilitar el asignar heldItem desde PlayerController
    public void SetHeldItem(GameObject obj)
    {
        heldItem = obj.GetComponent<IItem>();

        if (heldItem == null)
        {
            Debug.LogWarning("El objeto no implementa IItem");
        }
    }
    // --- USAR OBJETO ---
    // [!!!] HACE FALTA METER ESTA ACCIÓN EN EL INPUT SYSTEM!!!!!!
    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            StartHold();
        }

        if (!value.isPressed)
            CancelHold();
    }

    private void Update()
    {

        if (!isHolding) return;

        holdTimer += Time.deltaTime;

        progressBar.SetProgress(holdTimer / holdDuration);

        if (holdTimer >= holdDuration)
        {
            CompleteInteraction();
        }
    }

    void StartHold()
    {
        Debug.Log("Entro en StartHold");
        if (heldItem == null || currentReceiver == null) return;

        isHolding = true;
        holdTimer = 0f;

        progressBar.Show();
    }

    void CancelHold()
    {
        Debug.Log("Entro en CancelHold");
        if (!isHolding) return;

        isHolding = false;
        holdTimer = 0f;

        progressBar.Hide();
    }

    void CompleteInteraction()
    {
        heldItem.UseOn(currentReceiver, gameObject);
        CancelHold();
    }

    //NUEVO
    private void OnSoltarPerformed(InputAction.CallbackContext obj)
    {
        Soltar();

    }
    //private void OnTriggerEnter(Collider other) => TryPick(other.gameObject);

    private void Soltar()
    {
        if (objetoEnMano == null)
            return;
        objetoEnMano.GetComponent<GrabbableBehavior>().DropItem(); //avisamos al objeto que se suelte (para que haga cooldown y no se vuelva a coger inmediatamente)
        //objetoEnMano.transform.SetParent(null);
        //objetoEnMano.transform.position = transform.TransformPoint(dropOffset);
        //objetoEnMano.GetComponent<MeshCollider>().enabled = true; //para que vuelva a colisionar al soltarlo, si es que tenía meshcollider
        //if (objetoEnMano.TryGetComponent<Rigidbody>(out var rb))
        //{
        //    rb.isKinematic = false;
        //}
        objetoEnMano = null;    //pa poder coger mas

    }
}