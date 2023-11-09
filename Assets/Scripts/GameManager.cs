using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private Animator titleCards;

    private bool gameOver = false;

    private void Start()
    {
        instance = this;

        SoundManager.PlayMusic("Theme");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
    }

    public static void Victory()
    {
        if (!instance.gameOver)
        {
            instance.gameOver = true;

            instance.titleCards.SetTrigger("Victory");
            SoundManager.PlayMusic("Victory");
        }
    }

    public static void Failure()
    {
        if (!instance.gameOver)
        {
            instance.gameOver = true;

            instance.titleCards.SetTrigger("Failure");
            SoundManager.PlayMusic("Failure");
        }
    }
}
