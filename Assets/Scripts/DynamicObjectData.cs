using System;
using System.Collections.Generic;

[Serializable]
public struct DynamicObjectData
{
    public List<string> attributes;
    public string model_name;
    public Position position;
}

[Serializable]
public struct Position
{
    public int x;
    public int y;
    public int z;
}