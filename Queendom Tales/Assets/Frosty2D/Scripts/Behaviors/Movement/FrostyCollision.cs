﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FrostyCollision : MonoBehaviour
{
    [Header("Collider Parameters")]
    public Vector2 offset;
    public Vector2 direction;
    public float size;
    public Color color = Color.magenta;
    [Header("References")]
    public FrostyKinematics movement;
    public FrostyTag tags;
    [Header("Raycast Parameters")]
    public bool rayHasfixedSize;
    public float raySize;
    public float minimumRaySize = 0f;
    public bool overrideRayDirection;
    public Vector2 overriddenRayDirection;
    public float speedFactor = 1.0f;
    [Header("Clamp Direction")]
    public bool overrideClampDirection;
    public Vector2 clampDirection;
    [Header("Precision")]
    public float rayPrecision = 5;
    public bool ignoreBorders = false;

    void LateUpdate()
    {
        allHits = GetCollisions();
        var orig = this.GetOrigin();
        Debug.DrawRay(new Vector3(orig.x, orig.y, orig.z), new Vector2(direction.y, direction.x) * size, color);

        var d = GetValidDirection();
        for (int i = 0; i < rayPrecision; i++)
        {
            if (ignoreBorders && (i == 0 || i == rayPrecision - 1)) continue;
            Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * (size) / (rayPrecision - 1) * i, d * GetValidSpeed(d), color);
        }
    }

    private Vector3 GetOrigin()
    {
        return new Vector3(transform.position.x + offset.x - (direction.y * size / 2), transform.position.y + offset.y - (direction.x * size / 2));
    }

    private RaycastHit2D[] allHits;
    public RaycastHit2D[] AllHits
    {
        get
        {
            return allHits;
        }
    }

    private RaycastHit2D[] GetCollisions()
    {
        Vector2 validDirection = GetValidDirection();
        float validSpeed = GetValidSpeed(validDirection);

        List<RaycastHit2D[]> raycasts = new List<RaycastHit2D[]>();

        for (int i = 0; i < rayPrecision; i++)
        {
            if (ignoreBorders && (i == 0 || i == rayPrecision - 1)) continue;
            raycasts.Add(Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / (rayPrecision - 1) * i, validDirection, validSpeed));
        }

        return raycasts.SelectMany(h => h).Where(h => FrostyTag.AnyFromComponent(tags, h.collider)).ToArray();
    }

    public float GetValidSpeed(Vector2 validDirection)
    {
        return rayHasfixedSize || movement == null ? raySize : Mathf.Abs(movement.GetSpeed(validDirection) * speedFactor) + minimumRaySize;
    }

    public Vector2 GetValidDirection()
    {
        return !overrideRayDirection ? direction : overriddenRayDirection;
    }

    public Vector2 GetClampDirection()
    {
        return !overrideClampDirection ? direction : clampDirection;
    }
}
