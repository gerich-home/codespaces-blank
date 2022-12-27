using Engine;

namespace Materials;

public class CompositeMaterial : IMaterial
{
	public readonly double p1;
	public readonly double m1Factor;
	public readonly double m2Factor;
	public readonly IMaterial m1;
	public readonly IMaterial m2;

	public static IMaterial Create(IMaterial m1, IMaterial m2, double m1Energy, double m2Energy)
	{
		if (m1Energy == 0 && m2Energy == 0)
		{
			return new PureBlackMaterial();
		}

		if(m2Energy == 0)
		{
			return m1;
		}
		
		if(m1Energy == 0)
		{
			return m2;
		}

		return new CompositeMaterial(m1, m2, m1Energy, m2Energy);
	}

	private CompositeMaterial(IMaterial m1, IMaterial m2, double m1Energy, double m2Energy)
	{
		this.m1 = m1;
		this.m2 = m2;
		this.p1 = m1Energy / (m1Energy + m2Energy);
		this.m1Factor = 1 / p1;
		this.m2Factor = 1 / (1 - p1);
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector directionToLight) =>
		m1.BRDF(hitPoint, directionToLight) + m2.BRDF(hitPoint, directionToLight);

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		if (ksi < p1)
		{
			return m1.SampleDirection(hitPoint, ksi * m1Factor) * m1Factor;
		}
		else
		{
			return m2.SampleDirection(hitPoint, ksi * m2Factor) * m2Factor;
		}
	}
}
