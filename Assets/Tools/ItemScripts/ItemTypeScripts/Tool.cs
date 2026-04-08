using UnityEngine;

// Esta clase es para el comportamiento de las herramientas, las cuales NO SE DESTRUYEN al usarlas

public class Tool : MonoBehaviour, IItem
{
    public void UseOn(IItemReceiver receiver, GameObject user)
    {
        bool accepted = receiver.Receive(this, user);

        if (accepted)
        {
            Debug.Log("Herramienta usada");
        }
    }
}
