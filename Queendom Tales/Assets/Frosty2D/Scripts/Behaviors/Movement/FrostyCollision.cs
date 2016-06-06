using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FrostyCollision : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 direction;
    public float size;
    public Color color = Color.magenta;

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
                allHits = Physics2D.RaycastAll(this.GetOrigin(), direction, size);
            }
            return allHits;
        }
    }
}
