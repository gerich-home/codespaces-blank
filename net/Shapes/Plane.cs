using Engine;

namespace Shapes;

public class Plane : IShape
{
	public readonly double d;
	public readonly Vector normal;
	public readonly Vector A;
	public readonly AABB aabb;

	public Plane(Vector normal, double d)
	{
		this.normal = normal;
		this.d = d;
		this.A = normal.x != 0 ? new Vector(-d / normal.x, 0, 0) :
				normal.y != 0 ? new Vector(0, -d / normal.y, 0) :
				new Vector(0, 0, -d / normal.z);
		aabb = AABB.MaxValue;
	}

	public Plane(Vector a, Vector b, Vector A)
	{
		this.normal = a.CrossProduct(b).Normalized;
		this.A = A;
		this.d = A.DotProduct(normal);
		aabb = AABB.MaxValue;
	}

	public Plane(double a, double b, double c, double d)
	{
		this.normal = new Vector(a, b, c).Normalized;
		this.d = d;
		this.A = a != 0
			? new Vector(-d / a, 0, 0)
			: (
				b != 0
				? new Vector(0, -d / b, 0)
				: new Vector(0, 0, -d / c)
			);
		aabb = AABB.MaxValue;
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray)
	{
		double divident = normal.DotProduct(ray.direction);

		if(divident == 0)
		{
			return null;
		}

		double t = (ray.start.DotProduct(normal) + d) / normal.DotProduct(ray.direction);

		if(t < 0)
		{
			return null;
		}

		return new ShapeHitPoint(ray, t, normal);
	}
}
