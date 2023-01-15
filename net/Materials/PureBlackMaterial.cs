using Engine;

namespace Materials;

public class PureBlackMaterial : IMaterial
{
	public Luminance BRDF(BodyHitPoint hitPoint, in Vector directionToLight) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(BodyHitPoint hitPoint) =>
		new RandomDirection(Luminance.Zero, Vector.UnitX);

	public bool IsPerfect => true;
}
