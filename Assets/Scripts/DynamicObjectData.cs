using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DynamicObjectData
{
    public List<string> attributes;
    public string model_name;
    public Vector3 position;
}

[Serializable]
public struct Position
{
    public int x;
    public int y;
    public int z;
}