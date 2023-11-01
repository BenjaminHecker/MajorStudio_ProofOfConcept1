using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("Slash Attack")]
    [SerializeField] private SlashAttack slashAttack;
    [SerializeField] private float slashAttackDelay = 0.5f;
    [SerializeField] private float slashAttackCooldown = 2f;

    private bool slashAttackOnCooldown = false;
    public bool SlashAttackReady { get { return !slashAttackOnCooldown; } }

    public void SlashAttack()
    {
        StartCoroutine(SlashAttackRoutine());
        SoundManager.PlayMisc("Slash Attack");
    }

    private IEnumerator SlashAttackRoutine()
    {
        slashAttackOnCooldown = true;

        yield return new WaitForSeconds(slashAttackDelay);

        slashAttack.Trigger();

        yield return new WaitForSeconds(slashAttackCooldown);

        slashAttackOnCooldown = false;

        PlayerController.freezeCharacterDirection = false;
    }
}
