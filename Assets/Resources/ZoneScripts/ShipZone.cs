using UnityEngine;

// Esta clase maneja el comportamiento de la zona de la nave, la cual recibe consumibles

public class ShipZone : MonoBehaviour
{
    public bool Receive(IItem item, GameObject user)
    {
        // Lo del as: básicamente comprueba de forma segura si el item recibido es del tipo de la variable.
        // Si lo es, se le asigna a la variable
        // Si no, se le asigna null (manejado por el if)
        Bucket bucket = item as Bucket;
        if (bucket != null && bucket.IsFull)
        {
            bucket.Empty();
            Debug.Log("Cubo vaciado");
            return true;
        }

        Consumable consumable = item as Consumable;
        if (consumable != null)
        {
            Debug.Log("Consumible aplicado a la nave");
            return true; // Se destruye dentro de UseOn
        }

        // Si no se reconoce el item (aka no es un consumible), no se acepta
        return false;
    }
}
