using UnityEngine;

// Interfaz para objetos que PUEDEN RECIBIR ITEMS. Devuelve booleanos para indicar si ha sido recibido y, en consecuencia, destruirlos/vaciarlos
// Todo el punto de estas interfaces es simplificar su llamada en Player

public interface IItemReceiver
{
    bool Receive(IItem item, GameObject user);
}
