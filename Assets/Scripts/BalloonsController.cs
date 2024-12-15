using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsController : MonoBehaviour
{
    [SerializeField] List<GameObject> balloons;
    ParticleSystem ps;

    bool hasPopped;

    private void Awake()
    {
        hasPopped = false;
        ps = GetComponent<ParticleSystem>();
    }

    public void Pop()
    {
        if (!hasPopped)
        {
            ps.Play();
            foreach (GameObject b in balloons)
            {
                b.SetActive(false);
            }
            hasPopped = true;
        }
    }
}
