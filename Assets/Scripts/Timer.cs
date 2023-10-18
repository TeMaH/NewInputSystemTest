using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action OnTime;

    public float TargetTime;
    public float CurrentTime;

    void Start() { }

    void Update() 
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= TargetTime && OnTime != null)
        { 
            OnTime();
            Destroy(this);
        }
    }
}
