using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //refrences to UI objects
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject characterPortrait;
    [SerializeField] private GameObject characterName;

    //Components needed on objects
    private TextMeshProUGUI dialogueBoxText;
    private TextMeshProUGUI characterNameText;
    private Image characterPortraitImage;

    public DialogueData[] currentDialogueData;
    private float typingSpeed = 0.05f;

    private void Start()
    {
        dialogueBoxText = dialogueBox.GetComponent<TextMeshProUGUI>();
        characterNameText = characterName.GetComponent<TextMeshProUGUI>();
        characterPortraitImage = characterPortrait.GetComponent<Image>();
    }
    //Function updates the dialogue box
    public IEnumerator updateTextBox(DialogueData dialogue)
    {
        //Goes for every letter in the string
        for (int x = 0; x <= dialogue.dialogue.Length; x++)
        {
            dialogueBoxText.text += dialogue.dialogue[x];
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
