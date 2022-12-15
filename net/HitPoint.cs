namespace Engine
{
	public record class HitPoint(
		double t,
		Vector normal,
		IMaterial material
	);
}