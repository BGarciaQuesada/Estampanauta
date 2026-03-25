using UnityEngine;

public class PlanetController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController2 playerController = other.GetComponent<PlayerController2>();
        if (playerController != null)
        {
            if (playerController.currentPlanet == transform) return;
            playerController.currentPlanet = transform;
            playerController.EnterNewGravityField();
        }
    }
}
