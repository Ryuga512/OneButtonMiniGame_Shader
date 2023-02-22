using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;

public class PlayerSEController : MonoBehaviour
{

    [SerializeField] CriAtomExPlayer atomExPlayer;

    long time = 0;
    public  CriAtomSource[] atomSrc = new CriAtomSource[4];
    float[] partation_x = new float[3];
    // Start is called before the first frame update

    public void PlaySound(CriAtomSource atomSrc) {
        atomSrc.Play();
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
