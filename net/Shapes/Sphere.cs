using Engine;

namespace Shapes;

public class Sphere : IShape
{
	public readonly double r2;
	public readonly double rinv;
	public readonly Vector center;
	public readonly IMaterial material;

	public Sphere(Vector center, double r, IMaterial material)
	{
		this.center = center;
		this.material = material;
		this.r2 = r * r;
		this.rinv = 1 / r;
	}

	public HitPoint Intersection(Ray ray)
	{
		Vector ac = ray.start - center;
		
		double b = ac.DotProduct(ray.direction);
		double c = ac.Norm - r2;

		double D = b * b - c;

		if(D < 0)
		{
			return null;
		}

		D = Math.Sqrt(D);
		double t = -b - D;
		
		if(t < double.Epsilon)
		{
			t = -b + D;
		
			if(t < double.Epsilon)
			{
				return null;
			}
		}

		return new HitPoint(ray, t, (ray.PointAt(t) - center) * rinv, material);
	}
}
