using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;

public class BGMController : MonoBehaviour
{
    [SerializeField] CriAtomExPlayer atomExPlayer;
    [SerializeField]PlayerController player;
    [SerializeField]TreasureController treasure;
    long time = 0;
    public  CriAtomSource[] atomSrc = new CriAtomSource[4];
    float[] partation_x = new float[3];
    // Start is called before the first frame update
    void Start()
    {
        //atomSrc = (CriAtomSource)GetComponent("CriAtomSource");
        float temp = (treasure.init_treasure_position_x - player.init_player_position_x) / 5;
        partation_x[0] = temp;
        partation_x[1] = temp * 2;
        partation_x[2] = temp * 3;
        Debug.Log("partition");
        Debug.Log(partation_x[0]);
        Debug.Log(partation_x[1]);
        Debug.Log(partation_x[2]);
        PlaySound(atomSrc[0]);
    }

    void Update()
    {
        time = atomSrc[0].time % 4800;
        // time += atomExPlayer.Start().GetTimeSyncedWithAudio();
//        Debug.Log(time);
        if(player.transform.position.x >= partation_x[0])
        {
            PlaySound(atomSrc[1]);
        }
        if(player.transform.position.x >= partation_x[1])
        {
            PlaySound(atomSrc[2]);
        }
        if(player.transform.position.x >= partation_x[2])
        {
            PlaySound(atomSrc[3]);
        }
    
        if(player.player_animator.GetBool("Waiting"))
        {
            foreach(var sound in atomSrc)
            {
                sound.volume = 0.5f;
            }
        }
        else
        {
            foreach(var sound in atomSrc)
            {
                sound.volume = 0.8f;
            }
        }
    }
    public void PlaySound(CriAtomSource atomSrc) {
        CriAtomSource.Status status = atomSrc.status;
        if ((status == CriAtomSource.Status.Stop)) 
        {
            Debug.Log("a");
            atomSrc.startTime = (int)time;
            atomSrc.Play();
        }
    }

    public void PlayAndStopSound(CriAtomSource atomSrc) {
        if (atomSrc != null) {
            /* CriAtomSource の状態を取得 */
            CriAtomSource.Status status = atomSrc.status;
            if ((status == CriAtomSource.Status.Stop) || (status == CriAtomSource.Status.PlayEnd)) {
                /* 停止状態なので再生 */
                atomSrc.Play();
            } else {
                /* 再生中なので停止 */
                atomSrc.Stop();
            }
        }
    }
}
