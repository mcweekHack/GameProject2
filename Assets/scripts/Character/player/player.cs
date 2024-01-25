using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Status_machine;
public class player : Character
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D col;
    [SerializeField] playerCon controller;
    [SerializeField] float MoveSpeed;
    [SerializeField] Animi_Con anime_mach;
    [SerializeField] SpriteRenderer sprend;
    Vector2 tmp2 = Vector2.zero;
    Vector3 tmp3 = Vector3.one;
    [SerializeField] Status playerStatus;
    [SerializeField] float JumpForce;
    [SerializeField] IsGround groundcheck;
    [SerializeField] float RollSpeed;
    [SerializeField] float RollTime;
    [SerializeField] float TimeTag;
    [SerializeField] float TimeGap;
    [SerializeField] float FaceDir;
    [SerializeField] float Attack3Move;

    Coroutine FallCoro;
    Coroutine MoveCoro;
    Coroutine RollCoro;
    Coroutine AttackCoro;
    Coroutine defendCoro;
    Coroutine hurtCoro;

    protected override void OnEnable()
    {
        base.OnEnable();
        FaceDir = 1f;
        controller.onMove += OnPlayerMove;
        controller.onMove += FaceDirCheck;
        controller.stopMove += StopPlayerMove;
        controller.stopMove += FaceDirCheck;
        controller.onJump += OnJump;
        controller.onRoll += OnRoll;
        controller.onAttack += OnAttack;
        controller.onDefend += OnDefend;
        controller.stopDefend += StopDefend;
        if (FallCoro != null)
            StopCoroutine(FallCoro);
        FallCoro = StartCoroutine(CheckFallCoroutine());
        playerStatus = Status.Idle;
    }
    void OnDisable()
    {
        controller.onMove -= OnPlayerMove;
        controller.stopMove -= StopPlayerMove;
        controller.onRoll -= OnRoll;
        controller.onJump -= OnJump;
        controller.onAttack -= OnAttack;
        controller.onDefend -= OnDefend;
        controller.stopDefend -= StopDefend;
        if (FallCoro != null)
            StopCoroutine(FallCoro);
    }
    ////////////////////////////////////////////////
    void FaceDirCheck()
    {
        FaceDir = transform.localScale.x < 0 ? -1f : 1f;
    }
    void FaceDirCheck(Vector2 Movedir)
    {
        FaceDir = Movedir.x < 0 ? -1f : 1f;
    }
    void OnPlayerMove(Vector2 Movedir)
    {
        if (Status_Mach.IfCanTransit(playerStatus,Status.Move))
        {
            tmp2.y = 0f;
            tmp2.x = Movedir.x;
            tmp3 = Vector3.one;
            rb.velocity = tmp2.normalized * MoveSpeed;
            playerStatus = Status.Move;
            tmp3.x = rb.velocity.x > 0 ? 1 : -1;
            transform.localScale = tmp3;
            anime_mach.playAnime(Status.Move);
            if (MoveCoro != null)
                StopCoroutine(MoveCoro);
            MoveCoro = StartCoroutine(MoveCoroutine());
        }

    }
    void StopPlayerMove()
    {
        if (MoveCoro != null)
            StopCoroutine(MoveCoro);
        if(playerStatus == Status.Move)
        {
            playerStatus = Status.Idle;
            rb.velocity = Vector2.zero;
            anime_mach.playAnime(Status.Idle);
        }
    }
    void OnJump()
    {
        if (Status_Mach.IfCanTransit(playerStatus, Status.jump)&& groundcheck.IsOnGround())
        {
            playerStatus = Status.jump;
            rb.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
            anime_mach.playAnime(Status.jump);
        }
    }
    void OnRoll()
    {
        if(Status_Mach.IfCanTransit(playerStatus, Status.Roll))
        {
            playerStatus = Status.Roll;
            anime_mach.playAnime(Status.Roll);
            tmp2.y = 0;
            tmp2.x = FaceDir;
            tmp3 = Vector3.one;
            tmp3.x = FaceDir;
            transform.localScale = tmp3;
            rb.velocity = tmp2.normalized * RollSpeed;
            if (RollCoro != null)
                StopCoroutine(RollCoro);
            RollCoro = StartCoroutine(RollCoroutine());
        }
    }
    void OnAttack()
    {
        if (Status_Mach.IfCanTransit(playerStatus, Status.attack1))
        {
            rb.velocity = Vector2.zero;
            if (AttackCoro != null)
                StopCoroutine(AttackCoro);
            AttackCoro = StartCoroutine(AttackCoroutine());
        }
        else if(playerStatus == Status.attack1 || playerStatus == Status.attack2)
            TimeGap = TimeTag;
    }
    void OnDefend()
    {
        if(Status_Mach.IfCanTransit(playerStatus, Status.defend))
        {
            playerStatus = Status.defend;
            anime_mach.playAnime(Status.defend);
            rb.velocity = Vector2.zero;
            if (defendCoro != null)
                StopCoroutine(defendCoro);
            defendCoro = StartCoroutine(DefendCoroutine());
        }
    }
    void StopDefend()
    {
        if(playerStatus == Status.defend)
        {
            playerStatus = Status.Idle;
            rb.velocity = Vector2.zero;
            anime_mach.playAnime(Status.Idle);
        }
    }
    public override void TakeDamage(float damage)
    {
        if (Health <= damage)
        {
            if(Status_Mach.IfCanTransit(playerStatus, Status.death))
            {
                playerStatus = Status.death;
                rb.velocity = Vector2.zero;
                anime_mach.playAnime(Status.death);
            }
        }
        else
        {
            if(Status_Mach.IfCanTransit(playerStatus, Status.hurt))
            {
                Health -= damage;
                playerStatus = Status.hurt;
                rb.velocity = Vector2.zero;
                anime_mach.playAnime(Status.hurt);
                if (hurtCoro != null)
                    StopCoroutine(hurtCoro);
                hurtCoro = StartCoroutine(HurtCoroutine());
            }
        }
    }






   
    IEnumerator CheckFallCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => rb.velocity.y < 0&&!groundcheck.IsOnGround());
            if(Status_Mach.IfCanTransit(playerStatus, Status.Fall))
            {
                playerStatus = Status.Fall;
                anime_mach.playAnime(Status.Fall);
                yield return new WaitUntil(() => groundcheck.IsOnGround());
                if (playerStatus == Status.Fall)
                {
                    playerStatus = Status.Idle;
                    anime_mach.playAnime(Status.Idle);
                    rb.velocity = Vector2.zero;
                }
            }
            yield return null;
        }
    }
    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (Status_Mach.IfCanTransit(playerStatus, Status.Move))
            {
                tmp2.y = 0f;
                tmp2.x = transform.localScale.x;
                rb.velocity = tmp2.normalized * MoveSpeed;
                playerStatus = Status.Move;
                anime_mach.playAnime(Status.Move);
            }
            yield return null;
        }
    }
    IEnumerator RollCoroutine()
    {
        yield return new WaitForSeconds(RollTime);
        if(playerStatus == Status.Roll)
        {
            playerStatus = Status.Idle;
            anime_mach.playAnime(Status.Idle);
            rb.velocity = Vector2.zero;
        }
    }
    IEnumerator AttackCoroutine()
    {
        TimeTag = 0f;
        TimeGap = 0f;
        playerStatus = Status.attack1;
        anime_mach.playAnime(Status.attack1);
        while (TimeTag < 0.5f)
        {
            TimeTag += Time.deltaTime;
            yield return null;
        }
        if(TimeGap<0.25f|| TimeGap > 0.5f)
        {
            if (playerStatus == Status.attack1)
            {
                playerStatus = Status.Idle;
                anime_mach.playAnime(Status.Idle);
            }
            yield break;
        }
        TimeTag = 0f;
        TimeGap = 0f;
        playerStatus = Status.attack2;
        anime_mach.playAnime(Status.attack2);
        while (TimeTag < 0.5f)
        {
            TimeTag += Time.deltaTime;
            yield return null;
        }
        if (TimeGap < 0.25f || TimeGap > 0.5f)
        {
            if (playerStatus == Status.attack2)
            {
                playerStatus = Status.Idle;
                anime_mach.playAnime(Status.Idle);
            }
            yield break;
        }
        TimeTag = 0f;
        TimeGap = 0f;
        playerStatus = Status.attack3;
        anime_mach.playAnime(Status.attack3);
        transform.Translate(Attack3Move*transform.localScale.x, 0, 0);
        while (TimeTag < 1f)
        {
            TimeTag += Time.deltaTime;
            yield return null;
        }
        if (playerStatus == Status.attack3)
        {
            playerStatus = Status.Idle;
            anime_mach.playAnime(Status.Idle);
        }
    }
    IEnumerator DefendCoroutine()
    {
        yield break;
    }
    IEnumerator HurtCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if(playerStatus == Status.hurt)
        {
            playerStatus = Status.Idle;
            anime_mach.playAnime(Status.Idle);
        }
    }
}
