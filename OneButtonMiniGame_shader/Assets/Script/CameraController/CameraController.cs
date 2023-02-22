using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : CinemachineExtension
{
    public GameManager game_manager;
    public CinemachineVirtualCameraBase _CinemachineVirtualCameraBase;
    public CinemachineTargetGroup target;
    public PlayerController player = new PlayerController();
    int count = 0;
    [Tooltip("Lock the camera's Y position to this value")]
    public float m_XPosition = 29.3f;
    public float m_YPosition = -0.215f;
    // Start is called before the first frame update
    void Start()
    {
        //_CinemachineVirtualCameraBase.FollowTargetAttachment = ;
    }

    // Update is called once per frame
    void Update()
    {
        if(count == 1)
        {
            _CinemachineVirtualCameraBase.PreviousStateIsValid = false;
        }
        else if(count == 0)
        {
            count++;
        }
    }

        protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (game_manager.game_clear == true && stage == CinemachineCore.Stage.Finalize)
        {
            
            var pos = state.RawPosition;
            pos.x = m_XPosition;
            pos.y = m_YPosition;    
            state.RawPosition = pos;
            target.RemoveMember(player.transform);
        }
    }
}
