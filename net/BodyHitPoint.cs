namespace Engine;

public class BodyHitPoint
{
	public readonly IMaterial material;
	public readonly ShapeHitPoint shapeHitPoint;

	public BodyHitPoint(ShapeHitPoint shapeHitPoint, IMaterial material)
	{
		this.shapeHitPoint = shapeHitPoint;
		this.material = material;
	}
	
	public double T => shapeHitPoint.T;
	public ref readonly Vector Normal => ref shapeHitPoint.Normal;
	public ref readonly Vector Point => ref shapeHitPoint.Point;
	public ref readonly Vector IncomingDirection => ref shapeHitPoint.IncomingDirection;
	
	public Ray RayAlong(in Vector newDirection, double offset) =>
		(Point + offset * newDirection).RayAlong(newDirection);

	// SampleDirection(ksi).DotProduct(Normal) >= 0
	public RandomDirection SampleDirection() =>
		material.SampleDirection(this);

	// directionToLight.DotProduct(Normal) >= 0
	public Luminance BRDF(in Vector directionToLight) =>
		material.BRDF(this, directionToLight);

	// Reflection.DotProduct(Normal) >= 0
	public Vector Reflection =>
		IncomingDirection - 2 * Normal.DotProduct(IncomingDirection) * Normal;
}
