using UnityEngine;
using UnityEngine.UI;

// Esta clase maneja la barra de progreso que se muestra al usar un objeto. Esto incluye mostrarla, ocultarla y actualizar su progreso.

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void Show()
    {
        Debug.Log("Show");
        gameObject.SetActive(true);
        SetProgress(0);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetProgress(float value)
    {
        fillImage.fillAmount = value;
    }
}
