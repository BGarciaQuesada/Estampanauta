using System.Collections;
using UnityEngine;

public class RagdollControl : MonoBehaviour
{
    public GameObject root;
    public Transform pelvis; // Objeto raíz del Ragdoll (puede ser el pelvis o cualquier otro objeto que actúe como raíz)
    public Rigidbody[] rigidbodies;
    public Collider[] colliders;
    public Collider mainCollider; // Collider del objeto principal (si es necesario)
    public Vector3 fuerza;
    public Animator animator;
    private PlayerController playerController; // Referencia al script PlayerController para controlar el movimiento del jugador
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
        Debug.Log("Activando Ragdoll..."); // Agrega un mensaje de depuración para verificar que se llama a esta función
        animator.enabled = false; // Desactiva el Animator para evitar conflictos con el Ragdoll
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false; // Activa la física para cada Rigidbody
            rb.AddForce(fuerza, ForceMode.Impulse);
        }
        foreach (Collider col in colliders)
        {
            col.enabled = true; // Asegura que los Colliders estén habilitados
        }
        mainCollider.enabled = false; // Desactiva el Collider del objeto principal para evitar colisiones no deseadas con el Ragdoll
        playerController.canMove = false; // Desactiva el movimiento del jugador mientras el Ragdoll está activo
    }

    public void DesactivaRagdoll()
    {
        Debug.Log("Desactivando Ragdoll..."); // Agrega un mensaje de depuración para verificar que se llama a esta función
                                              // mover root a la posición final del ragdoll
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
        playerController.canMove = true; // Reactiva el movimiento del jugador después de desactivar el Ragdoll
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DesactivaRagdollDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        DesactivaRagdoll(); // Desactiva el Ragdoll después de un tiempo
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ActivaRagdoll(); // Activa el Ragdoll al presionar la tecla K
            StartCoroutine(DesactivaRagdollDespuesDeTiempo(2f)); // Desactiva el Ragdoll después de 5 segundos
        }
            
    }
}
