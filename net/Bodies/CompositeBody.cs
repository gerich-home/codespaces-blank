using Engine;

namespace Shapes;

public class CompositeBody : IBody
{
	public readonly IBody[] bodies;
	public readonly AABB aabb;

	public CompositeBody(IBody[] bodies)
	{
		this.bodies = bodies;
		aabb = bodies.Any() ?
			bodies.Skip(1)
				.Aggregate(bodies.First().AABB, (a, s) => a.Union(s.AABB)) :
			AABB.MaxValue;
	}

	public ref readonly AABB AABB => ref aabb;

	public BodyHitPoint Intersection(in Ray ray)
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

		BodyHitPoint bestHitPoint = null;
		
		for(int i = 0; i < bodies.Length; i++)
		{
			var body = bodies[i]; 
			if(!body.AABB.CanIntersect(ray, dirInv))
			{
				continue;
			}

			var hitPoint = body.Intersection(ray);
			if(hitPoint != null && (bestHitPoint == null || hitPoint.T < bestHitPoint.T))
			{
				bestHitPoint = hitPoint;
			}
		}

		return bestHitPoint;
	}
}
