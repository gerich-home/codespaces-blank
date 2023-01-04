namespace Engine;

public class TexturableShapeHitPoint : ShapeHitPoint
{
	public readonly double T1;
	public readonly double T2;

	public TexturableShapeHitPoint(in Ray ray, double t, in Vector normal, double t1, double t2)
		: base(ray, t, normal)
	{
		T1 = t1;
		T2 = t2;
	}
}