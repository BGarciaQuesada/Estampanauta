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
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            StartHold();

        if (context.canceled)
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
        if (heldItem == null || currentReceiver == null) return;

        isHolding = true;
        holdTimer = 0f;

        progressBar.Show();
    }

    void CancelHold()
    {
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
}