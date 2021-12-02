using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarbleIngameMenu : MonoBehaviour
{
    [SerializeField]
    private IngameMenu ingameMenu = null;

    // Start is called before the first frame update
    void Start()
    {
        ingameMenu.MenuOptionClicked += IngameMenu_MenuOptionClicked;
    }

    private void IngameMenu_MenuOptionClicked(string menuOption)
    {
        if (menuOption == "Back to main menu")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
