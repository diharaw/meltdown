using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorState : MonoBehaviour
{
    [SerializeField]
    GameObject ReactorBody;

    [SerializeField]
    GameObject ReactorCore;

    [SerializeField]
    GameObject RingTop;

    [SerializeField]
    GameObject RingBottom;

    [SerializeField]
    GameObject FXState1;

    [SerializeField]
    GameObject FXState2;

    [SerializeField]
    GameObject FXState3;

    [SerializeField]
    float ReactorRingsSpinRate = 60.0f;

    [SerializeField]
    public float ReactorHealth = 1.0f;

    private Renderer Rend;

    // Start is called before the first frame update
    void Start()
    {
        if (ReactorCore)
        {
            Rend = ReactorCore.GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpinReactorRings();

        WobbleReactorBody();

        UpdateReactorCore();

        UpdateFX();
    }

    void SpinReactorRings()
    {
        if (RingTop && RingBottom)
        {
            RingBottom.transform.Rotate(0, Mathf.Lerp(ReactorRingsSpinRate * 7.0f, ReactorRingsSpinRate, ReactorHealth) * Time.deltaTime, Mathf.Lerp(Mathf.Sin(Time.time * 30.0f) * 3.0f, 0, ReactorHealth));

            RingTop.transform.Rotate(0, -1.0f * Mathf.Lerp(ReactorRingsSpinRate * 7.0f, ReactorRingsSpinRate, ReactorHealth) * Time.deltaTime, Mathf.Lerp(Mathf.Sin(Time.time * 30.0f) * 3.0f, 0, ReactorHealth));

            if (ReactorHealth >= 1.0f)
            {
                RingBottom.transform.rotation = Quaternion.Euler(0, 0, 0);
                RingTop.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }



    }

    void WobbleReactorBody()
    {
        if (ReactorBody)
        {
            ReactorBody.transform.Rotate(0, Mathf.Lerp(Mathf.Sin(Time.time * 30.0f) * 2.0f, 0, ReactorHealth), Mathf.Lerp(Mathf.Sin(Time.time * 20.0f) * 2.0f, 0, ReactorHealth));

            if (ReactorHealth >= 1.0f)
            {
                ReactorBody.transform.rotation = Quaternion.Euler(0, 0, 0);                
            }

        }

    }

    void UpdateReactorCore()
    {
        if (Rend)
        {
            Rend.material.SetFloat("Health", ReactorHealth);
            Rend.material.SetFloat("EmissiveBoost", Mathf.Lerp(200, 5, ReactorHealth));
        }
    }

    void UpdateFX()
    {
        if(FXState1 && FXState2 && FXState3)
        {
            if (ReactorHealth > 0.75f)
            {
                FXState1.SetActive(true);
                FXState2.SetActive(false);
                FXState3.SetActive(false);
            }
            else if (ReactorHealth > 0.35f)
            {
                FXState1.SetActive(true);
                FXState2.SetActive(true);
                FXState3.SetActive(false);
            }
            else
            {
                FXState1.SetActive(true);
                FXState2.SetActive(true);
                FXState3.SetActive(true);
            }
        }
    }

    public void SetReactorHealth(float Health)
    {
        ReactorHealth = Health;
    }
}
