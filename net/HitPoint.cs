namespace Engine;

public class HitPoint
{
	public readonly double T;
	public readonly Vector incomingDirection; // IncomingDirection.DotProduct(Normal) <= 0
	public readonly Vector point;
	public readonly Vector normal;
	public readonly IMaterial material;
	public readonly IShape shape;

	public HitPoint(in Ray ray, double t, in Vector normal, IMaterial material, IShape shape)
	{
		this.T = t;
		this.incomingDirection = ray.direction;
		this.point = ray.PointAt(t);
		this.normal = normal;
		this.material = material;
		this.shape = shape;
	}
	
	public ref readonly Vector Normal => ref normal;
	public ref readonly Vector Point => ref point;
	public ref readonly Vector IncomingDirection => ref incomingDirection;
	
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
