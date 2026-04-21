using UnityEngine;

public class EnemyGravity : MonoBehaviour
{

    //public GameObject[] planets;
    private Rigidbody rb;
    public GameObject planet;
    public float gravityValue = 9.81F;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!planet) return;

        Vector3 gravityDirection = (planet.transform.position - transform.position).normalized;
        rb.AddForce(gravityDirection * gravityValue, ForceMode.Acceleration);
        transform.rotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;

        Debug.Log(Physics.gravity.ToString());
    }
}
