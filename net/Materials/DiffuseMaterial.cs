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
		var ndirection = rnd.NextSemisphereDirectionUniform()
			.Transform(hitPoint.Normal);

		return new RandomDirection(rdDiv2, ndirection);
	}
}
