using System.Collections;
using TMPro;
using UnityEngine;

public class Oxigeno : MonoBehaviour
{
    [SerializeField] private float cantidadOxigeno = 120f; // Cantidad de oxígeno disponible
    [SerializeField] TextMeshProUGUI txtOxigeno; // Referencia al texto que muestra la cantidad de oxígeno

    public float tiempoActual;
    public float referenciaTimer = 10f;
    public bool timerCorriendo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiempoActual = referenciaTimer;
        timerCorriendo = false;
    }

    // Update is called once per frame
    void Update()
    {
        //txtOxigeno.text = cantidadOxigeno.ToString("F0"); // Actualiza el texto con la cantidad de oxígeno formateada sin decimales
        if (timerCorriendo) {
            tiempoActual -= Time.deltaTime; // Resta el tiempo transcurrido desde el último frame al tiempo actual
            txtOxigeno.text = tiempoActual.ToString("F0"); // Actualiza el texto con el tiempo actual formateado con dos decimales
            if (tiempoActual <= 0f) // Si el tiempo actual llega a cero o es menor
            {
                tiempoActual = 0f;
                timerCorriendo = false;

                TimerExpirado();
            }
        }
    }

    public void TimerExpirado()
    {
        Debug.Log("Ha terminado tiempo");
    }

    public void IniciarTimer()
    {
        tiempoActual = referenciaTimer; // Reinicia el tiempo actual al valor de referencia
        timerCorriendo = true;
    }

    public void ResetTimer()
    {
        tiempoActual = referenciaTimer; // Reinicia el tiempo actual al valor de referencia
        timerCorriendo = false; // Detiene el timer
    }
    //IEnumerator RestaOxigeno()
    //{
    //    cantidadOxigeno -= 1f; // Resta 1 unidad de oxígeno cada segundo
    //    yield return new WaitForSeconds(1f); // Espera 1 segundo antes de la siguiente resta
    
}
