namespace Engine;

public record class LightHitPoint(ShapeHitPoint ShapeHitPoint, Luminance Le)
{
	public double T => ShapeHitPoint.T;
}
