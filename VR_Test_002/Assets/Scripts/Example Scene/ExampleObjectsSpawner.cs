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

    private VRInteractableGrab lastObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastObject == null)
        {
            counter += Time.deltaTime;
            if (counter >= spawnTime)
            {
                counter = 0f;

                spawnObject();
            }
        }
        else
        {
            if (Vector3.Distance(lastObject.transform.position, transform.position) >= 0.3f)
            {
                Destroy(lastObject.gameObject, 60f);
                lastObject = null;
                counter = 0f;
            }
        }

    }

    private void spawnObject()
    {
        int spawnPrefabIndex = Random.Range(0, prefabs.Length);

        GameObject instObj = Instantiate(prefabs[spawnPrefabIndex]);
        if (instObj.GetComponent<Rigidbody>() != null)
        {
            instObj.GetComponent<Rigidbody>().maxAngularVelocity = float.PositiveInfinity;
        }
        instObj.transform.position = spawnPos.position;

        lastObject = instObj.GetComponent<VRInteractableGrab>();
    }
}
