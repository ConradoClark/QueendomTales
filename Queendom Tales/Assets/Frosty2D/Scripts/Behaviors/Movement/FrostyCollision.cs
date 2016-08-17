using UnityEngine;
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

    void Start()
    {

    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        allHits = GetCollisions();
        Debug.DrawRay(this.GetOrigin(), new Vector2(direction.y, direction.x) * size, color);
        //Debug.DrawRay(this.GetOrigin(), direction * (movement != null ? movement.GetSpeed(direction) : raySize), color);
        //Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, direction * (movement != null ? movement.GetSpeed(direction) : raySize), color);

        var d = GetValidDirection();
        //Debug.DrawRay(this.GetOrigin(), d * GetValidSpeed(d),color);
        //Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, d * GetValidSpeed(d), color);

        for (int i = 0; i < rayPrecision; i++)
        {
            Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * (size) / (rayPrecision-1) * i, d * GetValidSpeed(d), color);
        }

        //Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size * Mathf.Pow(1, 2) / 1, d * GetValidSpeed(d), color);
        //Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 2, d * GetValidSpeed(d), color);
        //Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 4, d * GetValidSpeed(d), color);
        //Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size * 2 / 3, d * GetValidSpeed(d), color);
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

        //0var first = Physics2D.RaycastAll(this.GetOrigin(), validDirection, validSpeed);
        //var last = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, validDirection, validSpeed);

        for (int i = 0; i < rayPrecision; i++)
        {
            raycasts.Add(Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / (rayPrecision-1) * i, validDirection, validSpeed));
        }

        //var hits1 = Physics2D.RaycastAll(this.GetOrigin(), validDirection, validSpeed);

        //var hits2 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, validDirection, validSpeed);

        //var hits3 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 2, validDirection, validSpeed);

        //var hits4 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 4, validDirection, validSpeed);

        //var hits5 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size * 2 / 3, validDirection, validSpeed);

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
