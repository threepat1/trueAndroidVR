﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleUI : MonoBehaviour
{

    public MovieDB skinData;

    protected virtual void OnSkinUI()
    {

    }

    public virtual void Awake()
    {
        OnSkinUI();
    }

    public virtual void Update()
    {
        if (Application.isEditor)
        {
            OnSkinUI();
        }
    }

}
