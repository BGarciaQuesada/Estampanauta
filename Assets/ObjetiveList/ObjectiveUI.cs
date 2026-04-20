using UnityEngine;
using TMPro;

// Esta clase se encarga de actualizar el UI de un objetivo específico, mostrando un checkmark si es de 1 requerido o un contador si es de más de 1 requerido.
// SE ASIGNA A CADA ELEMENTO DE LA LISTA

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private string id;

    // En principio las checkmarks iban a ser para objetivos individuales pero... no hay. Se queda ahí por si acaso.
    // [SerializeField] private GameObject checkmark; // ✔
    [SerializeField] private TextMeshProUGUI counterText; // 0/X

    public void UpdateUI(Objective obj)
    {
        if (obj.required == 1)
        {
            // if (checkmark != null)
            // checkmark.SetActive(obj.IsComplete);

            if (counterText != null)
                counterText.gameObject.SetActive(false);
        }
        else
        {
            //  if (checkmark != null)
            // checkmark.SetActive(false);

            if (counterText != null)
            {
                counterText.gameObject.SetActive(true);
                counterText.text = $"{obj.current}/{obj.required}";
            }
        }
    }

    public string GetID()
    {
        return id;
    }
}