using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTransform.position;
        transform.rotation = followTransform.rotation;
    }
}
