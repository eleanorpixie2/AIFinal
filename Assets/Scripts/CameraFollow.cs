﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerToFollow;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = playerToFollow.position-transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerToFollow.position - offset;
    }
}
