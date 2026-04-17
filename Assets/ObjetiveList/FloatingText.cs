using System.Collections;
using UnityEngine;
using TMPro;

// Esta clase se encarga de mostrar un texto flotante (ej: "¡Lleno!") sobre la nave cuando se intenta usar un item no aceptado

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoLleno;
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float duration = 1f;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.localPosition;
        gameObject.SetActive(false);
    }

    // Activar, posicionar, iniciar animación...
    public void Show(string message)
    {
        // if (gameObject.activeSelf) return; // Si en algun punto molesta el spam al pulsar, descomentar esto. No lo veo necesario actualmente

        textoLleno.text = message;

        // Resetear alpha para que no se quede transparente ups
        Color c = textoLleno.color;
        textoLleno.color = new Color(c.r, c.g, c.b, 1f);

        transform.localPosition = startPos;
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    // Animación de subir el texto y luego desactivarlo
    IEnumerator Animate()
    {
        float time = 0f;
        Color startColor = textoLleno.color;

        while (time < duration)
        {
            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            // Para que haga fade
            float alpha = 1 - (time / duration);
            textoLleno.color = new Color(startColor.r, startColor.g, startColor.b, alpha);


            time += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}