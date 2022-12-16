using Engine;

namespace Shapes;

public class Plane : IShape
{
	public readonly double d;
	public readonly Vector normal;
	public readonly Vector A;
	public readonly IMaterial material;

	public Plane(Vector normal, double d, IMaterial material)
	{
		this.normal = normal;
		this.d = d;
		this.A = normal.x != 0 ? new Vector(-d / normal.x, 0, 0) :
				normal.y != 0 ? new Vector(0, -d / normal.y, 0) :
				new Vector(0, 0, -d / normal.z);
		this.material = material;
	}

	public Plane(Vector a, Vector b, Vector A, IMaterial material)
	{
		this.normal = a.CrossProduct(b).Normalized;
		this.A = A;
		this.material = material;
		this.d = A.DotProduct(normal);
	}

	public Plane(double a, double b, double c, double d, IMaterial material)
	{
		this.normal = new Vector(a, b, c).Normalized;
		this.d = d;
		this.material = material;
		this.A = a != 0
			? new Vector(-d / a, 0, 0)
			: (
				b != 0
				? new Vector(0, -d / b, 0)
				: new Vector(0, 0, -d / c)
			);
	}

	public HitPoint Intersection(Ray ray)
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

		return new HitPoint(ray, t, normal, material);
	}
}
