using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //refrences to UI objects
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject characterPortrait;
    [SerializeField] private GameObject characterName;

    //Components needed on objects
    private TextMeshProUGUI dialogueBoxText;
    private TextMeshProUGUI characterNameText;
    private Image characterPortraitImage;

    //Things here have to do with managing the dialogue
    public DialogueData[] currentDialogueData;
    private bool advanceCurrentDialogue = true;
    private int currentDialogueStep = 0;

    private float typingSpeed = 0.05f;
    private float timeAfterTextCompletes = 5f;

    public gameManager gameManager;
    private void Start()
    {
        //Assign components to variables
        dialogueBoxText = dialogueBox.GetComponent<TextMeshProUGUI>();
        characterNameText = characterName.GetComponent<TextMeshProUGUI>();
        characterPortraitImage = characterPortrait.GetComponent<Image>();
    }
    private void FixedUpdate()
    {
        //Advances dialogue automatically if there is dialogue data to run
        if (currentDialogueData != null && advanceCurrentDialogue)
        {
            if (currentDialogueStep + 1 <= currentDialogueData.Length)
            {
                UpdateDialogue(currentDialogueData[currentDialogueStep]);
            }
            else
            {
                //If at the end of the list, removes data and resets counter
                currentDialogueStep = 0;
                currentDialogueData = null;
                dialogueCanvas.SetActive(false);
                gameManager.EndDialogue();
            }
        }
    }
    //Updates the name and image then calls Couroutine that updates the text
    public void UpdateDialogue(DialogueData dialogue)
    {
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
}
