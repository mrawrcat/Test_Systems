using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
    void PlayAnimation(float animLength, Action onFinishedPlaying = null);
}
   