using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

public class ByteMapped : Attribute
{
    public int order { get; private set; }

    public ByteMapped ([CallerLineNumber]int order = 0)
    {
        this.order = order;
    }
}
