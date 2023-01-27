using Engine;

namespace Shapes;

public class TransformedShape : IShape
{
	public readonly IShape shape;
	public readonly Matrix worldToModelMatrix;
	public readonly Matrix modelToWorldMatrix;
	public readonly Matrix modelNormalToWorldNormalMatrix;
	public readonly AABB aabb;

	public TransformedShape(IShape shape, Matrix modelToWorldMatrix)
	{
		this.shape = shape;
		this.modelToWorldMatrix = modelToWorldMatrix;
		this.worldToModelMatrix = modelToWorldMatrix.Inverse;
		this.modelNormalToWorldNormalMatrix = worldToModelMatrix.Transpose;
		aabb = shape.AABB.Transform(modelToWorldMatrix);
	}

	public ref readonly AABB AABB => ref aabb;

	public ShapeHitPoint Intersection(in Ray ray)
	{
		var newStart = worldToModelMatrix.MultiplicateByPosition(ray.start);
		var newDirection = worldToModelMatrix.MultiplicateByDirection(ray.direction).Normalized;
		var newRay = newStart.RayAlong(newDirection);

		var hitPoint = shape.Intersection(newRay);
		
		if(hitPoint == null)
		{
			return null;
		}

		return new ShapeHitPoint(ray, hitPoint.T, modelNormalToWorldNormalMatrix.MultiplicateByDirection(hitPoint.Normal).Normalized);
	}
}
