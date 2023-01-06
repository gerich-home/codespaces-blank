using Engine;

namespace Shapes;

public class Square: ITexturableShape
{
	public readonly Vector a;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly Vector normalMulArea;
	public readonly Vector normal;

	public readonly AABB aabb;

	public Square(Vector a, Vector b, Vector c)
	{
		this.a = a;
		this.ba = b - a;
		this.ca = c - a;
		this.normalMulArea = (b - a).CrossProduct(c - a);
		this.normal = normalMulArea.Normalized;
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
		var divident = -ray.direction.DotProduct(normalMulArea);
		
		if(divident <= 0) // <0 leads to ignoring intersections from inside
		{
			return null;
		}

		var factor = 1 / divident;
		var sa = ray.start - a;
		var t = sa.DotProduct(normalMulArea) * factor;

		if(t < 0)
		{
			return null;
		}

		var saxdir = sa.CrossProduct(ray.direction);
		var t1 = -ba.DotProduct(saxdir) * factor;
		
		if((t1 < 0) || (1 < t1))
		{
			return null;
		}
					
		var t2 = ca.DotProduct(saxdir) * factor;
		
		if((t2 < 0) || (1 < t2))
		{
			return null;
		}

		return new TexturableShapeHitPoint(ray, t, normal, t1, t2);
	}
}
