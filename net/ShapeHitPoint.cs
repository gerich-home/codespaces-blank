namespace Engine;

public class ShapeHitPoint
{
	public readonly double T;
	private readonly Ray incomingRay; // IncomingDirection.DotProduct(Normal) <= 0
	private readonly Vector point;
	private readonly Vector normal;

	public ShapeHitPoint(in Ray incomingRay, double t, in Vector normal)
	{
		this.T = t;
		this.incomingRay = incomingRay;
		this.point = incomingRay.PointAt(t);
		this.normal = normal;
	}
	
	public ref readonly Vector Normal => ref normal;
	public ref readonly Vector Point => ref point;
	public ref readonly Ray IncomingRay => ref incomingRay;
	public Vector IncomingDirection => incomingRay.direction;
	
	public Ray RayAlong(in Vector newDirection, double offset) =>
		(Point + offset * newDirection).RayAlong(newDirection);

	// Reflection.DotProduct(Normal) >= 0
	public Vector Reflection =>
		IncomingDirection - 2 * Normal.DotProduct(IncomingDirection) * Normal;
}
