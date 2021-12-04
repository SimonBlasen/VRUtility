using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleElevator : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabBucket = null;
    [SerializeField]
    private float moveUpSpeed = 1f;
    [SerializeField]
    private float bucketFrequency = 1f;
    [SerializeField]
    private Transform lowerPos = null;
    [SerializeField]
    private Transform upperPos = null;

    private float spawnCounter = 0f;

    private List<Transform> instBuckets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter += Time.deltaTime * bucketFrequency;

        if (spawnCounter >= 1f)
        {
            spawnCounter = 0f;

            spawnBucket();
        }

        for (int i = 0; i < instBuckets.Count; i++)
        {
            instBuckets[i].position = Vector3.MoveTowards(instBuckets[i].position, upperPos.position, Time.deltaTime * moveUpSpeed);

            if (Vector3.Distance(instBuckets[i].position, upperPos.position) <= 0.001f)
            {
                Destroy(instBuckets[i].gameObject);
                instBuckets.RemoveAt(i);
                i--;
            }
        }
    }

    private void spawnBucket()
    {
        GameObject instBucket = Instantiate(prefabBucket, transform);
        instBucket.transform.position = lowerPos.position;
        instBucket.transform.rotation = lowerPos.rotation;

        instBuckets.Add(instBucket.transform);
    }
}
