using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableDisk : MonoBehaviour
{
    public delegate void DiskHitEvent(HitableDisk disk);
    public event DiskHitEvent DiskHit;

    [SerializeField]
    private GameObject particlesPrefab = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject particles = Instantiate(particlesPrefab);
        particles.transform.position = transform.position;
        particles.transform.rotation = transform.rotation;

        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<Collider>().enabled = false;

        DiskHit?.Invoke(this);
    }
}