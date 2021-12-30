using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerTrackpadIndic : MonoBehaviour
{
    private MeshRenderer meshRenderer = null;

    private void init()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            bool valToSet = isGlowing;
            Glowing = !valToSet;
            Glowing = valToSet;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private bool isGlowing = false;
    public bool Glowing
    {
        get
        {
            return isGlowing;
        }
        set
        {
            bool wasGlowing = isGlowing;
            isGlowing = value;
            init();

            if (wasGlowing != isGlowing)
            {
                meshRenderer.enabled = isGlowing;
            }
        }
    }
}
