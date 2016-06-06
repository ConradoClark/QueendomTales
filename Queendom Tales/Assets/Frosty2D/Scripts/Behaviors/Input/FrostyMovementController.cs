using UnityEngine;
using System.Linq;
using System.Collections;
using Assets.Frosty2D.Scripts.Core.Movement;

[AddComponentMenu("Frosty-Movement/Movement Controller")]
public class FrostyMovementController : MonoBehaviour
{
    public FrostyMovementControllerInput[] inputs;

    void Update()
    {
        FrostyMovementControllerInput[] orderedInputs = inputs.OrderBy(i => i.priority).ToArray();
        for (int i =0; i< orderedInputs.Length;i++)
        {
            FrostyMovementControllerInput input = orderedInputs[i];
            bool isHeld = Input.GetKey(input.key);
            bool isPressed = Input.GetKeyDown(input.key);
            bool isReleased = Input.GetKeyUp(input.key);
            
            if (!isPressed && isHeld && input.repeatOnHold && input.conditions.All(condition => condition.Value))
            {
                if (!input.movement.IsActivating)
                {
                    input.movement.Reactivate(input.keepSpeed);
                }
                continue;
            }

            if (isPressed && input.conditions.All(condition=>condition.Value))
            {
                if (input.toggle && !input.movement.HasFinished)
                {
                    input.movement.Deactivate();
                    continue;
                }

                if (input.exclusive)
                {
                    for (int j = 0; j < orderedInputs.Length; j++)
                    {
                        if (i != j)
                        {
                            orderedInputs[j].movement.Deactivate();
                        }
                    }
                }
                input.movement.Reactivate(input.keepSpeed);
                continue;
            }

            if (isReleased && !input.toggle && input.deactivateOnRelease)
            {
                input.movement.Deactivate();
            }
        }
    }
}
