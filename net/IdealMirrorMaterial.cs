using Engine;

namespace Materials;

public class IdealMirrorMaterial: IMaterial
{
	public readonly Luminance rs; //koefficient specular reflection

	public IdealMirrorMaterial(Luminance rs)
	{
		this.rs = rs;
	}

	public Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
	{
		return Luminance.Zero;
	}

	public RandomDirection SampleDirection(Vector direction, Vector normal, double ksi)
	{
		Vector R = direction - 2 * normal.DotProduct(direction) * normal;

		return new RandomDirection(rs, R);
	}
}