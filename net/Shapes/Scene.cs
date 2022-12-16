using Engine;

namespace Shapes;

public class Scene : IShape
{
	public readonly IShape[] shapes;

	public Scene(IShape[] shapes)
	{
		this.shapes = shapes;
	}

	public HitPoint Intersection(Ray ray)
	{
		HitPoint bestHitPoint = null;
		HitPoint hitPoint;

		for(int i = 0; i < shapes.Length; i++)
		{
			hitPoint = shapes[i].Intersection(ray);
			if(hitPoint != null && (bestHitPoint == null || hitPoint.t < bestHitPoint.t))
			{
				bestHitPoint = hitPoint;
			}
		}

		return bestHitPoint;
	}

}
