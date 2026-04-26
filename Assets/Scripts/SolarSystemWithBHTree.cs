using System.Collections.Generic;
using UnityEngine;
using BHTree;

public class SolarSystemWithBHTree : MonoBehaviour
{
    //real value of gravitational constant is 6.67430 × 10^-11 
    //but it makes movement too slow to showcase my simulation

    readonly float G = 1000f;
    //readonly float G = MathLib.ToSingle(MathLib.GravitationalConstant);

    GameObject[] celestials;
    List<Body> bodies = new List<Body>();

    [SerializeField] bool IsElipticalOrbit = false;

    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestials");

        //initialize bodies list
        foreach (GameObject gameObject in celestials)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Vector3 pos = gameObject.transform.position;
            bodies.Add(new Body(pos.x, pos.y, pos.z, rb.mass, G));
        }

        //SetInitialVelocity();
    }
    void FixedUpdate()
    {
        // Remove destroyed objects
        for (int i = celestials.Length - 1; i >= 0; i--)
        {
            if (celestials[i] == null)
            {
                //override list without the null gameobject and body from the octree
                List<GameObject> tempList = new List<GameObject>(celestials);
                tempList.RemoveAt(i);
                celestials = tempList.ToArray();

                bodies.RemoveAt(i);
            }
        }

        if (celestials.Length == 0)
        { 
            return; // nothing left
        }

        // Update body positions
        for (int i = 0; i < bodies.Count; i++)
        {
            Vector3 pos = celestials[i].transform.position;
            bodies[i].Update(pos.x, pos.y, pos.z);
            bodies[i].ResetForce();
        }

        // Compute bounding octant for all bodies
        Octant rootOctant = ComputeBoundingOctant();
        BarnesHutTree tree = new BarnesHutTree(rootOctant);

        // Insert bodies into the tree
        foreach (Body b in bodies)
        {
            tree.Insert(b);
        }

        // Compute forces using Barnes-Hut approximation
        foreach (Body b in bodies)
        {
            tree.updateForce(b);
        }

        // Apply forces to Rigidbodies
        for (int i = 0; i < celestials.Length; i++)
        {
            Rigidbody rb = celestials[i].GetComponent<Rigidbody>();
            Body b = bodies[i];
            rb.AddForce(new Vector3((float)b.fx, (float)b.fy, (float)b.fz));
        }
    }

    
// gives some speed so the planets don't just fall into the sun
//also sets the direction of movement to be perpendicular to the line connecting the two bodies
    void SetInitialVelocity() 
    {
        foreach (GameObject a in celestials)
        {
            foreach (GameObject b in celestials)
            {
                if(!a.Equals(b))
                {
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = MathLib.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform);
                    if (IsElipticalOrbit)
                    {
                        a.GetComponent<Rigidbody>().linearVelocity += a.transform.right * Mathf.Sqrt((G * m2) * ((2 / r) - (1 / (r * 1.5f))));
                    }
                    else
                    {
                        a.GetComponent<Rigidbody>().linearVelocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
                    }
                }
            }
        }
    }

   Octant ComputeBoundingOctant()
    {
        // Compute bounds of all bodies
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (Body b in bodies)
        {
            if (b != null)
            {
                if (b.rx < minX) minX = (float)b.rx;
                if (b.rx > maxX) maxX = (float)b.rx;
                if (b.ry < minY) minY = (float)b.ry;
                if (b.ry > maxY) maxY = (float)b.ry;
                if (b.rz < minZ) minZ = (float)b.rz;
                if (b.rz > maxZ) maxZ = (float)b.rz;
            }
        }

        double cx = (minX + maxX) / 2f;
        double cy = (minY + maxY) / 2f;
        double cz = (minZ + maxZ) / 2f;
        double len = MathLib.Max(maxX - minX, MathLib.Max(maxY - minY, maxZ - minZ)) * 1.5f;

        return new Octant(cx, cy, cz, len);
    } 
}