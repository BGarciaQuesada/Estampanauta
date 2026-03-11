using System.Collections;
using TMPro;
using UnityEngine;

public class Energia : MonoBehaviour
{
    public float timer = 0;
    public TextMeshProUGUI textoOxigeno;

    private void Update()
    {
        timer -= Time.deltaTime;
        textoOxigeno.text = "" + timer.ToString("F0"); // Actualiza el texto con el tiempo actual formateado sin decimales

        if(timer <0)
        {
                       timer = 0;
            Debug.Log("Ha terminado tiempo");
            //  detiene jugador
            //muestra aniamcaiojn de muerte
            //pantalla fin juego
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            timer = 120; // Aumenta el tiempo en 10 segundos al recoger el oxígeno
        }
    }
}
