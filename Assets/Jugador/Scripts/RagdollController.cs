using System.Collections;
using UnityEngine;

public class RagdollControl : MonoBehaviour
{
    public GameObject root;
    public Transform pelvis; // Objeto ra�z del Ragdoll (puede ser el pelvis o cualquier otro objeto que act�e como ra�z)
    public Rigidbody[] rigidbodies;
    public Collider[] colliders;
    public Collider mainCollider; // Collider del objeto principal (si es necesario)
    public Vector3 fuerza;
    public Animator animator;
    private PlayerController playerController; // Referencia al script PlayerController para controlar el movimiento del jugador
    private bool quitandoEnergia = false;

    public GameObject sparksFXPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbodies = root.GetComponentsInChildren<Rigidbody>();
        colliders = root.GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>(); // Asigna el Collider del objeto principal si es necesario
        playerController = GetComponent<PlayerController>(); // Obtiene la referencia al script PlayerController
        DesactivaRagdoll();
    }

    public void ActivaRagdoll()
    {
        Debug.Log("Activando Ragdoll..."); // Agrega un mensaje de depuraci�n para verificar que se llama a esta funci�n
        animator.enabled = false; // Desactiva el Animator para evitar conflictos con el Ragdoll
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false; // Activa la f�sica para cada Rigidbody
            rb.AddForce(fuerza, ForceMode.Impulse);
        }
        foreach (Collider col in colliders)
        {
            col.enabled = true; // Asegura que los Colliders est�n habilitados
        }
        mainCollider.enabled = false; // Desactiva el Collider del objeto principal para evitar colisiones no deseadas con el Ragdoll
        playerController.canMove = false; // Desactiva el movimiento del jugador mientras el Ragdoll est� activo
    }

    public void DesactivaRagdoll()
    {
        Debug.Log("Desactivando Ragdoll..."); // Agrega un mensaje de depuraci�n para verificar que se llama a esta funci�n
                                              // mover root a la posici�n final del ragdoll
        transform.position = pelvis.position;

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in colliders)
        {
            if (col.gameObject != this.gameObject)
                col.enabled = false;
        }

        animator.enabled = true;
        mainCollider.enabled = true; // Reactiva el Collider del objeto principal si es necesario
        playerController.canMove = true; // Reactiva el movimiento del jugador despu�s de desactivar el Ragdoll
    }


    IEnumerator DesactivaRagdollDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        DesactivaRagdoll(); // Desactiva el Ragdoll despu�s de un tiempo
        quitandoEnergia = false; // Permite que se vuelva a quitar energ�a al colisionar con un enemigo despu�s de desactivar el Ragdoll
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (quitandoEnergia)
                return;
            quitandoEnergia = true;
            Debug.Log("quito 10");
            GetComponent<Energia>().timer -= 10f; // Resta 10 segundos al tiempo de ox�geno al colisionar con un enemigo
            ActivaRagdoll(); // Activa el Ragdoll al presionar la tecla K
            if (GetComponent<Energia>().timer < 0)
                return;
            StartCoroutine(DesactivaRagdollDespuesDeTiempo(2f)); // Desactiva el Ragdoll despu�s de 5 segundos

            GameObject fx = Instantiate(sparksFXPrefab, transform.position, Quaternion.identity);
            fx.transform.SetParent(GetComponent<RagdollControl>().pelvis);
            fx.transform.localScale = Vector3.one * 2;
            Destroy(fx, 2F);
        }
            
    }
}
