using Engine;

namespace Shapes;

public class Disc: ITexturableShape
{
	public readonly Vector a;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly Vector normal;
	public readonly Vector baCrossCa;
	public readonly double radius;
	public readonly double r2;

	public readonly AABB aabb;
	public readonly bool oneSide;

	public Disc(Vector center, Vector normal, double radius, bool oneSide = false)
	{
		this.a = center;
		this.normal = normal;
		this.radius = radius;
		this.r2 = radius*radius;
		this.oneSide = oneSide;
		aabb = new AABB(
			center - Vector.Unit * radius,
			center + Vector.Unit * radius
		);
		
		const double threshold = 0.57735026919;
		Vector majorAxis;
		if(Math.Abs(normal.x) < threshold)
			majorAxis = Vector.UnitX;
		else if(Math.Abs(normal.y) < threshold)
			majorAxis = Vector.UnitY;
		else
			majorAxis = Vector.UnitZ;

		ba = normal.CrossProduct(majorAxis).Normalized * radius;
		ca = normal.CrossProduct(ba);
		baCrossCa = ba.CrossProduct(ca);
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray) =>
		TexturableIntersection(ray);

	public TexturableShapeHitPoint TexturableIntersection(in Ray ray)
	{
		var divident = -ray.direction.DotProduct(baCrossCa);
		
		if(divident == 0 || oneSide && divident < 0)
		{
			return null;
		}

		var factor = 1 / divident;
		var sa = ray.start - a;
		var t = sa.DotProduct(baCrossCa) * factor;

		if(t < 0)
		{
			return null;
		}

		var saCrossDir = sa.CrossProduct(ray.direction);
		var t1 = -ba.DotProduct(saCrossDir) * factor;
		
		if((t1 < -1) || (1 < t1))
		{
			return null;
		}
					
		var t2 = ca.DotProduct(saCrossDir) * factor;

		if((t2 < -1) || (1 < t2))
		{
			return null;
		}

		if (t1 * t1 + t2 * t2 > 1)
		{
			return null;
		}

		return new TexturableShapeHitPoint(ray, t, oneSide ? normal : (normal * Math.Sign(divident)), 0.5 * (t1 + 1), 0.5 * (t2 + 1));
	}
}
