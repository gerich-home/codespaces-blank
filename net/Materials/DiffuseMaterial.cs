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

	public Luminance BRDF(HitPoint hitPoint, in Vector directionToLight) =>
		diffuseResult;

	// BRDF(hitPoint, result.directionToLight) = rd / PI
	// prob(result.directionToLight) = cos(hitPoint.normal, result.directionToLight) / PI
	// result.factor == BRDF(hitPoint, result.directionToLight) * cos(hitPoint.normal, result.directionToLight) / prob(result.direction)
	// -> result.factor == (rd / PI) * cos(hitPoint.normal, result.directionToLight) / (cos(hitPoint.normal, result.directionToLight) / PI)
	// -> result.factor == rd
	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		var directionToLight = rnd.NextSemisphereDirectionCos()
			.Transform(hitPoint.Normal);

		return new RandomDirection(rd, directionToLight);
	}
}
