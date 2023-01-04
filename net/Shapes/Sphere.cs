using Engine;

namespace Shapes;

public class Sphere : IShape
{
	public readonly double r2;
	public readonly double rinv;
	public readonly Vector center;
	public readonly AABB aabb;

	public Sphere(Vector center, double r)
	{
		this.center = center;
		this.r2 = r * r;
		this.rinv = 1 / r;
		aabb = new AABB(
			center - Vector.Unit * r,
			center + Vector.Unit * r
		);
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray)
	{
		var ac = ray.start - center;
		
		var b = ac.DotProduct(ray.direction);

		var c = ac.Norm - r2;

		var D = b * b - c;

		if(D < 0)
		{
			return null;
		}

		D = Math.Sqrt(D);
		var t = -b - D;
		
		if(t < 0)
		{
			t = -b + D;
		
			if(t < 0)
			{
				return null;
			}
		}

		return new ShapeHitPoint(ray, t, (ray.PointAt(t) - center) * rinv);
	}
}
