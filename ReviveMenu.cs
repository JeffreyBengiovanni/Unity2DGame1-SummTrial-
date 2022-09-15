using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReviveMenu : MonoBehaviour
{

    public Animator anim;

    public static ReviveMenu objectInstance;

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
    }

    public void OnRespawnClick()
    {
        anim.SetTrigger("hide");
        GameManager.instance.player.Revive();
        GameManager.instance.LoadGMInt(GameManager.instance.mapSelect.currentIndex+1);
    }

    public void OnMenuClick()
    {
        anim.SetTrigger("hide");
        GameManager.instance.player.Revive();
        GameManager.instance.mainMenu.ShowMainMenu();
        GameManager.instance.LoadGM("MainMenu");

    }
}
