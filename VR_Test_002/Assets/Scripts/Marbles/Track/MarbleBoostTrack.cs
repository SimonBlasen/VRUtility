using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleBoostTrack : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] arrowMeshes = null;
    [SerializeField]
    private float boostStrength = 1f;
    [SerializeField]
    private Material matOn = null;
    [SerializeField]
    private Material matOff = null;
    [SerializeField]
    private float colorAnimTime = 1f;
    [SerializeField]
    private float arrowsAnimOffset = 1f;
    [SerializeField]
    private float arrowsPercentageOn = 1f;


    private float animS = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animS += Time.deltaTime / colorAnimTime;

        if (animS >= 1f)
        {
            animS -= 1f;
        }

        for (int i = 0; i < arrowMeshes.Length; i++)
        {
            float animSHere = animS + arrowsAnimOffset * i;
            animSHere = animSHere % 1f;

            if (animSHere <= arrowsPercentageOn)
            {
                arrowMeshes[i].sharedMaterial = matOn;
            }
            else
            {
                arrowMeshes[i].sharedMaterial = matOff;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRig = other.GetComponent<Rigidbody>();
        if (otherRig != null)
        {
            otherRig.AddForce(transform.forward * boostStrength * Time.fixedDeltaTime, ForceMode.Force);
        }
    }
}
