using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{

    #region PRIVATE_PROPERTIES_SERIAL
    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject optionMenu;

    [SerializeField]
    Button optionMenuButton;

    [SerializeField, Tooltip("Should pause menu be opened at the start of the game?")]
    bool defaultOnPauseMenu = false;

    [SerializeField]
    KeyCode resumePauseCode = KeyCode.Escape;
    #endregion

    #region PRIVATE_PROPERTIES
    bool tooglePause
    {
        get
        {
            if (!pauseMenu)
                return false;
            return !pauseMenu.activeSelf;
        }
    }
    #endregion

    #region MONO_CALLBACKS
    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenu)
        {
            pauseMenu.SetActive(defaultOnPauseMenu);
        }

        if (!optionMenu)
        {
            
        }

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(resumePauseCode))
        {
            pauseMenu.SetActive(tooglePause);
        }
    }
    #endregion

    #region GAMEMENU_CALLBACKS
    public void ResumeGameBtnCallback()
    {
        pauseMenu.SetActive(tooglePause);
    }

    public void OptionsBtnCallback()
    {
        // TODO
    }

    public void LeaveRoomBtnCallback()
    {
        GameManager.LeaveRoom();
    }
    #endregion

}
