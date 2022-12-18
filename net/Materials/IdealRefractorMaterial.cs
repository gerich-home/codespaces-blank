using Engine;

namespace Materials;

public record class IdealRefractorMaterial(
	Luminance rd,
	double refract
): IMaterial {
	public Luminance BRDF(HitPoint hitPoint, in Vector ndirection)
	{
		int n = 500;

		double cosa = -hitPoint.direction.DotProduct(hitPoint.normal);
		double factor = refract;
		if(cosa < 0)
		{
			factor = 1 / factor;
		}

		double cosb = 1 - (1 - cosa * cosa) * factor * factor;

		if(cosb < 0)
		{
			Vector R = hitPoint.direction + 2 * cosa * hitPoint.normal;
			
			double cosphi = ndirection.DotProduct(R);
		
			if(cosphi <= 0)
			{
				return Luminance.Zero;
			}
			
			return new Luminance(
				rd.r == 0 ? 0 : rd.r * (n + 2) * Math.Pow(cosphi, n),
				rd.g == 0 ? 0 : rd.g * (n + 2) * Math.Pow(cosphi, n),
				rd.b == 0 ? 0 : rd.b * (n + 2) * Math.Pow(cosphi, n)
				) / (2 * Math.PI);	
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
			Vector R = hitPoint.direction + 2 * cosa * hitPoint.normal;
			double cosphi = ndirection.DotProduct(R);
		
			if(cosphi > 0)
			{
				result = qreflect * new Luminance(
					rd.r == 0 ? 0 : rd.r * (n + 2) * Math.Pow(cosphi, n),
					rd.g == 0 ? 0 : rd.g * (n + 2) * Math.Pow(cosphi, n),
					rd.b == 0 ? 0 : rd.b * (n + 2) * Math.Pow(cosphi, n)
					) / (2 * Math.PI);
			}
		}

		{
			Vector R;
			if(cosa > 0)
			{
				R = -cosb * hitPoint.normal + factor * (cosa * hitPoint.normal + hitPoint.direction);
			}
			else	
			{
				R = cosb * hitPoint.normal + factor * (cosa * hitPoint.normal + hitPoint.direction);
			}
			
			double cosphi = ndirection.DotProduct(R);
		
			if(cosphi > 0)
			{
				result = (1 - qreflect) * new Luminance(
					rd.r == 0 ? 0 : rd.r * (n + 2) * Math.Pow(cosphi, n),
					rd.g == 0 ? 0 : rd.g * (n + 2) * Math.Pow(cosphi, n),
					rd.b == 0 ? 0 : rd.b * (n + 2) * Math.Pow(cosphi, n)
					) / (2 * Math.PI);
			}
		}

		return result;
	}

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi) =>
		new RandomDirection(rd, SampleDirectionVector(hitPoint, ksi));

	private Vector SampleDirectionVector(HitPoint hitPoint, double ksi)
	{	
		double cosa = -hitPoint.direction.DotProduct(hitPoint.normal);
		double factor = refract;
		if(cosa < 0)
		{
			factor = 1 / factor;
		}

		double cosb = 1 - (1 - cosa * cosa) * factor * factor;

		if(cosb < 0)
		{
			return hitPoint.direction + 2 * cosa * hitPoint.normal;
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
			return hitPoint.direction + 2 * cosa * hitPoint.normal;
		}
	
		if(cosa > 0)
		{
			return -cosb * hitPoint.normal + factor * (cosa * hitPoint.normal + hitPoint.direction);
		}
		else
		{
			return cosb * hitPoint.normal + factor * (cosa * hitPoint.normal + hitPoint.direction);
		}
	}
}