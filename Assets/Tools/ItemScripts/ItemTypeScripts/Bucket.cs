using UnityEngine;

// [!] El bucket es más especialito porque se usa tanto como herramienta como aplicable tras rellenar.
public class Bucket : MonoBehaviour, IItem
{
    [SerializeField] private GameObject liquid; // Referencia para ocultarlo/mostrarlo

    public bool IsFull = false;

    public void Start()
    {
        liquid.SetActive(false); // Asegurar que el líquido esté oculto al inicio
    }

    public void Fill()
    {
        liquid.SetActive(true);
        IsFull = true;
        Debug.Log("Bucket lleno");
    }

    public void Empty()
    {
        liquid.SetActive(false);
        IsFull = false;
        Debug.Log("Bucket vacío");
    }

    public void UseOn(IItemReceiver receiver, GameObject user)
    {
        receiver.Receive(this, user);
    }
}
