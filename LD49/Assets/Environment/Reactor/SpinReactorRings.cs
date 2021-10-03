using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinReactorRings : MonoBehaviour
{

    public bool DirectionClockwise = true;
    public float SpinRate = 60.0f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void spin()
    {
        if(DirectionClockwise)
        {
            transform.Rotate(0, SpinRate * Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, -1.0f *SpinRate * Time.deltaTime, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spin();
    }
}
