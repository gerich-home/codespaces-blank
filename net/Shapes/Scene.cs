using Engine;

namespace Shapes;

public class Scene : IShape
{
	public readonly IShape[] shapes;

	public Scene(IShape[] shapes)
	{
		this.shapes = shapes;
	}

	public HitPoint Intersection(IShape shape, in Ray ray)
	{
		HitPoint bestHitPoint = null;
		HitPoint hitPoint;

		for(int i = 0; i < shapes.Length; i++)
		{
			hitPoint = shapes[i].Intersection(shape, ray);
			if(hitPoint != null && (bestHitPoint == null || hitPoint.T < bestHitPoint.T))
			{
				bestHitPoint = hitPoint;
			}
		}

		return bestHitPoint;
	}

}
