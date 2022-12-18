namespace Engine;

public class HitPoint
{
	public readonly double t;
	public readonly Vector direction;
	public readonly Vector hitPoint;
	public readonly Vector normal;
	public readonly IMaterial material;

	public HitPoint(in Ray ray, double t, in Vector normal, IMaterial material)
	{
		this.t = t;
		this.direction = ray.direction;
		this.hitPoint = ray.PointAt(t);
		this.normal = normal;
		this.material = material;
	}
	
	public Ray RayAlong(in Vector newDirection) =>
		hitPoint.RayAlong(newDirection);

	public RandomDirection SampleDirection(double ksi) =>
		material.SampleDirection(this, ksi);

	public Luminance BRDF(in Vector ndirection) =>
		material.BRDF(this, ndirection);

	public Vector r =>
		direction - 2 * normal.DotProduct(direction) * normal;
}
