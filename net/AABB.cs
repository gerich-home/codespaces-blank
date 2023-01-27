namespace Engine;

public readonly record struct AABB(
	Vector min,
	Vector max
) {
    public static AABB FromEdgePoints(params Vector[] points) =>
        FromEdgePoints((IEnumerable<Vector>)points);

    public static AABB FromEdgePoints(IEnumerable<Vector> points) =>
        new AABB(
            new Vector(
                points.Min(p => p.x),
                points.Min(p => p.y),
                points.Min(p => p.z)
            ),
            new Vector(
                points.Max(p => p.x),
                points.Max(p => p.y),
                points.Max(p => p.z)
            )
        );

    public IEnumerable<Vector> EdgePoints()
    {
        yield return min;
        yield return new Vector(min.x, min.y, max.z);
        yield return new Vector(min.x, max.y, min.z);
        yield return new Vector(min.x, max.y, max.z);
        yield return new Vector(max.x, min.y, min.z);
        yield return new Vector(max.x, min.y, max.z);
        yield return new Vector(max.x, max.y, min.z);
        yield return max;
    }
    
    public AABB Transform(Matrix matrix) =>
        FromEdgePoints(EdgePoints().Select(point => matrix.MultiplicateByPosition(point)));
    
    public static AABB MaxValue =>
        new AABB(
            new Vector(double.MinValue, double.MinValue, double.MinValue),
            new Vector(double.MaxValue, double.MaxValue, double.MaxValue)
        );

    public bool CanIntersect(in Ray ray, in Vector dirInv)
    {
        // https://tavianator.com/2022/ray_box_boundary.html
        var x_inv = dirInv.x;
        var x_start = ray.start.x;
        var tx1 = (min.x - x_start)*x_inv;
        var tx2 = (max.x - x_start)*x_inv;

        var y_inv = dirInv.y;
        var y_start = ray.start.y;
        var ty1 = (min.y - y_start)*y_inv;
        var ty2 = (max.y - y_start)*y_inv;

        var z_inv = dirInv.z;
        var z_start = ray.start.z;
        var tz1 = (min.z - z_start)*z_inv;
        var tz2 = (max.z - z_start)*z_inv;

        var tmin = Math.Max(
            Math.Min(tx1, tx2),
            Math.Max(
                Math.Min(ty1, ty2),
                Math.Min(tz1, tz2)
            )
        );

        var tmax = Math.Min(
            Math.Max(tx1, tx2),
            Math.Min(
                Math.Max(ty1, ty2),
                Math.Max(tz1, tz2)
            )
        );

        return tmax >= tmin;
    }

    public AABB Union(in AABB other) =>
        new AABB(
            new Vector(
                Math.Min(min.x, other.min.x),
                Math.Min(min.y, other.min.y),
                Math.Min(min.z, other.min.z)
            ),
            new Vector(
                Math.Max(max.x, other.max.x),
                Math.Max(max.y, other.max.y),
                Math.Max(max.z, other.max.z)
            )
        );

    public Vector Center => (min + max) / 2;
}
