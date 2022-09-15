using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, coinText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;
    public static CharacterMenu objectInstance;
    public Button saveButton;

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
    // Character Selection
    public void OnArrowClick(bool right)
    {
        GameManager.instance.PlayAudio(6);
        if (right)
        {
            currentCharacterSelection++;

            // If we are out of bounds
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            // If we are out of bounds
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
        }
    }

    public void Update()
    {
        if (GameManager.instance.spawnerManager.waveOver)
        {
            saveButton.interactable = true;
        }
        else
        {
            saveButton.interactable = false;
        }
    }

    public void OnSaveClick()
    {
        GameManager.instance.PlayAudio(5);
        Debug.Log("Saving");
        GameManager.instance.SaveState();
    }
    public void OnSettingsClick()
    {
        GameManager.instance.PlayAudio(6);
        Debug.Log("Settings");
        GameManager.instance.OpenSettings();
    }

    public void OnMainMenuClick()
    {
        GameManager.instance.PlayAudio(6);
        if (GameManager.instance.spawnerManager.waveOver)
        {
            GameManager.instance.SaveState();
        } 
        else
        {
            GameManager.instance.spawnerManager.canceled = true;
            GameManager.instance.spawnerManager.waveOver = true;
        }
        GameManager.instance.mapSelect.UpdateMapSelect();
        GameManager.instance.pause.MainMenuResume(true);
        GameManager.instance.mainMenu.ShowMainMenu();
        GameManager.instance.LoadGM("MainMenu");
    }

    public void OnQuitClick()
    {
        GameManager.instance.PlayAudio(6);
        if (GameManager.instance.spawnerManager.waveOver)
        {
            GameManager.instance.SaveState();
        }
        else
        {
            GameManager.instance.spawnerManager.canceled = true;
            GameManager.instance.spawnerManager.waveOver = true;
        }
        Application.Quit();
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
        GameManager.instance.preferredSkin = currentCharacterSelection;
    }

    // NOT DONE:
    /*public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }*/

    public void UpdateMenu()
    {

        // Character Image
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[GameManager.instance.preferredSkin];

       /* // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
*/

        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitpoint.ToString() + " / " + GameManager.instance.player.maxHitpoint.ToString();
        coinText.text = GameManager.instance.coins.ToString();

        // XP Bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if(currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXP = GameManager.instance.GetXPToLevel(currLevel - 1);
            int currLevelXP = GameManager.instance.GetXPToLevel(currLevel);

            int diff = currLevelXP - prevLevelXP;
            int currXPIntoLevel = GameManager.instance.experience - prevLevelXP;

            float completionRatio = (float)currXPIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXPIntoLevel.ToString() + " / " + diff;

        } 
    }
}
