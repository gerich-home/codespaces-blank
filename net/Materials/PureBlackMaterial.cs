using Engine;

namespace Materials;

public class PureBlackMaterial : IMaterial
{
	public Luminance BRDF(HitPoint hitPoint, in Vector directionToLight) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(HitPoint hitPoint) =>
		new RandomDirection(Luminance.Zero, new Vector(1, 0, 0));

	public bool IsPerfect => true;
}
