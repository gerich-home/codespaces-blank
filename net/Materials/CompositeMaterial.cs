using Engine;

namespace Materials;

public class CompositeMaterial : IMaterial
{
	public readonly double qd;
	public readonly IMaterial d;
	public readonly IMaterial s;

	public CompositeMaterial(IMaterial d, IMaterial s, double qd)
	{
		this.d = d;
		this.s = s;
		this.qd = qd;
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector directionToLight) =>
		d.BRDF(hitPoint, directionToLight) + s.BRDF(hitPoint, directionToLight);

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		if (ksi < qd)
		{
			return d.SampleDirection(hitPoint, ksi / qd) / qd;
		}
		else
		{
			return s.SampleDirection(hitPoint, ksi / (1 - qd)) / (1 - qd);
		}
	}
}
