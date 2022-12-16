namespace Engine;

public class HitPoint
{
	public readonly double t;
	public readonly Ray ray;
	public readonly Vector hitPoint;
	public readonly Vector normal;
	public readonly IMaterial material;

	public HitPoint(Ray ray, double t, Vector normal, IMaterial material)
	{
		this.t = t;
		this.ray = ray;
		this.hitPoint = ray.PointAt(t);
		this.normal = normal;
		this.material = material;
	}
}
