using Engine;

namespace Materials;

public record class IdealRefractorMaterial(
	Luminance rd,
	double refract
): IMaterial {
	public static readonly double TwoPi = 2 * Math.PI;

	public Luminance BRDF(HitPoint hitPoint, in Vector ndirection)
	{
		int n = 500;

		double cosa = -hitPoint.IncomingDirection.DotProduct(hitPoint.Normal);
		double factor = refract;
		if(cosa < 0)
		{
			factor = 1 / factor;
		}

		double cosb = 1 - (1 - cosa * cosa) * factor * factor;

		if(cosb < 0)
		{
			Vector R = hitPoint.IncomingDirection + 2 * cosa * hitPoint.Normal;
			
			double cosphi = ndirection.DotProduct(R);
		
			if(cosphi <= 0)
			{
				return Luminance.Zero;
			}
			
			return new Luminance(
				rd.r == 0 ? 0 : rd.r * (n + 2) * Math.Pow(cosphi, n),
				rd.g == 0 ? 0 : rd.g * (n + 2) * Math.Pow(cosphi, n),
				rd.b == 0 ? 0 : rd.b * (n + 2) * Math.Pow(cosphi, n)
				) / TwoPi;	
		}

		cosb = Math.Sqrt(cosb);

		double cosabs = Math.Abs(cosa);
		double Rs = (factor * cosabs - cosb) / (factor * cosabs  + cosb);
		Rs *= Rs;

		double Rt = (factor * cosb - cosabs) / (factor * cosb + cosabs);
		Rt *= Rt;
		
		double qreflect = (Rs + Rt) / 2;
		
		Luminance result = Luminance.Zero;
		
		{
			Vector R = hitPoint.IncomingDirection + 2 * cosa * hitPoint.Normal;
			double cosphi = ndirection.DotProduct(R);
		
			if(cosphi > 0)
			{
				result = qreflect * new Luminance(
					rd.r == 0 ? 0 : rd.r * (n + 2) * Math.Pow(cosphi, n),
					rd.g == 0 ? 0 : rd.g * (n + 2) * Math.Pow(cosphi, n),
					rd.b == 0 ? 0 : rd.b * (n + 2) * Math.Pow(cosphi, n)
					) / TwoPi;
			}
		}

		{
			Vector R;
			if(cosa > 0)
			{
				R = -cosb * hitPoint.Normal + factor * (cosa * hitPoint.Normal + hitPoint.IncomingDirection);
			}
			else	
			{
				R = cosb * hitPoint.Normal + factor * (cosa * hitPoint.Normal + hitPoint.IncomingDirection);
			}
			
			double cosphi = ndirection.DotProduct(R);
		
			if(cosphi > 0)
			{
				result = (1 - qreflect) * new Luminance(
					rd.r == 0 ? 0 : rd.r * (n + 2) * Math.Pow(cosphi, n),
					rd.g == 0 ? 0 : rd.g * (n + 2) * Math.Pow(cosphi, n),
					rd.b == 0 ? 0 : rd.b * (n + 2) * Math.Pow(cosphi, n)
					) / TwoPi;
			}
		}

		return result;
	}

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi) =>
		new RandomDirection(rd, SampleDirectionVector(hitPoint, ksi));

	private Vector SampleDirectionVector(HitPoint hitPoint, double ksi)
	{	
		double cosa = -hitPoint.IncomingDirection.DotProduct(hitPoint.Normal);
		double factor = refract;
		if(cosa < 0)
		{
			factor = 1 / factor;
		}

		double cosb = 1 - (1 - cosa * cosa) * factor * factor;

		if(cosb < 0)
		{
			return hitPoint.IncomingDirection + 2 * cosa * hitPoint.Normal;
		}

		cosb = Math.Sqrt(cosb);

		double cosabs = Math.Abs(cosa);
		double Rs = (factor * cosabs - cosb) / (factor * cosabs  + cosb);
		Rs *= Rs;

		double Rt = (factor * cosb - cosabs) / (factor * cosb + cosabs);
		Rt *= Rt;
		
		double qreflect = (Rs + Rt) / 2;
		
		if(ksi < qreflect)
		{
			return hitPoint.IncomingDirection + 2 * cosa * hitPoint.Normal;
		}
	
		if(cosa > 0)
		{
			return -cosb * hitPoint.Normal + factor * (cosa * hitPoint.Normal + hitPoint.IncomingDirection);
		}
		else
		{
			return cosb * hitPoint.Normal + factor * (cosa * hitPoint.Normal + hitPoint.IncomingDirection);
		}
	}
}