using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
public class DogAnimator : MonoBehaviour
{

    [SerializeField]public GameManager game_manager;
    [SerializeField]public Animator dog_animator;
    void Start()
    {
        dog_animator = GetComponent<Animator>();
    }
    public async UniTask WakeUpAnim(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        // ColorManagement(Color.green);
        await UniTask.Delay(1000);
    }
    public void ColorManagement(Color color)
    {
        this.GetComponent<SpriteRenderer>().color = color;
    }

    public async UniTask LookAroundAnim(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        game_manager.wake_up_now = true;
        float look_around_number = game_manager.LookAroundToRollDice();
        for(int i = 0; i < look_around_number; i++)
        {
            await UniTask.WhenAll(WakeUpAnim(token));
        }
    } 
    public async UniTask SleepingAnim(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        float start_time = Time.time + game_manager.WakeUpToRollDice();
        while((start_time + game_manager.second_to_wake_up) >= Time.time)
        {
            // ColorManagement(Color.yellow);    
            await UniTask.Delay(200);
            // ColorManagement(Color.red);
            await UniTask.Delay(200);
        }
    }
     public async UniTask ReadyToWakeUpAnim(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        float start_time = Time.time;
        dog_animator.speed = 10f;
        while((start_time + game_manager.second_to_wake_up) >= Time.time)
        {
            // ColorManagement(Color.yellow);    
            await UniTask.Delay(100);
            // ColorManagement(Color.red);
            await UniTask.Delay(100);
        }
        dog_animator.speed = 1.0f;
    }


}
