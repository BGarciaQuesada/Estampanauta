using UnityEngine;

public class ItemGravityController : MonoBehaviour
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
        PlanetController2 planet = other.GetComponent<PlanetController2>();

        if (planet != null)
        {
            currentPlanet = planet.transform;
        }
    }
}