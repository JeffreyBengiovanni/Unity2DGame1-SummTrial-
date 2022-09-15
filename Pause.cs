using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool canUsePause = true;
    public static bool gameActive = true;
    private Animator anim;
    public int upgradeStart;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (!MainMenu.inMainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                && DialoguePause.isPaused == false && SkillPause.isPaused == false
                && canUsePause && SkillPause.canUsePause && DialoguePause.canUsePause
                && !SettingsMenu.inSettings)
            {
                CommencePause();
            }
        }
    }

    public void CommencePause()
    {
        if (canUsePause)
        {
            GameManager.instance.PlayAudio(6);
            canUsePause = false;
            GetComponent<CharacterMenu>().UpdateMenu();
            StartCoroutine(PauseAllowed());
            PauseState();
        }
    }
    public void CommencePauseNoMenu()
    {
        if (canUsePause)
        {
            canUsePause = false;
            GetComponent<CharacterMenu>().UpdateMenu();
            StartCoroutine(PauseAllowedNoMenu());
            PauseStateNoMenu();
        }
    }

    IEnumerator PauseAllowed()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        canUsePause = true;
    }

    IEnumerator PauseAllowedNoMenu()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        canUsePause = true;
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

    private void PauseStateNoMenu()
    {
        if (isPaused)
        {
            ResumeGameNoMenu();
        }
        else
        {
            PauseGameNoMenu();
        }
        isPaused = !isPaused;
    }

    private void ResumeGameNoMenu()
    {
        StartCoroutine(Resuming());
    }

    private void ResumeGame()
    {
        anim.SetTrigger("hide");
        StartCoroutine(Resuming());
    }
    IEnumerator Resuming()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        gameActive = true;
        BoughtUpgrades();
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

    private void BoughtUpgrades()
    {
        // Show cost of upgrades bought after resuming
        int counter = 0;
        for (int i = upgradeStart; i < GameManager.instance.madeUpgrades.Count; i++)
        {
            if (GameManager.instance.madeUpgrades[i] == 0)
            {
                continue;
            }
            else
            {
                //new Vector3(transform.position.x, (float)(transform.position.y + (counter * 0.07)), 1)
                GameManager.instance.ShowText("-" + GameManager.instance.weaponPrices[i] + " coins", 20, Color.yellow, 
                    new Vector3(GameManager.instance.player.transform.position.x, 
                                GameManager.instance.player.transform.position.y + (float)(0.07 * counter)), Vector3.up * 40, 1.0f);
                GameManager.instance.madeUpgrades[i] = 0;
                counter++;
            }
        }
    }

    private void PauseGame()
    {
        anim.SetTrigger("show");
        StartCoroutine(Pausing());
    }
    private void PauseGameNoMenu()
    {
        StartCoroutine(Pausing());
    }

    IEnumerator Pausing()
    {
        Time.timeScale = 0;
        gameActive = false;
        yield return new WaitForSecondsRealtime(0.0f);


    }

    public void ResetTable()
    {
        for (int i = 0; i < GameManager.instance.madeUpgrades.Count; i++)
            GameManager.instance.madeUpgrades[i] = 0;
    }
}
