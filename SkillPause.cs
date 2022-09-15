using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillPause : MonoBehaviour
{

    public static bool isPaused = false;
    public static bool canUsePause = true;
    public static bool gameActive = true;
    private Animator anim;
    public int upgradeStart;


    private void Start()
    {
        anim = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        if (!MainMenu.inMainMenu)
        {
            if ((Input.GetKeyDown(KeyCode.Tab)
                && DialoguePause.isPaused == false && Pause.isPaused == false
                && canUsePause && Pause.canUsePause && DialoguePause.canUsePause
                && !SettingsMenu.inSettings) || 
                (Input.GetKeyDown(KeyCode.Escape) 
                && Pause.isPaused == false && DialoguePause.isPaused == false
                && canUsePause && Pause.canUsePause && DialoguePause.canUsePause && isPaused 
                && !SettingsMenu.inSettings))
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
            GameManager.instance.sMenu.UpdateSkillMenu();
            canUsePause = false;
            StartCoroutine(PauseAllowed());
            PauseState();
        }
    }

    IEnumerator PauseAllowed()
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
    IEnumerator Pausing()
    {
        Time.timeScale = 0;
        gameActive = false;
        yield return new WaitForSecondsRealtime(0.3f);
    }

    public void ResetTable()
    {
        for (int i = 0; i < GameManager.instance.madeUpgrades.Count; i++)
            GameManager.instance.madeUpgrades[i] = 0;
    }
}
