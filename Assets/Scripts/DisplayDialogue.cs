using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class DisplayDialogue : MonoBehaviour
{
    public GameObject pressEButton;
    public BoxCollider2D merchColl;
    public GameObject DialogueCanvas;
    public Text dialogueText;
    public float letterDelay = 0.01f;
    public bool messageDisplayed = false;
    public DarkKingMovement darkKing;
    private bool isDialogueStarted = false;
    public GameObject pressEtoInteractDisplay;

    private void Start()
    {
        pressEButton.SetActive(false);
        DialogueCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !messageDisplayed)
        {
            Debug.Log("deneme");
            pressEButton.SetActive(true);
            pressEtoInteractDisplay.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressEButton.SetActive(false);
            pressEtoInteractDisplay.SetActive(false);
        }
    }

    void StartDialogue()
    {
        // Time.timeScale = 0;
        darkKing.enabled = false;
        DialogueCanvas.SetActive(true);
        if (!isDialogueStarted)
        {
            StartCoroutine(DisplayTextLetterByLetter());
            isDialogueStarted = true;
        }
    }

    IEnumerator DisplayTextLetterByLetter()
    {
        dialogueText.text = ""; // Clear the text initially
        string originalText = "In a prosperous yet oblivious world, corrupt individuals inadvertently empower the Heart Seeker, leading to the enslavement of humanity on \"doomsday.\"  \nThe resilient Blood King becomes humanity's final beacon of hope, defying the Heart Seeker's control and absorbing his power to restore the world. \npress 'E' to skip";
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < originalText.Length; i++)
        {
            stringBuilder.Append(originalText[i]);
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("LetterSound");
            }
            dialogueText.text = stringBuilder.ToString();
            yield return new WaitForSeconds(letterDelay);
        }
        messageDisplayed = true;
    }

    void Update()
    {
        if (pressEButton.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AudioManager audioManager = FindObjectOfType<AudioManager>();
                if (audioManager != null)
                {
                    audioManager.Play("MerchantInterract");
                }
                StartDialogue();
                if (Input.GetKeyDown(KeyCode.E) && messageDisplayed)
                {
                    DialogueCanvas.SetActive(false);
                    darkKing.enabled = true;
                    pressEtoInteractDisplay.SetActive(false);
                }
            }
        }
    }
}