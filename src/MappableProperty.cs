using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

public class MappableProperty : IComparable
{

    public int order;
    public PropertyInfo propertyInfo;

    public MappableProperty(int order, PropertyInfo props)
    {
        this.order = order;
        this.propertyInfo = props;
    }


    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        MappableProperty mappable = (MappableProperty)obj;

        if (mappable.order > this.order)
        {
            return -1;
        }

        if (mappable.order == this.order)
        {
            return 0;
        }

        return 1;
    }
}
