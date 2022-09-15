using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : MonoBehaviour
{

    private Animator anim, animBox;
    private string talker;
    private string[] lines;
    private int index;
    private int maxLines;
    private Text speaker;
    private Text dialogue;
    public bool inDialogue = false;
    public bool once = false, canClick = false;

    public static DialogueMenu objectInstance;


    public void Awake()
    {
        speaker = transform.Find("Container").Find("NameBackground").Find("TalkerName").GetComponent<Text>();
        dialogue = transform.Find("Container").Find("TalkBackground").Find("TalkText").GetComponent<Text>();


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
        animBox = transform.Find("Container").GetComponent<Animator>();
    }

    public void Update()
    {

        if (inDialogue)
        {
            if (once)
            {
                index = maxLines + 1;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (canClick)
                {
                    canClick = false;
                    StartCoroutine(Stall());
                    UpdateMenu();
                }
            }
        } 

    }

    public IEnumerator Stall()
    {
        yield return new WaitForSecondsRealtime(.3f);
        canClick = true;
    }

    public void OpenMenu(string name, string[] speech, int currentLine = 0, bool oneTime = false)
    {
        GameManager.instance.dPause.CommencePause();
        animBox.SetTrigger("show");
        maxLines = speech.Length;
        inDialogue = true;
        canClick = false;

        talker = name;
        lines = speech;
        index = currentLine;
        once = oneTime;
        speaker.text = talker;
        dialogue.text = lines[index];
        index++;
        StartCoroutine(Stall());

    }

    public void UpdateMenu()
    {
        if (index >= maxLines)
        {
            CloseMenu();
        } 
        else
        {
            speaker.text = talker;
            dialogue.text = lines[index];
            index++;
        }

    }

    public void CloseMenu()
    {
        canClick = false;
        inDialogue = false;
        animBox.SetTrigger("hide");
        GameManager.instance.dPause.CommencePause();
    }

}
