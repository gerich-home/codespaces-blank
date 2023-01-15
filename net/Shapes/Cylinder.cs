using Engine;

namespace Shapes;

public class Cylinder : IShape
{
	public readonly Vector bottomCenter;
	public readonly double r;
	public readonly double r2;
	public readonly double rinv;
	public readonly double minY;
	public readonly double maxY;
	public readonly AABB aabb;

    public static IShape CreateClosedCylinder(Vector bottomCenter, double r, double minY, double maxY)
    {
        return CompositeShape.Create(
            new Cylinder(bottomCenter, r, minY, maxY)
            //new Disc(bottomCenter with {y = minY}, -Vector.UnitY, r, true),
            //new Disc(bottomCenter with {y = maxY}, Vector.UnitY, r, true)
        );
    }

	public Cylinder(Vector bottomCenter, double r, double minY, double maxY)
	{
		this.bottomCenter = bottomCenter;
		this.r = r;
		this.r2 = r*r;
		this.rinv = 1/r;
		this.minY = minY;
		this.maxY = maxY;

		aabb = new AABB(
			(bottomCenter - Vector.Unit * r) with {y=minY},
			(bottomCenter + Vector.Unit * r) with {y=maxY}
		);
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray)
    {
        var A = ray.direction.x * ray.direction.x + ray.direction.z * ray.direction.z;
        if (Math.Abs(A) <= double.Epsilon * 100)
        {
            return null;
        }

        var a = ray.start - bottomCenter;
        var B = a.x * ray.direction.x + a.z * ray.direction.z;
        var C = a.x * a.x + a.z * a.z - r2;
        var D = B * B - A * C;

        if (D < 0)
        {
            return null;
        }

        A = 1 / A;
        D = Math.Sqrt(D);

        var t = (-B - D) * A;
		if(t >= 0) {
			var p = IntersectionIfCorrectHeight(ray, a, t);
			if(p != null) return p;
		}

        t = (-B + D) * A;
		if(t >= 0) {
			var p = IntersectionIfCorrectHeight(ray, a, t);
			if(p != null) return p;
		}

		return null;
    }

    private ShapeHitPoint IntersectionIfCorrectHeight(in Ray ray, in Vector a, double t)
    {
        var y = ray.start.y + t * ray.direction.y;

        if (y < minY || y > maxY)
        {
            return null;
        }

        var normal = new Vector(
            rinv * (a.x + t * ray.direction.x),
            0,
            rinv * (a.z + t * ray.direction.z)
        );

        return new ShapeHitPoint(ray, t, -normal * Math.Sign(normal.DotProduct(ray.direction)));
    }
}
