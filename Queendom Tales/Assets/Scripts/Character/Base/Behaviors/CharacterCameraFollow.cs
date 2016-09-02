﻿using UnityEngine;
using System.Collections;

public class CharacterCameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);
    private Vector3 velocity;
    public Rect bounds;
    public float dampening;

    private void LateUpdate()
    {
        Transform target = QTToolbox.Instance.characterManager.character.transform;
        var newPos = Vector3.SmoothDamp(this.transform.position, target.position + offset, ref velocity, dampening);
        transform.position = new Vector3(Mathf.Clamp(newPos.x, bounds.xMin, bounds.xMax), Mathf.Clamp(newPos.y, bounds.yMin, bounds.yMax), transform.position.z);
    }
}