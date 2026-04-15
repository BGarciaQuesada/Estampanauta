using UnityEngine;
using UnityEngine.UI;

public class SliderColorChanger : MonoBehaviour
{
    [SerializeField] private Slider slider;     // Referencia al slider
    [SerializeField] private Image fillImage;   // Imagen del "Fill"

    [SerializeField] private Color startColor = Color.white; // Color inicial
    [SerializeField] private Color endColor = Color.blue;    // Color final

    void Start()
    {
        // Nos suscribimos al cambio de valor del slider
        slider.onValueChanged.AddListener(UpdateColor);

        // Aplicamos color inicial
        UpdateColor(slider.value);
    }

    void UpdateColor(float value)
    {
        // value va de minValue a maxValue --> lo normalizamos a 0-1
        float normalized = Mathf.InverseLerp(slider.minValue, slider.maxValue, value);

        // Interpolamos entre blanco y azul
        fillImage.color = Color.Lerp(startColor, endColor, normalized);
    }
}