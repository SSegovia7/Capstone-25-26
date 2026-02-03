using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    [Header("References")]
    public DataD dialogue;
    public GameObject dialoguePanel;
    public DataD dialogueDemo;

    [Header("UI")]
    public TextMeshProUGUI txName;
    public TextMeshProUGUI txDialogue;
    public Image imageCharacter;


    private int currentLine = 0;
    public bool isDialogueActive = false;
    public enemySpawnSystem spawn;
    public void InitializeDialogue(DataD d)
    {
        StartDialogue(d);
    }

    public void OnSkipDialogue()
    {
        if (isDialogueActive)
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(DataD newDialogue)
    {
        isDialogueActive = true;
        dialogue = newDialogue;
        currentLine = 0;
        ShowNextLine();

        dialoguePanel.SetActive(true);
    }
    private void ShowNextLine()
    {
        if (currentLine >= dialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        txName.text = dialogue.lines[currentLine].characterName;
        txDialogue.text = dialogue.lines[currentLine].tx;
        imageCharacter.sprite = dialogue.lines[currentLine].spr;
        currentLine++;
    }
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        dialogue = null;
    }

    private void Start()
    {
        InitializeDialogue(dialogueDemo);
    }

    public void CheckAction(int a)
    {
        switch (a) 
        {
            case 0:
                Debug.Log("We dont have action");
                break;
            case 1:
                Debug.Log("We dont have action");
                break;
            
        }
    }
}
