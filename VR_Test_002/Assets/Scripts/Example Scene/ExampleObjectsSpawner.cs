using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleObjectsSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private Transform spawnPos;
    [SerializeField]
    private float spawnTime = 2f;
    [SerializeField]
    private float destoryAfter = 30f;

    private float counter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= spawnTime)
        {
            counter = 0f;

            spawnObject();
        }
    }

    private void spawnObject()
    {
        int spawnPrefabIndex = Random.Range(0, prefabs.Length);

        GameObject instObj = Instantiate(prefabs[spawnPrefabIndex]);
        instObj.transform.position = spawnPos.position;


        Destroy(instObj, destoryAfter);
    }
}
