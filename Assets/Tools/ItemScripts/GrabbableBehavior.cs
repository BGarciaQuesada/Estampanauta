using System.Collections;
using UnityEngine;

// Esta clase maneja el agarrado y soltado de objetos.

// [!] EL PLAYER ES EL QUE TIENE INPUT SYSTEM! No se manejan métodos con InputValue, solo la acción que conllevará hacerlo.
public class GrabbableBehavior : MonoBehaviour
{
    [SerializeField] public Transform grabPoint; // Punto donde aparecerá el objeto al ser agarrado

    private bool itemEquipped = false;
    public Collider colliderColision;   //hay que asignar el objeto Collider hijo
    private Rigidbody rb;
    [SerializeField] private GameObject efectoBrillo; // Efecto de brillo para indicar que el objeto es agarrable

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        efectoBrillo = transform.Find("Brillo Objetos").gameObject; // Asignar el efecto de brillo desde el hijo del objeto
    }
    public void PickUpitem()
    {
        if (!itemEquipped) 
        {
            // Avisar al player (busca que tenga PlayerInteraction y, si lo encuentra, le dice que el objeto que tiene en la mano es este)
            PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();
            if (player.objetoEnMano != null)
                return;

            efectoBrillo.SetActive(false); // Desactivar el efecto de brillo al agarrar el objeto            
            player.itemsSFX.PlayOneShot(player.sonidoRecogeObjeto);

            itemEquipped = true;
            colliderColision.enabled = false; // Desactivar colisión para evitar problemas al agarrar el objeto

            // Hacer al objeto hijo de la "mano"
            Debug.Log("Me voy a la mano");
            transform.SetParent(grabPoint.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            
            if (player != null)
            {
                player.SetHeldItem(gameObject);
                colliderColision.enabled = false; // Desactivar colisión para evitar problemas al agarrar el objeto
                rb.isKinematic = true; // Desactivar física para que el objeto no caiga mientras está agarrado
                player.objetoEnMano = gameObject; // Indicar que el jugador tiene un objeto en la mano
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !itemEquipped)
        {
            grabPoint = other.GetComponent<PlayerController>().mano;

            PickUpitem();
            
        }
    }

    public void DropItem(Transform currentPlanet)
    {
        if (itemEquipped)
        {
            colliderColision.enabled = true; // Reactivar colisión para que el objeto pueda ser agarrado de nuevo
            GetComponent<ItemGravityController>().currentPlanet = currentPlanet; // Asignar el planeta actual para que el objeto caiga correctamente
            StartCoroutine("CooldownPickUp");
            efectoBrillo.SetActive(true); // Reactivar el efecto de brillo al soltar el objeto para indicar que es agarrable de nuevo
        }
    }

    IEnumerator CooldownPickUp()
    {
        transform.SetParent(null);
        colliderColision.enabled = true; // Desactivar colisión para evitar problemas al agarrar el objeto
        rb.isKinematic = false; // Desactivar física para que el objeto no caiga mientras está agarrado
        yield return new WaitForSeconds(2f);
        itemEquipped = false;
        
    }
}
