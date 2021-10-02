using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool sharedInstance;
    public List<GameObject> m_bullets;
    public GameObject m_bulletPrefab;
    public int m_bulletPoolCount;

    // Start is called before the first frame update
    void Awake()
    {
        sharedInstance = this;
    }

    // Update is called once per frame
    void Start()
    {
        m_bullets = new List<GameObject>();

        GameObject tmp;

        for (int i = 0; i < m_bulletPoolCount; i++)
        {
            tmp = Instantiate(m_bulletPrefab);
            tmp.SetActive(false);
            m_bullets.Add(tmp);
        }
    }

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < m_bulletPoolCount; i++)
        {
            if (!m_bullets[i].activeInHierarchy)
            {                
                m_bullets[i].SetActive(true);
                m_bullets[i].GetComponent<Bullet>().ResetState();
                return m_bullets[i];
            }
        }

        return null;
    }
}
