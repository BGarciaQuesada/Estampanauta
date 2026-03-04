using UnityEngine;

// Esta clase maneja el comportamiento de las zonas de recursos, que pueden recibir herramientas para extraer recursos
// Todo menos el cohete, vamos

public class ResourceZone : MonoBehaviour
{
    public bool Receive(IItem item, GameObject user)
    {
        // Lo del as: básicamente comprueba de forma segura si el item recibido es del tipo de la variable.
        // Si lo es, se le asigna a la variable
        // Si no, se le asigna null (manejado por el if)
        Bucket bucket = item as Bucket;
        if (bucket != null && !bucket.IsFull)
        {
            bucket.Fill();
            return true;
        }

        Tool tool = item as Tool;
        if (tool != null)
        {
            Debug.Log("Herramienta usada en zona");
            return true;
        }

        // Si no se reconoce el item (aka no es una herramienta), no se acepta
        return false;
    }
}
