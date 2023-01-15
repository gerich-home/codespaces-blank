using Engine;

namespace Bodies;

public record class AABBBody(IBody inner): IBody
{
	public ref readonly AABB AABB => ref inner.AABB;

	public BodyHitPoint Intersection(in Ray ray)
	{
		var dirInv = ray.InvDirection;
		
		if (!inner.AABB.CanIntersect(ray, dirInv))
		{
			return null;
		}

		return inner.Intersection(ray);
	}
}
