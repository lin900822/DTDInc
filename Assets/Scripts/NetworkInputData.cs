using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum InputButtons
{
    JUMP,
    FIRE
}

public struct NetworkInputData : INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 MovementInput;
    public Angle Pitch;
    public Angle Yaw;
}