using Engine;

namespace Materials;

public class DiffuseMaterial : IMaterial
{
	public readonly Random rnd;
	public readonly Luminance rd;
	public readonly Luminance diffuseResult;

	public DiffuseMaterial(Random rnd, Luminance rd)
	{
		this.rnd = rnd;
		this.rd = rd;

		this.diffuseResult = rd / Math.PI;
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector ndirection) =>
		diffuseResult;


	// BRDF(hitPoint) * cos(hitPoint.normal, outDirection) / prob(outDirection)
	// BRDF(hitPoint) = rd / PI
	// prob(outgoingDirection) = cos(hitPoint.normal, outDirection) / PI
	// result = rd
	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		var ndirection = rnd.NextSemisphereDirectionCos()
			.Transform(hitPoint.Normal);

		return new RandomDirection(rd, ndirection);
	}
}
