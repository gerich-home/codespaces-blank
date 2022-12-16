using Engine;

namespace Materials;

public record class IdealMirrorMaterial(
	Luminance rs //koefficient specular reflection
): IMaterial {
	public Luminance BRDF(Vector direction, Vector ndirection, Vector normal) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(Vector direction, Vector normal, double ksi) =>
		new RandomDirection(rs, direction - 2 * normal.DotProduct(direction) * normal);
}
