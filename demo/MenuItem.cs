using System;
using System.Collections;

public class MenuItem
{
    [ByteMapped]
    public int itemCost {get; set;}

    [ByteMapped]
    public string itemName {get; set;}
}