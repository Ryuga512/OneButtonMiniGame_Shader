using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class PlayerController : PlayerAnimator
{
    [SerializeField]GameManager game_manager;
    [SerializeField]public float init_player_position_x;
    [SerializeField]public float init_player_position_y;
    bool clear_wall_run = false;
    private SpriteRenderer renderer;
    private Transform transform;
    private Rigidbody2D player_rigidbody2D;
    public PlayerSEController player_se;
    public CancellationTokenSource ct;
    public bool moving_now{get;set;}
    Vector3 pos;
    public AudioClip WaitingForSE;
    public AudioClip WaitingCancelSE;
    public AudioClip sound3;
    public AudioClip sound4;
    AudioSource player_AudioSource;
    public CancellationToken token;
    // Start is called before the first frame update
    void Start()
    {
        token = this.GetCancellationTokenOnDestroy();
        InitPlayer();
    }

    void InitPlayer()
    {
        renderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        ct = new CancellationTokenSource();
        player_rigidbody2D = GetComponent<Rigidbody2D>();
        moving_now = true;
        player_AudioSource = GetComponent<AudioSource>();
        player_animator = GetComponent<Animator>();
        transform.position = new Vector2(init_player_position_x,init_player_position_y);
    }
    async UniTask Update()
    {
        if(clear_wall_run && pos.y < 10.0f)
        {
            pos.y += (20.0f - 9.8f * Time.deltaTime) * Time.deltaTime;
            transform.position = pos;
        }
        var token = ct.Token;
        if(moving_now && !game_manager.game_over && !player_animator.GetBool("Waiting") && !player_animator.GetBool("PreMove"))
        {
            token.ThrowIfCancellationRequested();
            await UniTask.WhenAll(Move(token));
        }
        //ct = this.GetCancellationTokenOnDestroy();
        //await ct.ToUniTask().Item1;
    }

    public async UniTask Move(CancellationToken token)
    {        
        token.ThrowIfCancellationRequested();
        // Vector2 vec = new Vector2(0,game_manager.speed);
        // player_rigidbody2D.AddForce(vec);
        pos.x += game_manager.speed * Time.deltaTime;
        transform.position = pos;
    }

    public void OnStop(InputAction.CallbackContext context)
    {
        if(game_manager.game_clear)
        {
            return;
        }
        if(!game_manager.game_over && context.performed)
        {
            if(player_animator.GetBool("PreMove"))
            {
                player_animator.SetBool("PreMove", false);
            }
            player_animator.SetBool("Waiting", true);
            moving_now = false;
            //InversionWaitingPreMove();
            //player_AudioSource.PlayOneShot(WaitingForSE);
            player_se.PlaySound(player_se.atomSrc[0]);
            Debug.Log("aaa");
        }
    }

    // async public UniTask InPreMove()
    // {
    //     if(player_animator.GetBool("PreMove"))
    //     {
    //         await UniTask.Delay(250);
    //     }
    // }
    public void OnMove()
    {
        moving_now = true;
    }

    public void PreOnMove(InputAction.CallbackContext context)
    {
        if(!game_manager.game_over && context.performed)
        {
            OnMove();
            // ゲーム開始時
            if(!player_animator.GetBool("Waiting") && !player_animator.GetBool("PreMove"))
            {
                return;
            }
            else if(player_animator.GetBool("Waiting"))
            {
                player_animator.SetBool("PreMove", true);
                player_animator.SetBool("Waiting", false);
                //player_AudioSource.PlayOneShot(WaitingCancelSE);
                player_se.PlaySound(player_se.atomSrc[1]);
            }
        }
    }
    public void PreOnMoveCancel()
    {
        player_animator.SetBool("PreMove", false);
        
    }
    public void MoveOnWalking()
    {
        moving_now = true;
    }

    
    public void StopOnWalking()
    {
        moving_now = false;
    }

    public void Empty()
    {
        renderer.material.color= new Color32(0,0,0,0);  
    }

    public void GameEnd()
    {
        player_animator.SetBool("GameEnd", true);
        player_animator.SetBool("GameOver", false);
        player_animator.SetBool("GameClear", false);
    }
    public void InversionWaitingPreMove()
    {
        if(player_animator.GetBool("PreMove"))
        {
            player_animator.SetBool("Waiting", false);
        }
        else if(player_animator.GetBool("Waiting"))
        {
            player_animator.SetBool("PreMove", true);
        }
    }

    public void TreasureDelete()
    {

    }

    public void ClearMoveStart()
    {

    }

    public void ClearMoveStop()
    {

    }

    public void ClearJump()
    {
        transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
    }
    public void ClearJumpEnd()
    {
        
    }

    public void ClearWallRun()
    {
        // pos.y += 5.0f * Time.deltaTime;
        // transform.position = pos;
        clear_wall_run = true;
        // pos.y += 3.0f * Time.deltaTime;
        // transform.position = pos;
       //await UniTask.WaitUntil(() => transform.position.y > 10.0f);
    }

    public void ClearWallRunStop()
    {
        // pos.y += 5.0f * Time.deltaTime;
        // transform.position = pos;
        clear_wall_run = false;
        // pos.y += 3.0f * Time.deltaTime;
        // transform.position = pos;
       //await UniTask.WaitUntil(() => transform.position.y > 10.0f);
    }
    // // OnStop中は
    // public void OnStop()
    // {
    //     moving_now = false;
    //     InversionWaitingPreMove();
    //     player_animator.SetBool("Waiting", true);
    // }

    // public void InWaiting()
    // {
    //     player_AudioSource.PlayOneShot(WaitingForSE);
        
    //     player_animator.SetBool("PreMove", false);
    // }
    // public void OnMove()
    // {
    //     moving_now = true;
    //     player_animator.SetBool("Waiting", false);
    // }

    // public void PreOnMove()
    // {
    //     player_animator.SetBool("PreMove", true);
    // }
    // public void PreOnMoveCancel()
    // {
    //     player_animator.SetBool("PreMove", false);
    //     OnMove();
    // }
    // public void MoveOnWalking()
    // {
    //     moving_now = true;
    // }

    
    // public void StopOnWalking()
    // {
    //     moving_now = false;
    // }

    // public void InversionWaitingPreMove()
    // {
    //     if(player_animator.GetBool("PreMove"))
    //     {
    //         player_animator.SetBool("Waiting", false);
    //     }
    //     else if(player_animator.GetBool("Waiting"))
    //     {
    //         player_animator.SetBool("PreMove", true);
    //     }
    // }


}
