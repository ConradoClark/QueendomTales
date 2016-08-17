﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System;

public class FrostyTag : MonoBehaviour
{
    [Flags]
    public enum FrostyTags
    {
        Platform = 1,
        Wall = 2,
        Ceiling = 3,
        Slope = 4,
        Actor = 5
    }

    public FrostyTags[] tags = new FrostyTags[0];

    void Start()
    {
    }

    public bool Any(FrostyTag tag)
    {
        return tags.Any(t => tag.tags.Any(tg => (t & tg) == t));
    }

    public static bool AnyFromComponent(FrostyTag tag, Component comp)
    {
        FrostyTag compTag = comp.GetComponent<FrostyTag>();
        return compTag != null && tag != null ? tag.Any(compTag) : false;
    }
}
