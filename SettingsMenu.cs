using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    public RectTransform sfxBar;
    public Text sfxText;
    public RectTransform musicBar;
    public Text musicText;
    public Animator anim;

    public static bool inSettings;

    private static SettingsMenu objectInstance;

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
        inSettings = false;
    }

    public void OnSFXClick(bool plus)
    {
        if (plus)
        {
            if (GameManager.instance.sfxSound < 2)
                GameManager.instance.sfxSound += .10f;
        }
        else //is Minus
        {
            if (GameManager.instance.sfxSound > 0)
                GameManager.instance.sfxSound -= .10f;
        }
        if (GameManager.instance.sfxSound < 0)
        {
            GameManager.instance.sfxSound = 0;
        }
        if (GameManager.instance.sfxSound > 2)
        {
            GameManager.instance.sfxSound = 2;
        }
        GameManager.instance.PlayAudio(6);
        GameManager.instance.SaveVolume();
        UpdateSoundBars();
    }

    public void OnMusicClick(bool plus)
    {
        if (plus)
        {
            if (GameManager.instance.musicSound < 2)
                GameManager.instance.musicSound += .10f;
        }
        else //is Minus
        {
            if (GameManager.instance.musicSound > 0)
                GameManager.instance.musicSound -= .10f;
        }
        if (GameManager.instance.musicSound < 0)
        {
            GameManager.instance.musicSound = 0;
        }
        if (GameManager.instance.musicSound > 2)
        {
            GameManager.instance.musicSound = 2;
        }
        GameManager.instance.PlayAudio(6);
        GameManager.instance.SaveVolume();
        UpdateSoundBars();
    }

    public void OnBackClick()
    {
        anim.SetTrigger("hide");
        GameManager.instance.SaveVolume();
        GameManager.instance.PlayAudio(5);
        GameManager.instance.PlayAudio(6);
        inSettings = false;
    }

    public void UpdateSoundBars()
    {
        sfxBar.localScale = new Vector3(GameManager.instance.sfxSound, 1, 1);
        musicBar.localScale = new Vector3(GameManager.instance.musicSound, 1, 1);
        sfxText.text = ((int)(100 * GameManager.instance.sfxSound+.01f)).ToString() + "%";
        musicText.text = ((int)(100 * GameManager.instance.musicSound +.01f)).ToString() + "%";

    }


}
