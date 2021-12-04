using SappAnims;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    [SerializeField]
    private SappAnim animScaleup = null;
    [SerializeField]
    private float scrollYThresh = 0.6f;
    [SerializeField]
    private string[] menuOptions = null;
    [SerializeField]
    private GameObject firstButton = null;
    [SerializeField]
    private Transform instParent = null;
    [SerializeField]
    private float buttonsSpacing = 0.6f;
    [SerializeField]
    private float buttonsSpacingOffset = 0.6f;
    [SerializeField]
    private SappAnim selectionBox = null;

    private VRController vrController = null;

    private bool menuOpened = false;

    private float prevScrollY = 0f;
    private float scrollY = 0f;
    private int curSelIndex = 0;

    private Transform[] instOptions = null;

    public delegate void MenuOptionClickedEvent(string menuOption);
    public event MenuOptionClickedEvent MenuOptionClicked;

    // Start is called before the first frame update
    void Start()
    {
        vrController = GetComponentInParent<VRController>();

        animScaleup.transform.localScale = new Vector3(1f, 0f, 1f);
        animScaleup.Scale = new Vector3(1f, 0f, 1f);
        selectionBox.LocalPosition = new Vector3(0f, buttonsSpacingOffset, 0f);

        instOptions = new Transform[menuOptions.Length];
        instOptions[0] = firstButton.transform;
        instOptions[0].localPosition = new Vector3(0f, buttonsSpacingOffset, 0f);
        instOptions[0].GetComponentInChildren<TextMeshProUGUI>().text = menuOptions[0];
        for (int i = 1; i < menuOptions.Length; i++)
        {
            GameObject instMenuOption = Instantiate(firstButton, instParent);
            instOptions[i] = instMenuOption.transform;
            instOptions[i].localPosition = new Vector3(0f, i * buttonsSpacing + buttonsSpacingOffset, 0f);
            instOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = menuOptions[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vrController.MenuButtonDown)
        {
            menuOpened = !menuOpened;
            animScaleup.Scale = menuOpened ? new Vector3(1f, 1f, 1f) : new Vector3(1f, 0f, 1f);
        }

        if (menuOpened)
        {
            if (vrController.TrackpadTouchDown)
            {
                prevScrollY = vrController.Trackpad.y;
            }

            if (vrController.TrackpadTouch && vrController.Trackpad.y != 0f)
            {
                scrollY = vrController.Trackpad.y;
            }

            if (vrController.TrackpadSwipeUp)
            {
                scroll(true);
            }
            else if (vrController.TrackpadSwipeDown)
            {
                scroll(false);
            }

            /*if (vrController.TrackpadTouchUp)
            {
                float scrollYDelta = scrollY - prevScrollY;

                if (Mathf.Abs(scrollYDelta) >= scrollYThresh)
                {
                    scroll(scrollYDelta > 0f);
                }
            }*/

            if (vrController.TrackpadButtonDown)
            {
                MenuOptionClicked?.Invoke(menuOptions[curSelIndex]);
            }
        }
    }

    private void scroll(bool up)
    {
        int prevIndex = curSelIndex;
        if (up)
        {
            curSelIndex++;
        }
        else
        {
            curSelIndex--;
        }

        curSelIndex = Mathf.Clamp(curSelIndex, 0, menuOptions.Length - 1);

        if (curSelIndex != prevIndex)
        {
            selectionBox.LocalPosition = instOptions[curSelIndex].localPosition;
        }
    }

    public bool MenuOpened
    {
        get
        {
            return menuOpened;
        }
    }


}
