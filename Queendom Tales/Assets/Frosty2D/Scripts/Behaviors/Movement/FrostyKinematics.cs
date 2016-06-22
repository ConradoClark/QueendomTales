using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Frosty-Movement/Kinematics")]
public class FrostyKinematics : MonoBehaviour
{
    private List<Vector2> forces;
    private const int CLAMP_UP = 0;
    private const int CLAMP_RIGHT = 1;
    private const int CLAMP_DOWN = 2;
    private const int CLAMP_LEFT = 3;
    private float[] clamp;

    void Start()
    {
        forces = new List<Vector2>();
        clamp = new float[4];
        this.ResetMovement();
    }

    void Update()
    {
        //ApplyMovement(new Vector2(1,1), 5);
        //ClampPosition(Vector2.right, 400);
        //ClampPosition(Vector2.down, -194);

        Move();
        ResetMovement();
    }

    public float GetSpeed(Vector2 direction)
    {
        if (forces == null) forces = new List<Vector2>();
        direction.Normalize();
        return Vector2.Dot(direction, forces.Any() ? forces.Aggregate((v1, v2) => v1 + v2) : Vector2.zero);
    }

    private void Move()
    {
        Vector3 allForces = forces.Any() ? forces.Aggregate((v1, v2) => v1 + v2) : (Vector2) this.transform.position;
        allForces += this.transform.position;
        float clampX = Mathf.Clamp(allForces.x, float.IsNaN(clamp[CLAMP_LEFT]) ? allForces.x : clamp[CLAMP_LEFT],
                                                float.IsNaN(clamp[CLAMP_RIGHT]) ? allForces.x : clamp[CLAMP_RIGHT]);

        float clampY = Mathf.Clamp(allForces.y, float.IsNaN(clamp[CLAMP_DOWN]) ? allForces.y : clamp[CLAMP_DOWN],
                                                float.IsNaN(clamp[CLAMP_UP]) ? allForces.y : clamp[CLAMP_UP]);

        allForces = new Vector3(clampX, clampY);
        this.transform.position = new Vector3(allForces.x, allForces.y, this.transform.position.z);
    }

    private void ResetMovement()
    {
        forces.Clear();
        clamp[CLAMP_UP] = clamp[CLAMP_RIGHT] = clamp[CLAMP_DOWN] = clamp[CLAMP_LEFT] = float.NaN;
    }

    public void ApplyMovement(Vector2 direction, float speed)
    {
        forces.Add(direction.normalized * speed);
    }

    public void ClampPosition(Vector2 direction, float value)
    {
        Vector2 point = direction.normalized * value;

        clamp[CLAMP_RIGHT] = direction.x <= 0 ? clamp[CLAMP_RIGHT] : Mathf.Min(clamp[CLAMP_RIGHT], point.x);
        clamp[CLAMP_LEFT] = direction.x >= 0 ? clamp[CLAMP_LEFT] : Mathf.Max(clamp[CLAMP_LEFT], point.x);

        clamp[CLAMP_UP] = direction.y <= 0 ? clamp[CLAMP_UP] : Mathf.Min(clamp[CLAMP_UP], -point.y);
        clamp[CLAMP_DOWN] = direction.y >= 0 ? clamp[CLAMP_DOWN] : Mathf.Max(clamp[CLAMP_DOWN], -point.y);

        //clamp[CLAMP_UP] = Mathf.Min(clamp[CLAMP_UP] <= 0 ? position.y : clamp[CLAMP_UP], position.y);
        //clamp[CLAMP_DOWN] = Mathf.Min(clamp[CLAMP_DOWN] <= 0 ? -position.y : clamp[CLAMP_DOWN], -position.y);
        //clamp[CLAMP_RIGHT] = Mathf.Min(clamp[CLAMP_RIGHT] <= 0 ? position.x : clamp[CLAMP_RIGHT], position.x);
        //clamp[CLAMP_LEFT] = Mathf.Min(clamp[CLAMP_LEFT] <= 0 ? -position.x : clamp[CLAMP_LEFT], -position.x);
    }
}
