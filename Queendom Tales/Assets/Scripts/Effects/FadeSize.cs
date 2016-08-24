﻿using UnityEngine;
using System.Collections;

public class FadeSize : MonoBehaviour
{
    public float speed = 1f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale -= (Vector3) Vector2.one * Time.deltaTime * speed;
    }
}
