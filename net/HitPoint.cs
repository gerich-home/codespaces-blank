namespace Engine;

public class HitPoint
{
	public readonly double T;
	public readonly Vector IncomingDirection;
	public readonly Vector Point;
	public readonly Vector Normal;
	public readonly IMaterial material;

	public HitPoint(in Ray ray, double t, in Vector normal, IMaterial material)
	{
		this.T = t;
		this.IncomingDirection = ray.direction;
		this.Point = ray.PointAt(t);
		this.Normal = normal;
		this.material = material;
	}
	
	public Ray RayAlong(in Vector newDirection) =>
		Point.RayAlong(newDirection);

	public RandomDirection SampleDirection(double ksi) =>
		material.SampleDirection(this, ksi);

	public Luminance BRDF(in Vector directionToLight) =>
		material.BRDF(this, directionToLight);

	public Vector Reflection =>
		IncomingDirection - 2 * Normal.DotProduct(IncomingDirection) * Normal;
}
