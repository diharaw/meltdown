using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool sharedInstance;
    public List<GameObject> m_cannonBullets;
    public GameObject m_cannonBulletPrefab;
    public int m_cannonBulletPoolCount;

    public List<GameObject> m_minigunBullets;
    public GameObject m_minigunBulletPrefab;
    public int m_minigunBulletPoolCount;

    public List<GameObject> m_energyBullets;
    public GameObject m_energyBulletPrefab;
    public int m_energyBulletPoolCount;

    // Start is called before the first frame update
    void Awake()
    {
        sharedInstance = this;
    }

    // Update is called once per frame
    void Start()
    {
        {
            m_cannonBullets = new List<GameObject>();

            GameObject tmp;

            for (int i = 0; i < m_cannonBulletPoolCount; i++)
            {
                tmp = Instantiate(m_cannonBulletPrefab);
                tmp.SetActive(false);
                m_cannonBullets.Add(tmp);
            }
        }

        {
            m_minigunBullets = new List<GameObject>();

            GameObject tmp;

            for (int i = 0; i < m_minigunBulletPoolCount; i++)
            {
                tmp = Instantiate(m_minigunBulletPrefab);
                tmp.SetActive(false);
                m_minigunBullets.Add(tmp);
            }
        }

        {
            m_energyBullets = new List<GameObject>();

            GameObject tmp;

            for (int i = 0; i < m_energyBulletPoolCount; i++)
            {
                tmp = Instantiate(m_energyBulletPrefab);
                tmp.SetActive(false);
                m_energyBullets.Add(tmp);
            }
        }
    }

    public GameObject GetPooledCannonBullet()
    {
        for (int i = 0; i < m_cannonBulletPoolCount; i++)
        {
            if (!m_cannonBullets[i].activeInHierarchy)
            {
                m_cannonBullets[i].SetActive(true);
                m_cannonBullets[i].GetComponent<Bullet>().ResetState();
                return m_cannonBullets[i];
            }
        }

        return null;
    }

    public GameObject GetPooledMinigunBullet()
    {
        for (int i = 0; i < m_minigunBulletPoolCount; i++)
        {
            if (!m_minigunBullets[i].activeInHierarchy)
            {
                m_minigunBullets[i].SetActive(true);
                m_minigunBullets[i].GetComponent<Bullet>().ResetState();
                return m_minigunBullets[i];
            }
        }

        return null;
    }

    public GameObject GetPooledEnergyBullet()
    {
        for (int i = 0; i < m_energyBulletPoolCount; i++)
        {
            if (!m_energyBullets[i].activeInHierarchy)
            {
                m_energyBullets[i].SetActive(true);
                m_energyBullets[i].GetComponent<Bullet>().ResetState();
                return m_energyBullets[i];
            }
        }

        return null;
    }
}
