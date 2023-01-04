using Engine;

namespace Shapes;

public class Scene : IShape
{
	public readonly IShape[] shapes;
	public readonly AABB aabb;

	public Scene(IShape[] shapes)
	{
		this.shapes = shapes;
		aabb = shapes.Any() ?
			shapes.Skip(1)
				.Aggregate(shapes.First().AABB, (a, s) => a.Union(s.AABB)) :
			AABB.MaxValue;
	}

	public ref readonly AABB AABB => ref aabb;

	public HitPoint Intersection(in Ray ray)
	{
		var dirInv = new Vector(
			1 / ray.direction.x,
			1 / ray.direction.y,
			1 / ray.direction.z
		);
		
		if (!AABB.CanIntersect(ray, dirInv))
		{
			return null;
		}

		HitPoint bestHitPoint = null;
		
		for(int i = 0; i < shapes.Length; i++)
		{
			var shape = shapes[i]; 
			if(!shape.AABB.CanIntersect(ray, in dirInv))
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
