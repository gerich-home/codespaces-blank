namespace Engine;

public class HitPoint
{
	public readonly double t;
	public readonly Vector direction;
	public readonly Vector hitPoint;
	public readonly Vector normal;
	public readonly IMaterial material;

	public HitPoint(Ray ray, double t, Vector normal, IMaterial material)
	{
		this.t = t;
		this.direction = ray.direction;
		this.hitPoint = ray.PointAt(t);
		this.normal = normal;
		this.material = material;
	}
	
	public Ray RayAlong(Vector newDirection) =>
		hitPoint.RayAlong(newDirection);

	public RandomDirection SampleDirection(double ksi) =>
		material.SampleDirection(direction, normal, ksi);

	public Luminance BRDF(Vector ndirection) =>
		material.BRDF(direction, ndirection, normal);
}
