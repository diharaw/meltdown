using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolantPickup : MonoBehaviour
{
    public float m_coolantAmount = 250.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PowerPlantController.sharedInstance.DoRepair(m_coolantAmount);
            gameObject.SetActive(false);
        }
    }
}
