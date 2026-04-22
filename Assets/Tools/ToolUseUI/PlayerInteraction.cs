using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Esta clase DEBE APLICARSE A PLAYER!!! Maneja el coger y usar objetos al mantener pulsado el botón de interacción.
// Se encarga de iniciar la acción, llamar a la barra de progreso y completar la acción cuando se alcanza el tiempo necesario.

public class PlayerInteraction : MonoBehaviour
{
    private IItem heldItem;                     // Esto se lo tiene que asignar PlayerController (llamando a GrabbableBehavior)
    private IItemReceiver currentReceiver;
    private IItemReceiver lockedReceiver;

    [SerializeField] private float holdDuration = 2f; // TIEMPO GLOBAL DE USO DE COSAS

    private float holdTimer = 0f;
    private bool isHolding = false;

    public UIProgressBar progressBar; // esto se asigna desde el inspector

    //ESTO ES NUEVO
    public AudioSource itemsSFX;
    public AudioClip sonidoRecogeObjeto;
    public AudioClip sonidoRecogeLiquido;
    public AudioClip sueltaObjeto;
    public AudioClip sonidoPicar;

    public GameObject objetoEnMano; // Variable para verificar si el jugador tiene un objeto en la mano
    [Header("Drop")]
    [SerializeField] private Vector3 dropOffset = new Vector3(2f, 0f, 0f);
    private InputAction soltarAction;

    private void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            soltarAction = playerInput.actions.FindAction("Soltar", throwIfNotFound: false);
            if (soltarAction != null)
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

        //esto es pq si el player sale de la zona mientras recarga la barra de interacción da error
        StartCoroutine(SaleDeZona(receiver));
    }
    IEnumerator SaleDeZona(IItemReceiver receiver)
    {
        yield return new WaitForSeconds(.1F); // Espera medio segundo para evitar problemas de colisiones rápidas
        if (receiver != null && receiver == currentReceiver)
        {
            currentReceiver = null;
            Debug.Log("Fuera de zona");

            // Si está fuera de zona, automáticamente cancelar la interacción en curso
            CancelHold();
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

        if (heldItem == null || lockedReceiver == null)
        {
            CancelHold();
            return;
        }

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
        if (isHolding) return;

        lockedReceiver = currentReceiver; // Bloqueamos el receptor actual para evitar que cambie mientras se mantiene la interacción
        // (Esto hacia que se ejecutaba el hold en la nave y luego se desplazaba, se bugueaba porque el item se usaba en el vacío en vez de en la nave)

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

        lockedReceiver = null; // limpiar

        progressBar.Hide();
        progressBar.SetProgress(0f);
    }

    void CompleteInteraction()
    {
        Debug.Log("entro");

        if (lockedReceiver == null)
        {
            Debug.LogWarning("No hay receiver al completar interacción");
            CancelHold();
            return;
        }

        // USAR ANTES DE LIMPIAR
        heldItem.UseOn(lockedReceiver, this.gameObject);

        // sonidos
        if (objetoEnMano != null)
        {
            if (objetoEnMano.GetComponent<Bucket>() != null)
                itemsSFX.PlayOneShot(sonidoRecogeLiquido);
            else if (objetoEnMano.GetComponent<Tool>() != null)
                itemsSFX.PlayOneShot(sonidoPicar);
        }

        CancelHold();

        Debug.Log("salgo");
    }

    //NUEVO
    private void OnSoltarPerformed(InputAction.CallbackContext obj)
    {
        Soltar();

    }
    private void Soltar()
    {
        if (objetoEnMano == null)
            return;
        itemsSFX.PlayOneShot(sueltaObjeto);
        objetoEnMano.GetComponent<GrabbableBehavior>().DropItem(GetComponent<PlayerController>().currentPlanet); //avisamos al objeto que se suelte (para que haga cooldown y no se vuelva a coger inmediatamente)
        objetoEnMano = null;    //pa poder coger mas
        heldItem = null; // Limpiar el item que se tiene en la mano al soltarlo

        // Si el jugador suelta el objeto mientras está en una zona de interacción, cancelar la interacción en curso
        CancelHold();
    }
}