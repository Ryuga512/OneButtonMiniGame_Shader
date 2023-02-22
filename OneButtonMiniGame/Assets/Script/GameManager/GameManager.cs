using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public class GameManager : DataManager
{
    [SerializeField] PlayerController player;
    [SerializeField]CameraController camera_controller;
    [SerializeField] DogController dog;
    [SerializeField] TreasureController treasure;
    [SerializeField] float clear_pos_x = 25;
    // public bool wake_up_start{get;set;}
    public bool wake_up_now{get;set;} = false;
    public bool game_over{get; set;}
    public bool game_clear{get; set;}
    // [SerializeField] List<WakeUpProbability[]> wake_up_probability = new List<WakeUpProbability[]>();
    // Start is called before the first frame update
    async UniTask Start()
    {
        game_over = false;
        game_clear = false;
        var token = this.GetCancellationTokenOnDestroy();
        //clear_pos_x = treasure.transform.position.x + 0.5f;
        // wake_up_start = true;
    }

    // Update is called once per frame
    async UniTask Update()
    {
        if(!game_over && !game_clear)
        {
            Clear();
            JudgingOut();
        }
    }

    public float WakeUpToRollDice()
    {
        int result = 0;
        foreach(var value in wake_up_probability)
        {
            result += value.wake_up_probability;
        }

        int rand = Random.Range(1,result);
        int i = 0; 
        int temp = 0;
        for(;i < wake_up_probability.Length; i++)
        {
            //Debug.Log(i);
            if(temp < rand && rand <= wake_up_probability[i].wake_up_probability + temp)
            {
                break;
            }
            temp += wake_up_probability[i].wake_up_probability;
        }
        Debug.Log(wake_up_probability[i].second);
        return wake_up_probability[i].second;
    }

    public int LookAroundToRollDice()
    {
        int result = 0;
        foreach(var value in kyoro_probability)
        {
            result += value.kyoro_probability;
        }

        int rand = Random.Range(1,result);
        int i = 0; 
        int temp = 0;
        for(;i < kyoro_probability.Length; i++)
        {
            Debug.Log(i);
            if(temp < rand && rand <= kyoro_probability[i].kyoro_probability + temp)
            {
                break;
            }
            temp += kyoro_probability[i].kyoro_probability;
        }
        Debug.Log(kyoro_probability[i].number);
        return kyoro_probability[i].number;
    }
    async UniTask JudgingOut()
    {

        if(wake_up_now && player.moving_now && dog.in_left)
        {
            Debug.Log("死");
            dog.cts.Cancel();
            player.ct.Cancel();
            // dog.ColorManagement(Color.black);
            dog.dog_animator.SetBool("GameOver", true);
            player.player_animator.SetBool("GameOver", true);
            game_over = true;
            
        }
    }

    async UniTask Clear()
    {
        if(treasure.treasure_open && player.transform.position.x >= clear_pos_x)
        {
            Debug.Log("clear");
            dog.cts.Cancel();
            dog.dog_animator.SetBool("GameClear", true);
            game_clear = true;
            player.Move(player.token);
            speed = 3.0f;
            await UniTask.WaitUntil(()=>player.transform.position.x >= (treasure.transform.position.x+0.5f));
            //camera_controller.target.RemoveMember(player.transform);
            player.ct.Cancel();
            player.player_animator.SetBool("GameClear", true);
        }
    }
}
