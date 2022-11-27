using Fusion;
using System.Collections;
using UnityEngine;

public enum InputButtons
{
	Fire = 0,
	Jump = 1,
	Run = 2,
	UseAbility = 3, 
}

public struct GameplayInput : INetworkInput
{
	public Vector2 MoveDirection;
	public Vector2 LookRotationDelta;
	public float MouseWheelDelta;
	public NetworkButtons Buttons;
}