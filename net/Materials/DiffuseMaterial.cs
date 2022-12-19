using Engine;

namespace Materials;

public class DiffuseMaterial : IMaterial
{
	public readonly Random rnd;
	public readonly Luminance rd;

	public readonly Luminance rdDiv2;
	public readonly Luminance diffuseResult;

	public static readonly double TwoPi = 2 * Math.PI;

	public DiffuseMaterial(Random rnd, Luminance rd)
	{
		this.rnd = rnd;
		this.rd = rd;

		this.diffuseResult = rd / Math.PI;
		this.rdDiv2 = rd / 2;
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector ndirection) =>
		diffuseResult;

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		var (cosa, sina) = rnd.NextCosDistribution();
		var b = rnd.NextDouble(TwoPi);
		var (sinb, cosb) = Math.SinCos(b);

		var ndirection = new Vector(
			sina * cosb,
			sina * sinb,
			cosa
		).Transform(hitPoint.Normal);

		return new RandomDirection(rdDiv2, ndirection);
	}
}
