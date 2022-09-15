using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public FloatingText backdrop;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            if (!active)
                return;

            if (GameManager.instance.player.alive == false)
                Hide();

            if (Time.time - lastShown > duration)
                Hide();

            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (float)(Time.deltaTime / (duration * 1.5)));

            go.transform.position += motion * Time.deltaTime;

        }
    }
}
