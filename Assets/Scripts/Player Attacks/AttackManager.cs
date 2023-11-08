using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private PlayerController player;

    [Header("Slash Attack")]
    [SerializeField] private SlashAttack slashAttack;
    [SerializeField] private float slashAttackDelay = 0.5f;
    [SerializeField] private float slashAttackCooldown = 2f;

    private bool slashAttackOnCooldown = false;
    public bool SlashAttackReady { get { return !slashAttackOnCooldown; } }

    [Header("Dash Special")]
    [SerializeField] private float dashSpecialDelay = 0f;
    [SerializeField] private float dashSpecialCooldown = 1f;

    [Space]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    private bool dashSpecialOnCooldown = false;
    public bool DashSpecialReady
    {
        get
        {
            return !dashSpecialOnCooldown && player.ringManager.CompleteRingsCount >= 1;
        }
    }

    [Header("Range Special")]
    [SerializeField] private RangeSpecial rangeSpecial;
    [SerializeField] private float rangeSpecialDelay;
    [SerializeField] private float rangeSpecialCooldown;

    private bool rangeSpecialOnCooldown = false;
    public bool RangeSpecialReady
    {
        get
        {
            return !rangeSpecialOnCooldown && player.ringManager.CompleteRingsCount >= 2;
        }
    }

    public void Setup(PlayerController playerController)
    {
        player = playerController;

        rangeSpecial.Setup(player);
    }

    public void SlashAttack()
    {
        StartCoroutine(SlashAttackRoutine());
        SoundManager.PlayMisc("Slash Attack");
    }

    private IEnumerator SlashAttackRoutine()
    {
        slashAttackOnCooldown = true;
        player.freezeCharacterDirection = true;

        yield return new WaitForSeconds(slashAttackDelay);

        slashAttack.Trigger();

        yield return new WaitForSeconds(slashAttackCooldown);

        slashAttackOnCooldown = false;
        player.freezeCharacterDirection = false;
    }

    public void DashSpecial()
    {
        StartCoroutine(DashSpecialRoutine());
    }

    private IEnumerator DashSpecialRoutine()
    {
        dashSpecialOnCooldown = true;
        player.freezeCharacterDirection = true;

        yield return new WaitForSeconds(dashSpecialDelay);

        float dashTimer = 0f;

        while (dashTimer < dashDuration)
        {
            if (player.FacingRight)
                player.transform.position += Vector3.right * dashSpeed * Time.deltaTime;
            else
                player.transform.position += Vector3.left * dashSpeed * Time.deltaTime;

            dashTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(dashSpecialCooldown);

        dashSpecialOnCooldown = false;
        player.freezeCharacterDirection = false;
    }

    public void RangeSpecial()
    {
        StartCoroutine(RangeSpecialRoutine());
    }

    private IEnumerator RangeSpecialRoutine()
    {
        rangeSpecialOnCooldown = true;
        player.freezeCharacterDirection = true;

        yield return new WaitForSeconds(rangeSpecialDelay);

        rangeSpecial.Trigger();

        yield return new WaitForSeconds(rangeSpecialCooldown);

        rangeSpecialOnCooldown = false;
        player.freezeCharacterDirection = false;
    }
}
