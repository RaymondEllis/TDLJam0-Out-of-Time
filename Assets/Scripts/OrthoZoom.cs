using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class OthroZoom : Cinemachine.CinemachineExtension
{
	private float initialSize;

	protected override void Awake()
	{
		base.Awake();
		initialSize = VirtualCamera.State.Lens.OrthographicSize;
	}

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == CinemachineCore.Stage.Aim)
		{
			var lens = state.Lens;
			lens.OrthographicSize = initialSize * vcam.Follow.transform.localScale.x;
			state.Lens = lens;
		}
	}
}
