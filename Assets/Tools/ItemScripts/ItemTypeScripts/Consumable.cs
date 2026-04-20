using UnityEngine;

// Esta clase es para el comportamiento de las herramientas, las cuales NO SE DESTRUYEN al usarlas

public class Consumable : MonoBehaviour, IItem
{
    // Necesito especificar el ID aquí temprano para que luego se puedan reconocer los distintos scraps como el mismo objetivo
    [SerializeField] private string objectiveID; // "Crystal", "Scrap"

    public string GetObjectiveID()
    {
        return objectiveID;
    }

    public void UseOn(IItemReceiver receiver, GameObject user)
    {
        bool accepted = receiver.Receive(this, user);

        if (accepted) // SI HA sido aceptado, destruir
        {
            Debug.Log("Recurso consumido");
            Destroy(gameObject); // A tomar por cleta la biciculo
        }
    }
}
