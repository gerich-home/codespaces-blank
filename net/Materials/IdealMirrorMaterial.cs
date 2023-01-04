using Engine;

namespace Materials;

public record class IdealMirrorMaterial(): IMaterial {
	public Luminance BRDF(HitPoint hitPoint, in Vector directionToLight) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(HitPoint hitPoint) =>
		new RandomDirection(Luminance.Unit, hitPoint.Reflection);
	
	public bool IsPerfect => true;
}
