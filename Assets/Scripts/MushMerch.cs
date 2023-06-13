using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class MushMerch : MonoBehaviour
{
    public GameObject pressEButton;
    public BoxCollider2D merchColl;
    public GameObject pressEtoInteractDisplay;

    private void Start()
    {
        pressEButton.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
                    audioManager.Stop("OST");
                    SceneManager.LoadScene(4);
                    audioManager.Play("MenuOST");
                }
            }
        }
    }
}