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
        textoLleno.text = message;
        transform.localPosition = startPos;
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    // Animación de subir el texto y luego desactivarlo
    IEnumerator Animate()
    {
        float time = 0f;

        while (time < duration)
        {
            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}