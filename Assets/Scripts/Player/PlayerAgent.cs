using Fusion.KCC;
using System.Collections;
using UnityEngine;

public class PlayerAgent : Agent
{
	public PlayerController playerController = null;

	[SerializeField]
	private Transform cameraTrans;

	[SerializeField]
	private float _maxCameraAngle = 75f;
	[SerializeField]
	private Vector3 _jumpImpulse = new Vector3(0f, 6f, 0f);

	protected override void OnDespawned()
	{
		playerController = null;
	}

	protected override void ProcessEarlyFixedInput()
	{
		if (playerController == null)
			return;

		// Look Rotation Input

		var input = playerController.Input.FixedInput;

		Vector2 lookRotation = KCC.FixedData.GetLookRotation(true, true);
		Vector2 lookRotationDelta = KCCUtility.GetClampedLookRotationDelta(lookRotation, input.LookRotationDelta, -_maxCameraAngle, _maxCameraAngle);

		KCC.AddLookRotation(lookRotationDelta);

		// Movement Input

		Vector3 inputDirection = KCC.FixedData.TransformRotation * new Vector3(input.MoveDirection.x, 0.0f, input.MoveDirection.y);

		KCC.SetInputDirection(inputDirection);

		if (playerController.Input.WasPressed(InputButtons.Jump))
		{
			Quaternion jumpRotation = KCC.FixedData.TransformRotation;

			if (inputDirection.IsAlmostZero() == false)
			{
				jumpRotation = Quaternion.LookRotation(inputDirection);
			}

			KCC.Jump(jumpRotation * _jumpImpulse);
		}

		playerController.AbilityHandler.ProcessInput();
	}

    protected override void OnFixedUpdate()
	{
		Vector2 pitchRotation = KCC.FixedData.GetLookRotation(true, false);
		cameraTrans.localRotation = Quaternion.Euler(pitchRotation);
	}

	protected override void ProcessLateFixedInput() 
	{
		// Weapon Input
		playerController.AttackHandler.ProcessInput();
	}

	protected override void OnLateFixedUpdate() 
	{
		
	}

	protected override void ProcessRenderInput()
	{
		if (playerController == null)
			return;

		// Look Rotation Input

		var input = playerController.Input;

		Vector2 lookRotation = KCC.FixedData.GetLookRotation(true, true);

		Vector2 lookRotationDelta = KCCUtility.GetClampedLookRotationDelta(lookRotation, input.CachedInput.LookRotationDelta, -_maxCameraAngle, _maxCameraAngle);

		KCC.SetLookRotation(lookRotation + lookRotationDelta);

		// Movement Input

		Vector3 inputDirection = default;

		Vector3 moveDirection = input.RenderInput.MoveDirection.X0Y();
		if (moveDirection.IsZero() == false)
		{
			inputDirection = KCC.RenderData.TransformRotation * moveDirection;
		}

		KCC.SetInputDirection(inputDirection);

		if (playerController.Input.WasPressed(InputButtons.Jump))
		{
			Quaternion jumpRotation = KCC.RenderData.TransformRotation;

			if (inputDirection.IsZero() == false)
			{
				jumpRotation = Quaternion.LookRotation(inputDirection);
			}

			KCC.Jump(jumpRotation * _jumpImpulse);
		}
	}

	protected override void OnLateRender()
	{
		if (Object.HasInputAuthority == true)
		{
			Vector2 pitchRotation = KCC.RenderData.GetLookRotation(true, false);
			cameraTrans.localRotation = Quaternion.Euler(pitchRotation);
		}

		playerController.ThirdPersonCamera.OnRender();
	}
}