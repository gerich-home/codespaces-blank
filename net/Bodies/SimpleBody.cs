using Engine;

namespace Bodies;

public record class SimpleBody(IShape shape, IMaterial material): IBody
{
	public ref readonly AABB AABB => ref shape.AABB;

	public BodyHitPoint Intersection(in Ray ray)
	{
		var shapeHitPoint = shape.Intersection(ray);

		if(shapeHitPoint == null) 
		{
			return null;
		}

		return new BodyHitPoint(shapeHitPoint, material);
	}
}
