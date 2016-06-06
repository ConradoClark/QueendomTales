using UnityEngine;
using System.Collections;

[AddComponentMenu("Frosty-Movement/Conditions/Custom")]
public class FrostyMovementPredicateCustom : FrostyMovementPredicate
{
    private bool value { get; set; }
    public override bool Value { get { return value; } }

    public void SetValue(bool value)
    {
        this.value = value;
    }
}
