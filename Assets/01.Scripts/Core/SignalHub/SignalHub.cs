using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnJumpEvent(float power);
public static class SignalHub
{
    public static OnJumpEvent OnJumpEventHandled;
}
