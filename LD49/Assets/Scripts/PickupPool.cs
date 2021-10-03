using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPool : MonoBehaviour
{
    public static PickupPool sharedInstance;

    public List<GameObject> m_coolantPickups;
    public GameObject m_coolantPickupPrefab;
    public int m_coolantPickupPoolCount;

    public List<GameObject> m_scrapPickups;
    public GameObject m_scrapPickupPrefab;
    public int m_scrapPickupPoolCount;

    // Start is called before the first frame update
    void Awake()
    {
        sharedInstance = this;
    }

    // Update is called once per frame
    void Start()
    {
        {
            m_coolantPickups = new List<GameObject>();

            GameObject tmp;

            for (int i = 0; i < m_coolantPickupPoolCount; i++)
            {
                tmp = Instantiate(m_coolantPickupPrefab);
                tmp.SetActive(false);
                m_coolantPickups.Add(tmp);
            }
        }

        {
            m_scrapPickups = new List<GameObject>();

            GameObject tmp;

            for (int i = 0; i < m_scrapPickupPoolCount; i++)
            {
                tmp = Instantiate(m_scrapPickupPrefab);
                tmp.SetActive(false);
                m_scrapPickups.Add(tmp);
            }
        }
    }

    public GameObject GetPooledCoolantPickup()
    {
        for (int i = 0; i < m_coolantPickupPoolCount; i++)
        {
            if (!m_coolantPickups[i].activeInHierarchy)
            {
                m_coolantPickups[i].SetActive(true);
                return m_coolantPickups[i];
            }
        }

        return null;
    }

    public GameObject GetPooledScrapPickup()
    {
        for (int i = 0; i < m_scrapPickupPoolCount; i++)
        {
            if (!m_scrapPickups[i].activeInHierarchy)
            {
                m_scrapPickups[i].SetActive(true);
                return m_scrapPickups[i];
            }
        }

        return null;
    }
}
