using System.Collections;
using UnityEngine;

// Esta clase maneja el agarrado y soltado de objetos.

// [!] EL PLAYER ES EL QUE TIENE INPUT SYSTEM! No se manejan métodos con InputValue, solo la acción que conllevará hacerlo.
public class GrabbableBehavior : MonoBehaviour
{
    [SerializeField] private Transform grabPoint; // Punto donde aparecerá el objeto al ser agarrado

    private bool itemEquipped = false;

    public void PickUpitem()
    {
        // [!] ESTE IF ES REDUNDANTE Y DEBERÍA SER CONTROLADO EN PLAYER. Hasta que no se haga merge, se queda así.
        if (!itemEquipped) 
        {
            itemEquipped = true;

            // Hacer al objeto hijo de la "mano"
            transform.SetParent(grabPoint.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public void DropItem()
    {
        if (itemEquipped)
        {
            StartCoroutine("CooldownPickUp");
        }
    }

    IEnumerator CooldownPickUp()
    {
        transform.SetParent(null);
        yield return new WaitForSeconds(2f);
        itemEquipped = false;
    }
}
