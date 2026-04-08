using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public List<IItem> items = new List<IItem>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        
        if (playerController != null)
        {
            if (playerController.currentPlanet == transform) return;
            playerController.currentPlanet = transform;
            playerController.EnterNewGravityField();
        }

        IItem item = other.GetComponent<IItem>();

        if (item != null)
        {
            items.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IItem item = other.GetComponent<IItem>();

        if (item != null)
        {
            items.Remove(item);
        }
    }
}