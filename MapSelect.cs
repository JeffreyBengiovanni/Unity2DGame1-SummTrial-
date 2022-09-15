using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MapSelect : MonoBehaviour
{
    public Animator anim;
    public Animator mainMenuAnim;
    public int currentIndex = 0;
    public Image mapPreview;
    public Text mapName;
    public Text mapNameUnder;

    public static bool inSelection;

    private static MapSelect objectInstance;

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
        inSelection = false;
        UpdateMapSelect();
    }


    public void OnRightArrowClick()
    {
        GameManager.instance.PlayAudio(6);
        if (currentIndex < GameManager.instance.mapNames.Length - 1)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        UpdateMapSelect();
    }

    public void OnLeftArrowClick()
    {
        GameManager.instance.PlayAudio(6);
        if (currentIndex > 0)
        {
            currentIndex--;
        }
        else
        {
            currentIndex = GameManager.instance.mapNames.Length - 1;
        }
        UpdateMapSelect();
    }

    public void OnSelectClick()
    {
        PlayerPrefs.DeleteKey("SaveState");
        GameManager.instance.PlayAudio(6);
        MainMenu.inMainMenu = false;
        anim.SetTrigger("hide");
        mainMenuAnim.SetTrigger("hide");
        GameManager.instance.LoadGMFreshInt(currentIndex + 1);
        GameManager.instance.ResetInfo();
        GameManager.instance.SaveState();
    }


    public void OnBackClick()
    {
        anim.SetTrigger("hide");
        GameManager.instance.PlayAudio(6);
        inSelection = false;
    }

    public void UpdateMapSelect()
    {
        mapName.text = GameManager.instance.mapNames[currentIndex];
        mapNameUnder.text = GameManager.instance.mapNames[currentIndex];
        mapPreview.sprite = GameManager.instance.mapPreviews[currentIndex];
    }


}
