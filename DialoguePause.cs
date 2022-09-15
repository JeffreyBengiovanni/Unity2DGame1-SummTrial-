using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePause : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool canUsePause = true;
    public static bool gameActive = true;
    private bool unlocked = true;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
   

    public void CommencePause()
    {
        if (canUsePause || unlocked)
        {
            unlocked = false;
            GameManager.instance.PlayAudio(6);
            canUsePause = false;
            StartCoroutine(PauseAllowed());
            PauseState();
        }
    }

    IEnumerator PauseAllowed()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        unlocked = true;
    }

    private void PauseState()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
        isPaused = !isPaused;

    }

    private void ResumeGame()
    {
        StartCoroutine(Resuming());
    }
    IEnumerator Resuming()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        anim.SetTrigger("hide");
        Time.timeScale = 1;
        gameActive = true;
        canUsePause = true;
    }

    public void MainMenuResume(bool hide)
    {
        if (hide)
            anim.SetTrigger("hide");
        else
        {
            anim.SetTrigger("show");
        }
        Time.timeScale = 1;
        isPaused = false;
        gameActive = true;
    }

    private void PauseGame()
    {
        anim.SetTrigger("show");
        StartCoroutine(Pausing());
    }

    IEnumerator Pausing()
    {
        Time.timeScale = 0;
        gameActive = false;
        yield return new WaitForSecondsRealtime(0.0f);
    }

}
