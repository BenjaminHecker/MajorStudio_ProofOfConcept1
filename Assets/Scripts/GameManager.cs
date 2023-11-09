using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private Animator titleCards;

    [Header("Controls UI")]
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;
    [SerializeField] private Image jumpImg;
    [SerializeField] private Image slashImg;
    [SerializeField] private Image dashImg;
    [SerializeField] private Image rangeImg;

    private bool gameOver = false;

    private void Start()
    {
        instance = this;

        SoundManager.PlayMusic("Theme");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);

        UpdateJumpUI(PlayerController.instance.grounded);
        UpdateSlashUI(PlayerController.instance.attackManager.SlashAttackReady);
        UpdateDashUI(PlayerController.instance.attackManager.DashSpecialReady);
        UpdateRangeUI(PlayerController.instance.attackManager.RangeSpecialReady);
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

    private void UpdateJumpUI(bool enabled)
    {
        jumpImg.color = enabled ? enabledColor : disabledColor;
    }

    private void UpdateSlashUI(bool enabled)
    {
        slashImg.color = enabled ? enabledColor : disabledColor;
    }

    private void UpdateDashUI(bool enabled)
    {
        dashImg.color = enabled ? enabledColor : disabledColor;
    }

    private void UpdateRangeUI(bool enabled)
    {
        rangeImg.color = enabled ? enabledColor : disabledColor;
    }
}
