using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Energia : MonoBehaviour
{
    public float timer = 0;
    //public TextMeshProUGUI textoOxigeno;
    public float tiempoMax = 120f;
    public bool death = false;

    public Image barraBateria;

    private void Update()
    {
        timer -= Time.deltaTime;
        //textoOxigeno.text = "" + timer.ToString("F0"); // Actualiza el texto con el tiempo actual formateado sin decimales
        barraBateria.fillAmount = timer / tiempoMax; // Actualiza la barra de batería según el tiempo restante (valor entre 0 y 1)

        if (timer <0)
        {
            timer = 0;
            if (!death)
            {
                GetComponent<PlayerController>().canMove = false; // Detiene el movimiento del jugador al quedarse sin oxígeno
                GetComponent<Animator>().SetTrigger("Death"); // Activa la animación de muerte al quedarse sin oxígeno
                death = true; // Marca que el jugador ha muerto para evitar que se active la animación varias veces
            }            
            //  detiene jugador
            //muestra aniamcaiojn de muerte
            //pantalla fin juego
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            timer = tiempoMax; //devuelve el tiempo a su valor máximo al entrar en el trigger con el tag "Respawn"
        }
    }

    private void ActivarRagdoll()
    {
        GetComponent<RagdollControl>().ActivaRagdoll(); // Llama a la función para activar el Ragdoll
    }
}
