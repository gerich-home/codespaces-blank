using Engine;

namespace Materials;

public class SpecularMaterial : IMaterial
{
	public readonly Random rnd;
	public readonly Luminance rs;
	public readonly Luminance factorPart;
	public readonly Luminance brdfPart;
	public readonly double n;

	public static IMaterial Create(Random rnd, Luminance rs, Luminance n)
	{
		if (n.r == n.g)
		{
			if(n.r == n.b)
			{
				return new SpecularMaterial(rnd, rs, n.r);
			}
			else
			{
				var m1rs = rs * new Luminance(1, 1, 0);
				var m2rs = rs * new Luminance(0, 0, 1);

				return CompositeMaterial.Create(
					rnd,
					new SpecularMaterial(rnd, m1rs, n.r),
					new SpecularMaterial(rnd, m2rs, n.b),
					m1rs.Energy,
					m2rs.Energy
				);
			}
		}
		
		if(n.g == n.b)
		{
			var m1rs = rs * new Luminance(0, 1, 1);
			var m2rs = rs * new Luminance(1, 0, 0);

			return CompositeMaterial.Create(
				rnd,
				new SpecularMaterial(rnd, m1rs, n.g),
				new SpecularMaterial(rnd, m2rs, n.r),
				m1rs.Energy,
				m2rs.Energy
			);
		}
		else
		{
			var m1rs = rs * new Luminance(1, 0, 0);
			var m2rs = rs * new Luminance(0, 1, 0);
			var m3rs = rs * new Luminance(0, 0, 1);

			return CompositeMaterial.Create(
				rnd,
				new SpecularMaterial(rnd, m1rs, n.r),
				CompositeMaterial.Create(
					rnd,
					new SpecularMaterial(rnd, m2rs, n.g),
					new SpecularMaterial(rnd, m3rs, n.b),
					m2rs.Energy,
					m3rs.Energy
				),
				m1rs.Energy,
				m2rs.Energy + m3rs.Energy
			);
		}
	}

	private SpecularMaterial(Random rnd, Luminance rs, double n)
	{
		this.rnd = rnd;
		this.rs = rs;
		this.n = n;

		this.brdfPart = rs * (n + 2) / (2 * Math.PI);
		this.factorPart = rs * (1 + 1 / (n + 1));
	}

	public Luminance BRDF(BodyHitPoint hitPoint, in Vector directionToLight)
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
	public RandomDirection SampleDirection(BodyHitPoint hitPoint)
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

	public bool IsPerfect => false;
}
