namespace Engine;

public record class BodyHitPoint(ShapeHitPoint ShapeHitPoint, IMaterial Material)
{
	public double T => ShapeHitPoint.T;
	public ref readonly Vector Normal => ref ShapeHitPoint.Normal;
	public ref readonly Vector Point => ref ShapeHitPoint.Point;
	public Vector IncomingDirection => ShapeHitPoint.IncomingDirection;
	
	public Ray RayAlong(in Vector newDirection, double offset) =>
		ShapeHitPoint.RayAlong(newDirection, offset);

	// SampleDirection(ksi).DotProduct(Normal) >= 0
	public RandomDirection SampleDirection() =>
		Material.SampleDirection(this);

	// directionToLight.DotProduct(Normal) >= 0
	public Luminance BRDF(in Vector directionToLight) =>
		Material.BRDF(this, directionToLight);

	// Reflection.DotProduct(Normal) >= 0
	public Vector Reflection => ShapeHitPoint.Reflection;
}
