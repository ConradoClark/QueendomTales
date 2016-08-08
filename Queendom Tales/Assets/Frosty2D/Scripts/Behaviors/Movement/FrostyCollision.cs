using UnityEngine;
using System.Linq;
using System.Collections;

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
        Debug.DrawRay(this.GetOrigin(), direction * (movement != null ? movement.GetSpeed(direction) : raySize), color);
        Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, direction * (movement != null ? movement.GetSpeed(direction) : raySize), color);

        var d = GetValidDirection();
        Debug.DrawRay(this.GetOrigin(), d * GetValidSpeed(d),color);
        Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, d * GetValidSpeed(d), color);
        Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 2, d * GetValidSpeed(d), color);
        Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 4, d * GetValidSpeed(d), color);
        Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size * 2 / 3, d * GetValidSpeed(d), color);
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

    public RaycastHit2D[] GetCollisions()
    {
        Vector2 validDirection = GetValidDirection();
        float validSpeed = GetValidSpeed(validDirection);

        var hits1 = Physics2D.RaycastAll(this.GetOrigin(), validDirection, validSpeed);

        var hits2 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, validDirection, validSpeed);

        var hits3 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 2, validDirection, validSpeed);

        var hits4 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size / 4, validDirection, validSpeed);

        var hits5 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size * 2 / 3, validDirection, validSpeed);

        return new[] { hits1, hits2, hits3, hits4, hits5 }.SelectMany(h => h).Where(h => FrostyTag.AnyFromComponent(tags, h.collider)).ToArray();
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
