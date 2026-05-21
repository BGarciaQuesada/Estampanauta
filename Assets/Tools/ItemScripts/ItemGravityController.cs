using Fusion;
using UnityEngine;

public class ItemGravityController : NetworkBehaviour
{
    public Transform currentPlanet;
    public float gravity = -20f;
    Rigidbody rb;

    void Awake()
    {
        // Auto-asignar Rigidbody del objeto
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // Solo ApplyGravity() de player
    {
        if (currentPlanet == null) return;

        Vector3 normalVector = (transform.position - currentPlanet.position).normalized;

        rb.AddForce(-normalVector * Mathf.Abs(gravity), ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentPlanet == null)
        {
            if (other.CompareTag("Planet"))
            {
                Transform planet = other.transform.parent;

                if (planet != null)
                {
                    currentPlanet = planet;
                }
            }
        }
        
    }

}