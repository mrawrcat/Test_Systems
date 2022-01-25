using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorker
{
    void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
    void PlayAnimation(Action onFinishedPlaying = null);
}
