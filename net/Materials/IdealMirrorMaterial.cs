using Engine;

namespace Materials;

public record class IdealMirrorMaterial(): IMaterial {
	public Luminance BRDF(BodyHitPoint hitPoint, in Vector directionToLight) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(BodyHitPoint hitPoint) =>
		new RandomDirection(Luminance.Unit, hitPoint.Reflection);
	
	public bool IsPerfect => true;
}
