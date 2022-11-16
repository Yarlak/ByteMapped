using System;
using System.Collections;

public class Restaurant
{
    [ByteMapped]
    public int capacity {get; set;}

    [ByteMapped]
    public string name {get; set;}

    [ByteMapped]
    public Dictionary<string, MenuItem> menu {get; set;}
}