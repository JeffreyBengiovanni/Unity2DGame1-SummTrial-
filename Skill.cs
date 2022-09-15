using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public List<int> damageChart;
    public List<int> damagePrices;
    public List<float> speedChart;
    public List<int> speedPrices;
    public List<int> sizeChart;
    public List<int> sizePrices;
    public List<int> projectilesChart;
    public List<int> projectilesPrices;
    public List<float> velocityChart;
    public List<int> velocityPrices;
    public List<float> specialChart;
    public List<int> specialPrices;
    public int damageLevel;
    public int speedLevel;
    public int sizeLevel;
    public int projectilesLevel;
    public int velocityLevel;
    public int specialLevel;
    public float castSpeed;
    public float scale;
    public float velocity;
    public Sprite skillSprite;
    public string skillName;
    public string skilLDescription;
    public float duration;
    public int unlocked;
    public float baseKnockback;
    public int pierce;
    public int projectiles;
    public float windup;

    // Damage
    public void SetDamageLevel(int x)
    {
        damageLevel = x;
    }
    public void DamageUpgrade()
    {
        damageLevel++;
    }
    public int GetDamageLevel()
    {
        return damageLevel;
    }

    public List<int> GetDamagePrices()
    {
        return damagePrices;
    }

    public int GetDamage()
    {
        if (GameManager.instance.player.isBerserk)
        {
            return Mathf.RoundToInt(damageChart[damageLevel] * 1.20f);

        }
        return damageChart[damageLevel];
    }


    // Cast Speed
    public void SetSpeedLevel(int x)
    {
        speedLevel = x;
    }
    public void SpeedUpgrade()
    {
        speedLevel++;
    }
    public int GetSpeedLevel()
    {
        return speedLevel;
    }

    public List<int> GetSpeedPrices()
    {
        return speedPrices;
    }

    public float GetSpeed()
    {
        if (GameManager.instance.player.isBerserk)
        {
            return (float)(castSpeed * (1f - speedChart[speedLevel]) * .8f);
        }
        return (float) (castSpeed * (1f - speedChart[speedLevel]));
    }


    // Size (NOW PIERCE)
    public void SetSizeLevel(int x)
    {
        sizeLevel = x;
    }
    public void SizeUpgrade()
    {
        sizeLevel++;
    }
    public int GetSizeLevel()
    {
        return sizeLevel;
    }

    public List<int> GetSizePrices()
    {
        return sizePrices;
    }

    public int GetSize()
    {
        pierce = sizeChart[sizeLevel];
        return pierce;
    }
    public int GetPierce()
    {
        pierce = sizeChart[sizeLevel];
        return pierce;
    }


    // Projectiles
    public void SetProjectilesLevel(int x)
    {
        projectilesLevel = x;
        projectiles = projectilesChart[projectilesLevel];
    }
    public void ProjectilesUpgrade()
    {
        projectilesLevel++;
        projectiles = projectilesChart[projectilesLevel];
    }
    public int GetProjectilesLevel()
    {
        projectiles = projectilesChart[projectilesLevel];
        return projectilesLevel;
    }

    public List<int> GetProjectilesPrices()
    {
        projectiles = projectilesChart[projectilesLevel];
        return projectilesPrices;
    }

    public float GetProjectiles()
    {
        projectiles = projectilesChart[projectilesLevel];
        return projectilesChart[projectilesLevel];
    }


    // Velocity
    public void SetVelocityLevel(int x)
    {
        velocityLevel = x;
    }
    public void VelocityUpgrade()
    {
        velocityLevel++;
    }
    public int GetVelocityLevel()
    {
        return velocityLevel;
    }

    public List<int> GetVelocityPrices()
    {
        return velocityPrices;
    }

    public float GetVelocity() 
    {
        if (GameManager.instance.player.isBerserk)
        {
            return (float)(velocity + velocityChart[velocityLevel] * 1.20);

        }
        return (float)(velocity + velocityChart[velocityLevel]);
    }


    // Special (KNOCKBACK)
    public void SetSpecialLevel(int x)
    {
        specialLevel = x;
    }
    public void SpecialUpgrade()
    {
        specialLevel++;
    }
    public int GetSpecialLevel()
    {
        return specialLevel;
    }

    public List<int> GetSpecialPrices()
    {
        return specialPrices;
    }

    public virtual float GetSpecial()
    {
        return (float)(baseKnockback + (specialChart[specialLevel]));
    }

    // Unlock Skill

    public void SetUnlocked (int x)
    {
        unlocked = x;
    }

    public bool IsUnlocked()
    {
        return (unlocked == 1 || unlocked == 1f);
    }

    // Windup
    public float GetWindup()
    {
        return windup;
    }

    // Sprite
    public Sprite GetSprite()
    {
        return skillSprite;
    }

    // Name
    public string GetName()
    {
        return skillName;
    }

    // Description 
    public string GetDescription()
    {
        return skilLDescription;
    }
}
