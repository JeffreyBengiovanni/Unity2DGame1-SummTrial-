using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    public Font f;
    public static FloatingTextManager objectInstance;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Awake()
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
    private void Start()
    {
        f = GameManager.instance.f1;
    }
    private void Update()
    {
        if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            foreach (FloatingText txt in floatingTexts)
                txt.UpdateFloatingText();
        }
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {

        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.font = f;
        floatingText.txt.color = color;
        Vector3 spot = Camera.main.WorldToScreenPoint(position); // Transfer world space to screen space so we can use it in the UI
        floatingText.go.transform.position = new Vector3(spot.x + Random.Range(-40f, 40f), spot.y + Random.Range(-40f, 40f));
        floatingText.motion = motion;
        floatingText.duration = duration;
        floatingText.Show();

    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }

}
