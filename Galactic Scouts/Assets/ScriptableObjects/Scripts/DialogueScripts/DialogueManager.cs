using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //refrences to UI objects
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject characterPortrait;
    [SerializeField] private GameObject characterName;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private enemySpawnSystem enemySpawner;

    //Components needed on objects
    private TextMeshProUGUI dialogueBoxText;
    private TextMeshProUGUI characterNameText;
    private Image characterPortraitImage;

    //Things here have to do with managing the dialogue
    public DataD startDialogue;
    public DataD currentDialogueData;
    public bool advanceCurrentDialogue = true;
    public int currentDialogueStep = 0;
    public enemySpawnSystem enemyspawnsystem;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float timeAfterTextCompletes = 5f;
    

    public gameManager gameManager;
    private void Start()
    {
        //Assign components to variables
        dialogueBoxText = dialogueBox.GetComponent<TextMeshProUGUI>();
        characterNameText = characterName.GetComponent<TextMeshProUGUI>();
        characterPortraitImage = characterPortrait.GetComponent<Image>();
        if (startDialogue != null) 
        {
            StartDialogue(startDialogue);
        }
    }
    
    //Updates the name and image then calls Couroutine that updates the text
    public void UpdateDialogue(DialogueData dialogue)
    {
        Debug.Log("Calling Update dialogue");
        dialogueCanvas.SetActive(true);
        advanceCurrentDialogue = false;
        characterNameText.text = dialogue.speakerName;
        dialogueBoxText.text = null;
        characterPortraitImage.sprite = dialogue.speakerPortrait;
        StartCoroutine(UpdateTextBox(dialogue));
    }
    //Function updates the dialogue box to create the typewriter effect
    private IEnumerator UpdateTextBox(DialogueData dialogue)
    {
        //Goes through every letter in the string
        for (int x = 0; x < dialogue.dialogue.Length; x++)
        {
            dialogueBoxText.text += dialogue.dialogue[x];
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(timeAfterTextCompletes);
        currentDialogueStep += 1;
        advanceCurrentDialogue = true;
    }
    public void StartEnemySpawner() 
    {
        enemySpawner.enabled = true;
    }
    public void StartDialogue(DataD newDialogue)
    {
        advanceCurrentDialogue = true;
        currentDialogueData = newDialogue;
        currentDialogueStep = 0;
        ShowNextLine();

        dialogueCanvas.SetActive(true);
        skipButton.SetActive(true);
        Debug.Log("Starting Conversartion");
    }
    public void ShowNextLine()
    {
        if (currentDialogueStep >= currentDialogueData.lines.Length)
        {
            EndDialogue();
            return;
        }

        characterNameText.text = currentDialogueData.lines[currentDialogueStep].characterName;
        dialogueBoxText.text = currentDialogueData.lines[currentDialogueStep].tx;
        characterPortraitImage.sprite = currentDialogueData.lines[currentDialogueStep].spr;
        currentDialogueStep++;
    }
    private void EndDialogue()
    {
        switch(currentDialogueData.lines[currentDialogueStep - 1].action)
        {
            case 1:
                enemyspawnsystem.startWaves();
                break;
        }
        dialogueCanvas.SetActive(false);
        advanceCurrentDialogue = false;
        currentDialogueData = null;
        skipButton.SetActive(false);
    }

    public void ConfigerateDialogue(DataD d) 
    {
       
        StartDialogue(d);
    }
}
