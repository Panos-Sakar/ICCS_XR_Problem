using System;
using System.Collections.Generic;

[Serializable]
public struct SpawnedObjectData
{
    public int object_id;

    public double x;
    public double y;
    public double z;

    //Omitted
    //public Header header;
    
#if VISUALIZE_CONTOUR_POINTS
    public List<ContourPoint> contour_points;
#endif

    //Omitted
    //public int bin_id;

    //Omitted
    //public int n_objects;

    //Omitted
    //public double object_area;

    //Omitted
    //public double total_area;
    
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
