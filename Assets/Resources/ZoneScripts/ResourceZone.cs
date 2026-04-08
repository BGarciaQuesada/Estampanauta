using UnityEngine;

// Esta clase maneja el comportamiento de las zonas de recursos, que pueden recibir herramientas para extraer recursos
// Todo menos el cohete, vamos

public class ResourceZone : MonoBehaviour, IItemReceiver
{
    [SerializeField] private GameObject resourcePrefab; // Prefab del recurso que se va a generar al usar la herramienta
    [SerializeField] private bool canReceiveBuckets;    // Si esta zona puede recibir cubos para rellenarlos
    [SerializeField] private bool canReceiveTools;      // Si esta zona puede recibir herramientas (pico)

    public bool Receive(IItem item, GameObject user)
    {
        // Lo del as: básicamente comprueba de forma segura si el item recibido es del tipo de la variable.
        // Si lo es, se le asigna a la variable
        // Si no, se le asigna null (manejado por el if)
        Bucket bucket = item as Bucket;
        if (bucket != null && !bucket.IsFull && canReceiveBuckets)
        {
            bucket.Fill();
            return true;
        }

        Tool tool = item as Tool;
        if (tool != null && canReceiveTools)
        {
            Debug.Log("Herramienta usada en zona");

            // Tiene que spawnear el recurso, y esto tiene que llamar a PlanetController para que se lo añada al inventario del planeta
            Vector3 spawnPos = transform.position + transform.up * 1.5f;
            GameObject resource = Instantiate(resourcePrefab, spawnPos, Quaternion.identity);

            ItemGravityController gravity = resource.GetComponent<ItemGravityController>();

            if (gravity != null)
            {
                PlayerController2 player = user.GetComponent<PlayerController2>();

                if (player != null)
                    gravity.currentPlanet = player.currentPlanet;
            }


            return true;
        }

        // Si no se reconoce el item (aka no es una herramienta válida), no se acepta
        return false;
    }
}
