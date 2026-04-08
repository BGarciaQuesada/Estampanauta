using UnityEngine;

// Interfaz para objetos que PUEDEN USARSE
// Todo el punto de estas interfaces es simplificar su llamada en Player

public interface IItem
{
    void UseOn(IItemReceiver receiver, GameObject user);
}
