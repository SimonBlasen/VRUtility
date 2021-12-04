using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static void removeCollidersRec(Transform trans)
    {
        Collider[] colliders = trans.GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
