#pragma once


#define _USE_MATH_DEFINES

using namespace Engine;

namespace Materials
{
	class IdealRefractorMaterial: public IMaterial
	{
		IdealRefractorMaterial(double rd[], double refract) :
			rd(rd),
			refract(refract)
		{
		}

		Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
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
				readonly Vector R = direction + 2 * cosa * normal;
				
				readonly double cosphi = ndirection.DotProduct(R);
			
				if(cosphi > 0)
				{
					return new Luminance(
						rd.colors[L_R] == 0 ? 0 : rd.colors[L_R] * (n + 2) * Math.Pow(cosphi, n),
						rd.colors[L_G] == 0 ? 0 : rd.colors[L_G] * (n + 2) * Math.Pow(cosphi, n),
						rd.colors[L_B] == 0 ? 0 : rd.colors[L_B] * (n + 2) * Math.Pow(cosphi, n)
						) / (2 * Math.PI);	
				}
				else
				{
					return new Luminance();
				}
			}

			cosb = Math.Sqrt(cosb);

			double cosabs = Math.Abs(cosa);
			double Rs = (factor * cosabs - cosb) / (factor * cosabs  + cosb);
			Rs *= Rs;

			double Rt = (factor * cosb - cosabs) / (factor * cosb + cosabs);
			Rt *= Rt;
			
			double qreflect = (Rs + Rt) / 2;
			
			Luminance result;
			
			{
				readonly Vector R = direction + 2 * cosa * normal;
				readonly double cosphi = ndirection.DotProduct(R);
			
				if(cosphi > 0)
				{
					result = qreflect * new Luminance(
						rd.colors[L_R] == 0 ? 0 : rd.colors[L_R] * (n + 2) * Math.Pow(cosphi, n),
						rd.colors[L_G] == 0 ? 0 : rd.colors[L_G] * (n + 2) * Math.Pow(cosphi, n),
						rd.colors[L_B] == 0 ? 0 : rd.colors[L_B] * (n + 2) * Math.Pow(cosphi, n)
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
				
				readonly double cosphi = ndirection.DotProduct(R);
			
				if(cosphi > 0)
				{
					result = (1 - qreflect) * new Luminance()
						rd.colors[L_R] == 0 ? 0 : rd.colors[L_R] * (n + 2) * Math.Pow(cosphi, n),
						rd.colors[L_G] == 0 ? 0 : rd.colors[L_G] * (n + 2) * Math.Pow(cosphi, n),
						rd.colors[L_B] == 0 ? 0 : rd.colors[L_B] * (n + 2) * Math.Pow(cosphi, n)
						) / (2 * Math.PI);
				}
			}

			return result;
		}

		RandomDirection SampleDirection(Vector direction, Vector normal, double ksi)
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
				readonly Vector R = direction + 2 * cosa * normal;
				return new RandomDirection(rd, R);	
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
				readonly Vector R = direction + 2 * cosa * normal;
				return new RandomDirection(rd, R);	
			}
			else
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
				
				return new RandomDirection(rd, R);
			}
		}

	private:
		readonly Luminance rd;
		readonly double refract;
	};
}