using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class DogController : DogAnimator
{
    public CancellationTokenSource cts = new CancellationTokenSource();
    [SerializeField]float init_dog_position_x;
    [SerializeField]float init_dog_position_y;
    private Transform transform;
    [SerializeField]PlayerController player;
    // public Animator dog_animator;
    public bool in_left = false;
    public AudioClip KyoroKyoroSE;
    AudioSource dog_AudioSource;
    public DogSEController dog_se;
    // Start is called before the first frame update
    async UniTask Start()
    {
        transform = GetComponent<Transform>();
        transform.position = new Vector2(init_dog_position_x,init_dog_position_y);
        dog_animator = GetComponent<Animator>();
        var token = this.GetCancellationTokenOnDestroy();
        dog_AudioSource = GetComponent<AudioSource>();
        token = cts.Token;

         while(true)
         {
            await UniTask.WhenAll(WakeUp(token));
         }
    }

    async UniTask Update()
    {
    }
   async UniTask WakeUp(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        // game_manager.wake_up_start = false;
        await UniTask.WhenAll(Sleeping(token));
        await UniTask.WhenAll(ReadyToWakeUpAnim(token));
        await UniTask.WhenAll(Waking(token),LookAroundAnim(token));
        await UniTask.WhenAll(WakeDown(token));
        // game_manager.wake_up_start = true;
        
    }

    async UniTask Waking(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        dog_animator.SetBool("WakeUpStart", true);
        await UniTask.Delay(280);
    }
    public void GameEnd()
    {
        dog_animator.SetBool("GameEnd", true);
        dog_animator.SetBool("GameOver", false);
        dog_animator.SetBool("GameClear", false);
    }
    public void InLeft()
    {

        in_left = true;
    }
    public void InCenter()
    {
        in_left = false;
    }    
    public void InRight()
    {
        //dog_AudioSource.PlayOneShot(KyoroKyoroSE);
        dog_se.PlaySound(dog_se.atomSrc[0]);
        in_left = false;
    }
    async UniTask WakeDown(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        dog_animator.SetBool("WakeUpStart", false);
        game_manager.wake_up_now = false;
    }
    async UniTask Sleeping(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await UniTask.WhenAll(SleepingAnim(token));
    }
}

//memo
// using UniRx.Async;
// using UnityEngine;

// public class Sample11 : MonoBehaviour
// {
//     async void Start()
//     {
//         // 条件を満たしたら待機終了
//         await UniTask.WaitUntil(() => transform.position.y < 0);

//         // 指定オブジェクトの状態が変化するのを待つ
//         await UniTask.WaitUntilValueChanged(transform, x => x.position.x); //x方向に移動した

//         // 指定オブジェクトの状態が変化するのを待つ ＆ 対象がDestroyされたら待機終了
//         await UniTask.WaitUntilValueChangedWithIsDestroyed(transform, x => x.position.x);

//         // 条件を満たさなくなったら待機終了
//         await UniTask.WaitWhile(() => transform.position.z < 0);
//     }
// }

//　デリゲートでunitaskで作ったメソッドを入れる