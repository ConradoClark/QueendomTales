using UnityEngine;
using System.Linq;
using System.Collections;

[ExecuteInEditMode]
public class FrostyCollision : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 direction;
    public float size;
    public Color color = Color.magenta;
    public float raySize;
    public FrostyKinematics movement;
    public FrostyTag tags;

    void Start()
    {

    }

    void Update()
    {
        allHits = null;
    }

    void LateUpdate()
    {
        Debug.DrawRay(this.GetOrigin(), new Vector2(direction.y, direction.x) * size, color);
        Debug.DrawRay(this.GetOrigin(), direction * (movement != null ? movement.GetSpeed(direction) : raySize), color);
        Debug.DrawRay(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, direction * (movement != null ? movement.GetSpeed(direction) : raySize), color);
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
            if (allHits == null)
            {

                var hits1 = Physics2D.RaycastAll(this.GetOrigin(), direction, (movement != null ? movement.GetSpeed(direction) : raySize));

                var hits2 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size, direction, (movement != null ? movement.GetSpeed(direction) : raySize));

                var hits3 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size/2, direction, (movement != null ? movement.GetSpeed(direction) : raySize));

                var hits4 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size/4, direction, (movement != null ? movement.GetSpeed(direction) : raySize));

                var hits5 = Physics2D.RaycastAll(this.GetOrigin() + new Vector3(direction.y, direction.x) * size*2/3, direction, (movement != null ? movement.GetSpeed(direction) : raySize));

                allHits = new[] { hits1, hits2, hits3, hits4, hits5 }.SelectMany(h => h).Where(h=>FrostyTag.AnyFromComponent(tags,h.collider)).ToArray();
            }
            return allHits;
        }
    }
}
