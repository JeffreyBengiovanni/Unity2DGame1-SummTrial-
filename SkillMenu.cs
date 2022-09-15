using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenu : MonoBehaviour
{
    public Skill currentSkill;
    public Image skillSprite;
    public Text skillName;
    public Text skillDescriptionText;

    public Text damageButtonText;
    public Text speedButtonText;
    public Text sizeButtonText;
    public Text projectilesButtonText;
    public Text velocityButtonText;
    public Text specialButtonText;

    public Text damageStatText;
    public Text speedStatText;
    public Text sizeStatText;
    public Text projectilesStatText;
    public Text velocityStatText;
    public Text specialStatText;

    public Button damageButton;
    public Button speedButton;
    public Button sizeButton;
    public Button projectilesButton;
    public Button velocityButton;
    public Button specialButton;

    public Button resetButton;

    public Image specialImage;
    public Text specialName;

    public Text coinText;


    public static SkillMenu objectInstance;

    public void Awake()
    {

        specialImage = transform.Find("Container").Find("SpecialSelection").Find("SpecialSprite").GetComponent<Image>();
        specialName = transform.Find("Container").Find("SpecialSelection").Find("SpecialName").GetComponent<Text>();
        
        resetButton = transform.Find("Container").Find("SkillUpgrades").Find("Reset").Find("ResetButton").GetComponent<Button>();

        coinText = transform.Find("Container").Find("CoinBox").Find("CoinText").GetComponent<Text>();

        skillSprite = transform.Find("Container").Find("SkillSelection").Find("SkillSprite").GetComponent<Image>();
        skillName = transform.Find("Container").Find("SkillSelection").Find("SkillTextID").GetComponent<Text>();
        skillDescriptionText = transform.Find("Container").Find("SkillUpgrades").Find("SkillDescriptionContainer").Find("TextContainer").Find("Description").GetComponent<Text>();

        damageButtonText = transform.Find("Container").Find("SkillUpgrades").Find("DamageUpgrade").Find("UpgradeButton").Find("UpgradeCostText").GetComponent<Text>();
        speedButtonText = transform.Find("Container").Find("SkillUpgrades").Find("CastSpeedUpgrade").Find("UpgradeButton").Find("UpgradeCostText").GetComponent<Text>();
        sizeButtonText = transform.Find("Container").Find("SkillUpgrades").Find("SizeUpgrade").Find("UpgradeButton").Find("UpgradeCostText").GetComponent<Text>();
        projectilesButtonText = transform.Find("Container").Find("SkillUpgrades").Find("ProjectilesUpgrade").Find("UpgradeButton").Find("UpgradeCostText").GetComponent<Text>();
        velocityButtonText = transform.Find("Container").Find("SkillUpgrades").Find("VelocityUpgrade").Find("UpgradeButton").Find("UpgradeCostText").GetComponent<Text>();
        specialButtonText = transform.Find("Container").Find("SkillUpgrades").Find("SpecialUpgrade").Find("UpgradeButton").Find("UpgradeCostText").GetComponent<Text>();

        damageStatText = transform.Find("Container").Find("SkillUpgrades").Find("DamageUpgrade").Find("Stat").Find("StatText").GetComponent<Text>();
        speedStatText = transform.Find("Container").Find("SkillUpgrades").Find("CastSpeedUpgrade").Find("Stat").Find("StatText").GetComponent<Text>();
        sizeStatText = transform.Find("Container").Find("SkillUpgrades").Find("SizeUpgrade").Find("Stat").Find("StatText").GetComponent<Text>();
        projectilesStatText = transform.Find("Container").Find("SkillUpgrades").Find("ProjectilesUpgrade").Find("Stat").Find("StatText").GetComponent<Text>();
        velocityStatText = transform.Find("Container").Find("SkillUpgrades").Find("VelocityUpgrade").Find("Stat").Find("StatText").GetComponent<Text>();
        specialStatText = transform.Find("Container").Find("SkillUpgrades").Find("SpecialUpgrade").Find("Stat").Find("StatText").GetComponent<Text>();

        damageButton = transform.Find("Container").Find("SkillUpgrades").Find("DamageUpgrade").Find("UpgradeButton").GetComponent<Button>();
        speedButton = transform.Find("Container").Find("SkillUpgrades").Find("CastSpeedUpgrade").Find("UpgradeButton").GetComponent<Button>();
        sizeButton = transform.Find("Container").Find("SkillUpgrades").Find("SizeUpgrade").Find("UpgradeButton").GetComponent<Button>();
        projectilesButton = transform.Find("Container").Find("SkillUpgrades").Find("ProjectilesUpgrade").Find("UpgradeButton").GetComponent<Button>();
        velocityButton = transform.Find("Container").Find("SkillUpgrades").Find("VelocityUpgrade").Find("UpgradeButton").GetComponent<Button>();
        specialButton = transform.Find("Container").Find("SkillUpgrades").Find("SpecialUpgrade").Find("UpgradeButton").GetComponent<Button>();

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

    public void SkillMenuCall()
    {
        currentSkill = GameManager.instance.currentSkill;
    }

    public void OnDamageClick()
    {
        SkillMenuCall();
        if (GameManager.instance.TryUpgradeSkill(currentSkill.GetDamagePrices(), currentSkill.GetDamageLevel()))
        {
            GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
            currentSkill.DamageUpgrade();
            GameManager.instance.hud.UpdateHUD();
            UpdateSkillMenu();
        }
    }

    public void OnSpeedClick()
    {
        SkillMenuCall();
        if (GameManager.instance.TryUpgradeSkill(currentSkill.GetSpeedPrices(), currentSkill.GetSpeedLevel()))
        {
            GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
            currentSkill.SpeedUpgrade();
            GameManager.instance.hud.UpdateHUD();
            UpdateSkillMenu();
        }
    }

    public void OnSizeClick()
    {
        SkillMenuCall();
        if (GameManager.instance.TryUpgradeSkill(currentSkill.GetSizePrices(), currentSkill.GetSizeLevel()))
        {
            GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
            currentSkill.SizeUpgrade();
            GameManager.instance.hud.UpdateHUD();
            UpdateSkillMenu();
        }
    }

    public void OnProjectilesClick()
    {
        SkillMenuCall();
        if (GameManager.instance.TryUpgradeSkill(currentSkill.GetProjectilesPrices(), currentSkill.GetProjectilesLevel()))
        {
            GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
            currentSkill.ProjectilesUpgrade();
            GameManager.instance.hud.UpdateHUD();
            UpdateSkillMenu();
        }
    }

    public void OnVelocityClick()
    {
        SkillMenuCall();
        if (GameManager.instance.TryUpgradeSkill(currentSkill.GetVelocityPrices(), currentSkill.GetVelocityLevel()))
        {
            GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
            currentSkill.VelocityUpgrade();
            GameManager.instance.hud.UpdateHUD();
            UpdateSkillMenu();
        }
    }
    public void OnSpecialClick()
    {
        SkillMenuCall();
        if (GameManager.instance.TryUpgradeSkill(currentSkill.GetSpecialPrices(), currentSkill.GetSpecialLevel()))
        {
            GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
            currentSkill.SpecialUpgrade();
            GameManager.instance.hud.UpdateHUD();
            UpdateSkillMenu();
        }
    }


    // Skill Selection
    public void OnArrowClick(bool right)
    {
        GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
        SkillMenuCall();
        if (right)
        {
            GameManager.instance.AddSkillIndex();
        }
        else
        {
            GameManager.instance.SubtractSkillIndex();
        }
        UpdateSkillMenu();
    }


    // Reset Skill
    public void OnResetClick()
    {
        GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
        GameManager.instance.ResetSkill(GameManager.instance.currentSkillIndex);
        UpdateSkillMenu();
    }

    public void ResetButtonToggle()
    {
        if(currentSkill.GetDamageLevel() == 0 &&
            currentSkill.GetSpeedLevel() == 0 &&
            currentSkill.GetSizeLevel() == 0 &&
            currentSkill.GetProjectilesLevel() == 0 &&
            currentSkill.GetVelocityLevel() == 0 &&
            currentSkill.GetSpeedLevel() == 0)
        {
            resetButton.interactable = false;
        } 
        else
        {
            resetButton.interactable = true;
        }
    }


    public void UpdateSkillMenu()
    {
        SkillMenuCall();
        ResetButtonToggle();
        skillSprite.sprite = GameManager.instance.holdingSprites[GameManager.instance.currentSkillIndex];
        skillName.text = GameManager.instance.currentSkill.GetName();
        skillDescriptionText.text = GameManager.instance.currentSkill.GetDescription();

        coinText.text = "COINS: " + GameManager.instance.coins.ToString();

        
        specialImage.sprite = GameManager.instance.specialSprites[GameManager.instance.specialManager.currentSkillindex];
        specialName.text = GameManager.instance.specialManager.names[GameManager.instance.specialManager.currentSkillindex];

        if (GameManager.instance.currentSkill.GetDamagePrices().Count <= GameManager.instance.currentSkill.GetDamageLevel())
        {
            damageButton.interactable = false;
            damageButtonText.text = "MAX";
        }
        else
        {
            damageButton.interactable = true;
            damageButtonText.text = GameManager.instance.currentSkill.GetDamagePrices()[GameManager.instance.currentSkill.GetDamageLevel()].ToString();

        }
        damageStatText.text = GameManager.instance.currentSkill.GetDamage().ToString();

        if (GameManager.instance.currentSkill.GetSpeedPrices().Count <= GameManager.instance.currentSkill.GetSpeedLevel())
        {
            speedButton.interactable = false;
            speedButtonText.text = "MAX";
        }
        else
        {
            speedButton.interactable = true;
            speedButtonText.text = GameManager.instance.currentSkill.GetSpeedPrices()[GameManager.instance.currentSkill.GetSpeedLevel()].ToString();

        }
        speedStatText.text = GameManager.instance.currentSkill.GetSpeed().ToString("F2") + "s";


        if (GameManager.instance.currentSkill.GetSizePrices().Count <= GameManager.instance.currentSkill.GetSizeLevel())
        {
            sizeButton.interactable = false;
            sizeButtonText.text = "MAX";
        }
        else
        {
            sizeButton.interactable = true;
            sizeButtonText.text = GameManager.instance.currentSkill.GetSizePrices()[GameManager.instance.currentSkill.GetSizeLevel()].ToString();
        }
        sizeStatText.text = GameManager.instance.currentSkill.GetSize().ToString();


        if (GameManager.instance.currentSkill.GetProjectilesPrices().Count <= GameManager.instance.currentSkill.GetProjectilesLevel())
        {
            projectilesButton.interactable = false;
            projectilesButtonText.text = "MAX";
        }
        else
        {
            projectilesButton.interactable = true;
            projectilesButtonText.text = GameManager.instance.currentSkill.GetProjectilesPrices()[GameManager.instance.currentSkill.GetProjectilesLevel()].ToString();
        }
        projectilesStatText.text = GameManager.instance.currentSkill.GetProjectiles().ToString();


        if (GameManager.instance.currentSkill.GetVelocityPrices().Count <= GameManager.instance.currentSkill.GetVelocityLevel())
        {
            velocityButton.interactable = false;
            velocityButtonText.text = "MAX";
        }
        else
        {
            velocityButton.interactable = true;
            velocityButtonText.text = GameManager.instance.currentSkill.GetVelocityPrices()[GameManager.instance.currentSkill.GetVelocityLevel()].ToString();
        }
        velocityStatText.text = (100f * GameManager.instance.currentSkill.GetVelocity()).ToString() + "%";


        if (GameManager.instance.currentSkill.GetSpecialPrices().Count <= GameManager.instance.currentSkill.GetSpecialLevel())
        {
            specialButton.interactable = false;
            specialButtonText.text = "MAX";
        }
        else
        {
            specialButton.interactable = true;
            specialButtonText.text = GameManager.instance.currentSkill.GetSpecialPrices()[GameManager.instance.currentSkill.GetSpecialLevel()].ToString();
        }
        specialStatText.text = GameManager.instance.currentSkill.GetSpecial().ToString("F2");


        if (GameManager.instance.currentSkill.GetDamageLevel() == 0)
        {
            damageStatText.color = Color.white;
        }
        else
        {
            damageStatText.color = Color.cyan;
        }

        if (GameManager.instance.currentSkill.GetSpeedLevel() == 0)
        {
            speedStatText.color = Color.white;
        }
        else
        {
            speedStatText.color = Color.cyan;
        }

        if (GameManager.instance.currentSkill.GetSizeLevel() == 0)
        {
            sizeStatText.color = Color.white;
        }
        else
        {
            sizeStatText.color = Color.cyan;
        }

        if (GameManager.instance.currentSkill.GetProjectilesLevel() == 0)
        {
            projectilesStatText.color = Color.white;
        }
        else
        {
            projectilesStatText.color = Color.cyan;
        }

        if (GameManager.instance.currentSkill.GetVelocityLevel() == 0)
        {
            velocityStatText.color = Color.white;
        }
        else
        {
            velocityStatText.color = Color.cyan;
        }

        if (GameManager.instance.currentSkill.GetSpecialLevel() == 0)
        {
            specialStatText.color = Color.white;
        } 
        else
        {
            specialStatText.color = Color.cyan;
        }

    }
}