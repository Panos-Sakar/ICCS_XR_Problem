using System;
using System.Collections.Generic;

[Serializable]
public struct SpawnedObjectData
{
    public int object_id; 
    public double x;
    public double y;
    public double z;
    public Header header;
    public List<ContourPoint> contour_points;
    public int bin_id;
    public int n_objects;
    public double object_area;
    public double total_area;
    public double velocity;
}

[Serializable]
public struct ContourPoint
{
    public double x;
    public double y;
}

[Serializable]
public struct Header
{
    public Stamp stamp;
    public string frame_id;
}

[Serializable]
public struct Stamp
{
    public int sec;
    public int nsec;
}
