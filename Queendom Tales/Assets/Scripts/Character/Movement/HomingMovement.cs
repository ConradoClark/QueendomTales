using UnityEngine;
using System.Collections;

[AddComponentMenu("Queendom-Tales/Character/Homing Movement")]
public class HomingMovement : MonoBehaviour
{
    public TargetCursor cursor;
    public FullCharacterController character;
    public float speed;
    public bool enableMovement;

    private float xVelocity;
    private float yVelocity;
    public TimeLayers timeLayer;

    void Update()
    {
        if (cursor.currentTarget == null || !enableMovement) return;

        float x = Mathf.SmoothDamp(character.kinematics.transform.position.x, cursor.currentTarget.transform.position.x, ref xVelocity, 0.15f, speed);
        float y = Mathf.SmoothDamp(character.kinematics.transform.position.y, cursor.currentTarget.transform.position.y, ref yVelocity, 0.15f, speed);

        x = character.kinematics.transform.position.x - x;
        y = -character.kinematics.transform.position.y + y;

        x *= Toolbox.Instance.time.GetLayerMultiplier(timeLayer);
        y *= Toolbox.Instance.time.GetLayerMultiplier(timeLayer);

        character.kinematics.ApplyMovement(new Vector2(Mathf.Sign(x) * character.GetFacingDirection().x, 0), x);
        character.kinematics.ApplyMovement(new Vector2(0, Mathf.Sign(y)), y * Mathf.Sign(y));
    }
}
