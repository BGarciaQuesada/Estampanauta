using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Energia : MonoBehaviour
{
    public GameObject efectoFinEnergia;
    public GameObject efectoRecuperaEnergia;
    public float timer = 0;
    //public TextMeshProUGUI textoOxigeno;
    public float tiempoMax = 120f;
    public bool death = false;

    public Image barraBateria;
    public AudioSource audioSource;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoRecuperaBateria;

    public GameObject sparksFXPrefab;

    public MenuManager menuManager; // Referencia al MenuManager para llamar a la función de fade out

    private void Update()
    {
        timer -= Time.deltaTime;
        //textoOxigeno.text = "" + timer.ToString("F0"); // Actualiza el texto con el tiempo actual formateado sin decimales
        barraBateria.fillAmount = timer / tiempoMax; // Actualiza la barra de bater�a seg�n el tiempo restante (valor entre 0 y 1)

        if (timer <0)
        {
            timer = 0;
            if (!death)
            {
                GetComponent<PlayerController>().canMove = false; // Detiene el movimiento del jugador al quedarse sin ox�geno
                GetComponent<Animator>().SetTrigger("Death"); // Activa la animaci�n de muerte al quedarse sin ox�geno
                if(!audioSource.isPlaying)
                    audioSource.PlayOneShot(sonidoMuerte);
                death = true; // Marca que el jugador ha muerto para evitar que se active la animaci�n varias veces
                StartCoroutine(EfectoFinJuego());

            }            
            //pantalla fin juego
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            timer = tiempoMax; //devuelve el tiempo a su valor m�ximo al entrar en el trigger con el tag "Respawn"
            StartCoroutine(EfectoCargaEnergia());
        }
        if (other.CompareTag("Enemy"))
        {
            timer -= 10;
            
            GameObject fx = Instantiate(sparksFXPrefab, transform.position, Quaternion.identity);
            fx.transform.SetParent(GetComponent<RagdollControl>().pelvis);
            fx.transform.localScale = Vector3.one * 2;
            Destroy(fx, 2F);
        }
    }

    private void ActivarRagdoll()
    {
        GetComponent<RagdollControl>().ActivaRagdoll(); // Llama a la funci�n para activar el Ragdoll
    }

    IEnumerator EfectoFinJuego()
    {
        efectoFinEnergia.SetActive(true);
        yield return new WaitForSeconds(2f);
        efectoFinEnergia.SetActive(false);

        StartCoroutine(menuManager.FinJuego());
    }
    
    IEnumerator EfectoCargaEnergia()
    {
        efectoRecuperaEnergia.SetActive(true);
        yield return new WaitForSeconds(2f);
        efectoRecuperaEnergia.SetActive(false);
    }

    


}
