using Engine;

namespace Shapes;

public class CompositeShape : IShape
{
	public readonly IShape[] shapes;
	public readonly AABB aabb;
	
	public static IShape Create(params IShape[] shapes)
	{
		if (shapes.Length == 1)
		{
			return shapes[0];
		}

		return new CompositeShape(shapes);
	}

	private CompositeShape(IShape[] shapes)
	{
		this.shapes = shapes;
		aabb = shapes.Any() ?
			shapes.Skip(1)
				.Aggregate(shapes.First().AABB, (a, s) => a.Union(s.AABB)) :
			AABB.MaxValue;
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray)
	{
		var dirInv = ray.InvDirection;
		
		if (!AABB.CanIntersect(ray, dirInv))
		{
			return null;
		}

		ShapeHitPoint bestHitPoint = null;
		
		for(int i = 0; i < shapes.Length; i++)
		{
			var shape = shapes[i]; 
			if(!shape.AABB.CanIntersect(ray, dirInv))
			{
				continue;
			}

			var hitPoint = shape.Intersection(ray);
			if(hitPoint != null && (bestHitPoint == null || hitPoint.T < bestHitPoint.T))
			{
				bestHitPoint = hitPoint;
			}
		}

		return bestHitPoint;
	}
}
