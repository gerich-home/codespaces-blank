using Engine;

namespace Materials;

public class SpecularMaterial : IMaterial
{
	public readonly Random rnd;
	public readonly Luminance rs;
	public readonly Luminance factorPart;
	public readonly Luminance brdfPart;
	public readonly double n;

	public SpecularMaterial(Random rnd, Luminance rs, double n)
	{
		this.rnd = rnd;
		this.rs = rs;
		this.n = n;

		this.brdfPart = rs * (n + 2) / (2 * Math.PI);
		this.factorPart = rs * (1 + 1 / (n + 1));
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector directionToLight)
	{
		var cosPhi = hitPoint.Reflection.DotProduct(directionToLight);
		
		if (cosPhi <= 0)
		{
			return Luminance.Zero;
		}

		return brdfPart * Math.Pow(cosPhi, n);
	}

	// BRDF(hitPoint, result.directionToLight) = rs * (n + 2) * cos(hitPoint.R, result.directionToLight) ^ n / (2 * PI)
	// prob(result.directionToLight) = cos(hitPoint.R, result.directionToLight) ^ n * (n + 1) / (2 * PI)
	// result.factor == BRDF(hitPoint, result.directionToLight) * cos(hitPoint.normal, result.directionToLight) / prob(result.direction)
	// -> result.factor == rs * (n + 2) * cos(hitPoint.R, result.directionToLight) ^ n / (2 * PI) * cos(hitPoint.normal, result.directionToLight) / (cos(hitPoint.R, result.directionToLight) ^ n * (n + 1) / (2 * PI))
	// -> result.factor == rs * (1 + 1 / (n + 1)) * cos(hitPoint.normal, result.directionToLight)
	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		var directionToLight = rnd.NextSemisphereDirectionPhong(n)
			.Transform(hitPoint.Reflection);

		var cosTheta = hitPoint.Normal.DotProduct(directionToLight);

		if (cosTheta <= 0)
		{
			return new RandomDirection(Luminance.Zero, Vector.Zero);
		}

		return new RandomDirection(factorPart * cosTheta, directionToLight);
	}
}
