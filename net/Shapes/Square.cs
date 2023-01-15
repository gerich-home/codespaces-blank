using Engine;

namespace Shapes;

public class Square: ITexturableShape
{
	public readonly Vector a;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly Vector baCrossCa;
	public readonly Vector normal;

	public readonly AABB aabb;
	public readonly bool oneSide;

	public Square(Vector a, Vector b, Vector c, bool oneSide = false)
	{
		this.a = a;
		this.ba = b - a;
		this.ca = c - a;
		this.baCrossCa = ba.CrossProduct(ca);
		this.normal = baCrossCa.Normalized;
		this.oneSide = oneSide;
		aabb = AABB.FromEdgePoints(
			a,
			a + ba, 
			a + ca,
			a + ba + ca 
		);
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
		
		if((t1 < 0) || (1 < t1))
		{
			return null;
		}
					
		var t2 = ca.DotProduct(saCrossDir) * factor;
		
		if((t2 < 0) || (1 < t2))
		{
			return null;
		}

		return new TexturableShapeHitPoint(ray, t, oneSide ? normal : (normal * Math.Sign(divident)), t1, t2);
	}
}
