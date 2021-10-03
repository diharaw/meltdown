using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals sharedInstance;

    public bool m_isPaused = true;
    public bool m_isGameOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        sharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
