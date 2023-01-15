using Engine;

namespace Shapes;

public class Triangle: IShape
{
	public readonly Vector baCrossCa;
	public readonly Vector normal;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly Vector a;

	public readonly AABB aabb;
	public readonly bool oneSide;

	public Triangle(Vector a, Vector b, Vector c, bool oneSide = false)
	{
		this.a = a;
		this.ba = b - a;
		this.ca = c - a;
		this.baCrossCa = ba.CrossProduct(ca);
		this.normal = ba.CrossProduct(ca).Normalized;
		this.oneSide = oneSide;
		aabb = AABB.FromEdgePoints(
			a,
			b, 
			c 
		);
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray)
	{
		var divident = -ray.direction.DotProduct(baCrossCa);
		
		if(divident == 0 || oneSide && divident < 0)
		{
			return null;
		}

		var factor = 1 / divident;

		var sa = ray.start - a;
		var saCrossDir = sa.CrossProduct(ray.direction);
		var t1 = -ba.DotProduct(saCrossDir) * factor;
		
		if((t1 < 0) || (1 < t1))
		{
			return null;
		}
					
		var t2 = ca.DotProduct(saCrossDir) * factor;
		
		if((t2 < 0) || (1 < t2))
		{
			return null;
		}
		
		if((t2 + t1) > 1)
		{
			return null;
		}
		
		var t = sa.DotProduct(baCrossCa) * factor;

		if(t < 0)
		{
			return null;
		}

		return new ShapeHitPoint(ray, t, oneSide ? normal : (normal * Math.Sign(divident)));
	}
}
