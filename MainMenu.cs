using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public Pause pause;
    private bool startup;
    private Animator anim;
    public static bool inMainMenu;
    public Button continueButton;
    public bool initial = true;
    public Animator selectAnim;


    public static MainMenu objectInstance;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (objectInstance == null)
        {
            objectInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void Start()
    {
        anim = GetComponent<Animator>();
        inMainMenu = true;
        if (!PlayerPrefs.HasKey("SaveState"))
        {
            // Disable continue button since no save state
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
        ShowMainMenu();
        GameManager.instance.mapSelect.UpdateMapSelect();
    }

    public void ShowMainMenu()
    {
        if (!inMainMenu)
        {
            anim.SetTrigger("show");
            inMainMenu = true;
        }
    }

    public void Update()
    {
        if (inMainMenu && initial && PlayerPrefs.HasKey("SaveState"))
        {
            continueButton.interactable = true;
            initial = false;
        }
    }

    public void OnContinueClick()
    {
        GameManager.instance.PlayAudio(6);
        inMainMenu = false;
        anim.SetTrigger("hide");
        GameManager.instance.LoadGMInt(GameManager.instance.mapSelect.currentIndex+1);
        GameManager.instance.cameraMotor.targetLock = false;
        GameManager.instance.cameraMotor.changeTarget = true;
    }

    public void OnNewGameClick()
    {
        GameManager.instance.PlayAudio(6);
        selectAnim.SetTrigger("show");
        GameManager.instance.cameraMotor.targetLock = false;
        GameManager.instance.cameraMotor.changeTarget = true;
    }

    public void OnExitGameClick()
    {
        GameManager.instance.PlayAudio(6);
        Application.Quit();
    }
}
