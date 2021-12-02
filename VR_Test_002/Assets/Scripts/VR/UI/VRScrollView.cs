using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VRScrollView : MonoBehaviour
{
    private ScrollRect scrollView = null;

    // Start is called before the first frame update
    void Start()
    {
        scrollView = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
