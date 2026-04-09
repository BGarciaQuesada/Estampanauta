using System.Collections;
using UnityEngine;

// Esta clase maneja el agarrado y soltado de objetos.

// [!] EL PLAYER ES EL QUE TIENE INPUT SYSTEM! No se manejan métodos con InputValue, solo la acción que conllevará hacerlo.
public class GrabbableBehavior : MonoBehaviour
{
    [SerializeField] private Transform grabPoint; // Punto donde aparecerá el objeto al ser agarrado

    private bool itemEquipped = false;
    public Collider collider;

    public void PickUpitem()
    {
        if (!itemEquipped) 
        {
            itemEquipped = true;
            collider.enabled = false; // Desactivar colisión para evitar problemas al agarrar el objeto

            // Hacer al objeto hijo de la "mano"
            Debug.Log("Me voy a la mano");
            transform.SetParent(grabPoint.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Avisar al player (busca que tenga PlayerInteraction y, si lo encuentra, le dice que el objeto que tiene en la mano es este)
            PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();
            
            if (player != null)
            {
                player.SetHeldItem(gameObject);
                GetComponent<MeshCollider>().enabled = false; // Desactivar colisión para evitar problemas al agarrar el objeto
                GetComponent<Rigidbody>().isKinematic = true; // Desactivar física para que el objeto no caiga mientras está agarrado
                player.objetoEnMano = gameObject; // Indicar que el jugador tiene un objeto en la mano
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !itemEquipped)
        {
            PickUpitem();
        }
    }

    public void DropItem()
    {
        if (itemEquipped)
        {
            collider.enabled = true; // Reactivar colisión para que el objeto pueda ser agarrado de nuevo
            StartCoroutine("CooldownPickUp");
        }
    }

    IEnumerator CooldownPickUp()
    {
        transform.SetParent(null);
        GetComponent<MeshCollider>().enabled = true; // Desactivar colisión para evitar problemas al agarrar el objeto
        GetComponent<Rigidbody>().isKinematic = false; // Desactivar física para que el objeto no caiga mientras está agarrado
        yield return new WaitForSeconds(2f);
        itemEquipped = false;
        
    }
}
