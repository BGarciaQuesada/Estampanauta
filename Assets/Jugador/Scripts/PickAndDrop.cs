using UnityEngine;
using UnityEngine.InputSystem;

public class PickAndDrop : MonoBehaviour
{
    [Header("Mochila")]
    [SerializeField] private Transform mochila;

    [Header("Input System")]
    [Tooltip("Opcional: arrastra aquí la accion 'Soltar' (InputActionReference). Si lo dejas vacio, se busca por nonmbre en PlayerInput")]
    [SerializeField] private InputActionReference soltarActionRef;

    [SerializeField] private string soltarActionName;

    [Header("Drop")]
    [SerializeField] private Vector3 dropOffset = new Vector3(2f,0f,0f);

    private GameObject objetoEnMochila;

    private InputAction soltarAction;

    [Header("Soltar de mano")]
    [SerializeField] private GameObject mano;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //1 si el usuario asigna un inputactionreference lo usamos
        if(soltarActionRef != null)
        {
            //guardamos la accion de soltar directamente desde la referencia
            soltarAction = soltarActionRef.action;
        } else
        {
            //2 si no hay referencia, buscamos la accion por nombre en el mcomponente playerinput del player 
            var playerInput = GetComponent<PlayerInput>();
            if(playerInput != null)
            {
                soltarAction = playerInput.actions.FindAction(soltarActionName, throwIfNotFound:false);
            }
        }
    }

    private void OnEnable()
    {
        if(soltarAction != null)
        {
            soltarAction.performed += OnSoltarPerformed;
            soltarAction.Enable();
        } 
    }

    private void OnSoltarPerformed(InputAction.CallbackContext obj)
    {
        if(mano != null)
        {
            SoltarMano();
        } else
        {
            Soltar();
        }
    }
    //private void OnTriggerEnter(Collider other) => TryPick(other.gameObject);

    private void Soltar()
    {
        if (objetoEnMochila == null)
            return;
        objetoEnMochila.transform.SetParent(null);
        objetoEnMochila.transform.position = transform.TransformPoint(dropOffset);
        objetoEnMochila.GetComponent<MeshCollider>().enabled = true; //para que vuelva a colisionar al soltarlo, si es que tenía meshcollider
        if (objetoEnMochila.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
        objetoEnMochila = null;    //pa poder coger mas

    }
    private void TryPick(GameObject go)
    {
        if (objetoEnMochila != null)
            return;
        if(!go.CompareTag("Pick")) 
            return;
        objetoEnMochila = go;
        if(objetoEnMochila.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        objetoEnMochila.transform.SetParent(mochila, worldPositionStays:false);
        objetoEnMochila.transform.localPosition = Vector3.zero;
        objetoEnMochila.transform.localRotation = Quaternion.identity;
    }
    private void SoltarMano()
    {
        Transform obj = mano.transform.GetChild(0);
        objetoEnMochila.GetComponent<MeshCollider>().enabled = true; //para que vuelva a colisionar al soltarlo, si es que tenía meshcollider
        objetoEnMochila.GetComponent<Rigidbody>().isKinematic = false;
        obj.SetParent(null);
        obj.position = transform.TransformPoint(dropOffset);
        objetoEnMochila = null;    //pa poder coger mas
    }
}
