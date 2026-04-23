using System;
using Unity.Mathematics;
using UnityEngine;

public class PlanetBase : MonoBehaviour
{
    public double density;
    [SerializeField]private Rigidbody rb;

    void Update()
    {
        //density = (3 * rb.mass) / (4/3 * Mathf.PI * (PRadius * PRadius * PRadius));
        //mass = (4/3) * mathf.PI * (PRadius*PRadius*PRadius) * density
        float PRadius = (float)Math.Cbrt(3 * rb.mass / 4 * Mathf.PI * density);
        
        
        Vector3 scaleChange = new Vector3 (PRadius, PRadius - (PRadius * 0.02f), PRadius);
        rb.transform.localScale = scaleChange;


        if (rb.mass <= 0.0001)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rigidbody = collision.rigidbody;
        if(collision.gameObject.tag == "Celestials" && collision.gameObject.name != rb.name)
        {
            if (rb.mass > rigidbody.mass)
            {
                rb.mass += rigidbody.mass / 10;
                rigidbody.mass -= rigidbody.mass / 10;

                if (rigidbody.mass <= 0.0001)
                {
                    Destroy(collision.gameObject);
                }
                
            }
        }
    }
}
