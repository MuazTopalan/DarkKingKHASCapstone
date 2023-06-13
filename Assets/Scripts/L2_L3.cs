using UnityEngine;
using UnityEngine.SceneManagement;

public class L2_L3 : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(3);
            FindObjectOfType<AudioManager>().Play("MerchantInterract");
        }
    }
}



