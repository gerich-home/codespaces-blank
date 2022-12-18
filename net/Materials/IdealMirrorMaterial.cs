using Engine;

namespace Materials;

public record class IdealMirrorMaterial(
	Luminance rs //koefficient specular reflection
): IMaterial {
	public Luminance BRDF(HitPoint hitPoint, Vector ndirection) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi) =>
		new RandomDirection(rs, hitPoint.r);
}
