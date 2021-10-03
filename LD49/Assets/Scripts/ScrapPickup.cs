using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapPickup : MonoBehaviour
{
    public int m_scrapAmount = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().AddScrap(m_scrapAmount);
            gameObject.SetActive(false);
        }
    }
}