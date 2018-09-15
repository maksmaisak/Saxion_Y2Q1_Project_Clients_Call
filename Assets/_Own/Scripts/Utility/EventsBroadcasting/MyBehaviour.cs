﻿using System;
using UnityEngine;

/// A behaviour that can receive broadcast events.
public abstract class MyBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        EventsManager.instance.Add(this);
    }

    protected virtual void OnDestroy()
    {
        EventsManager.instance.Remove(this);
    }
}