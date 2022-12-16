using Engine;

namespace Materials;

public record class IdealRefractorMaterial(
	Luminance rd,
	double refract
): IMaterial {
	public Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
	{
		int n = 500;

		double cosa = -direction.DotProduct(normal);
		double factor = refract;
		if(cosa < 0)
		{
			factor = 1 / factor;
		}

		double cosb = 1 - (1 - cosa * cosa) * factor * factor;

		if(cosb < 0)
		{
			Vector R = direction + 2 * cosa * normal;
			
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
			Vector R = direction + 2 * cosa * normal;
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
				R = -cosb * normal + factor * (cosa * normal + direction);
			}
			else	
			{
				R = cosb * normal + factor * (cosa * normal + direction);
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

	public RandomDirection SampleDirection(Vector direction, Vector normal, double ksi) =>
		new RandomDirection(rd, SampleDirectionVector(direction, normal, ksi));

	private Vector SampleDirectionVector(Vector direction, Vector normal, double ksi)
	{	
		double cosa = -direction.DotProduct(normal);
		double factor = refract;
		if(cosa < 0)
		{
			factor = 1 / factor;
		}

		double cosb = 1 - (1 - cosa * cosa) * factor * factor;

		if(cosb < 0)
		{
			return direction + 2 * cosa * normal;
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
			return direction + 2 * cosa * normal;
		}
	
		if(cosa > 0)
		{
			return -cosb * normal + factor * (cosa * normal + direction);
		}
		else
		{
			return cosb * normal + factor * (cosa * normal + direction);
		}
	}
}