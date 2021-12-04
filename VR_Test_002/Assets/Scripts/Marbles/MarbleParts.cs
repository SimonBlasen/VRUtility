using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleParts : MonoBehaviour
{
    [SerializeField]
    private MarbleTrackPart[] marblePartPrefabs = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MarbleTrackPart[] MarblePartPrefabs
    {
        get
        {
            return marblePartPrefabs;
        }
    }

    private static MarbleParts inst = null;
    public static MarbleParts Inst
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<MarbleParts>();
                for (int i = 0; i < inst.marblePartPrefabs.Length; i++)
                {
                    inst.marblePartPrefabs[i].ownIndex = i;
                }
            }
            return inst;
        }
    }
}


[Serializable]
public class MarbleTrackPart
{
    public GameObject prefabSelection;
    public GameObject prefabNonConvex;
    public GameObject prefabThrowing;
    [HideInInspector]
    public int ownIndex = -1;
}