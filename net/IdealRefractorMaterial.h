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

		const Luminance BRDF(Vector direction, Vector ndirection, Vector normal) const
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
				const Vector R = direction + 2 * cosa * normal;
				
				const double cosphi = ndirection.DotProduct(R);
			
				if(cosphi > 0)
				{
					return Luminance(
						rd.colors[L_R] == 0 ? 0 : rd.colors[L_R] * (n + 2) * pow(cosphi, n),
						rd.colors[L_G] == 0 ? 0 : rd.colors[L_G] * (n + 2) * pow(cosphi, n),
						rd.colors[L_B] == 0 ? 0 : rd.colors[L_B] * (n + 2) * pow(cosphi, n)
						) / (2 * M_PI);	
				}
				else
				{
					return Luminance();
				}
			}

			cosb = sqrt(cosb);

			double cosabs = abs(cosa);
			double Rs = (factor * cosabs - cosb) / (factor * cosabs  + cosb);
			Rs *= Rs;

			double Rt = (factor * cosb - cosabs) / (factor * cosb + cosabs);
			Rt *= Rt;
			
			double qreflect = (Rs + Rt) / 2;
			
			Luminance result;
			
			{
				const Vector R = direction + 2 * cosa * normal;
				const double cosphi = ndirection.DotProduct(R);
			
				if(cosphi > 0)
				{
					result = qreflect * Luminance(
						rd.colors[L_R] == 0 ? 0 : rd.colors[L_R] * (n + 2) * pow(cosphi, n),
						rd.colors[L_G] == 0 ? 0 : rd.colors[L_G] * (n + 2) * pow(cosphi, n),
						rd.colors[L_B] == 0 ? 0 : rd.colors[L_B] * (n + 2) * pow(cosphi, n)
						) / (2 * M_PI);
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
				
				const double cosphi = ndirection.DotProduct(R);
			
				if(cosphi > 0)
				{
					result = (1 - qreflect) * Luminance(
						rd.colors[L_R] == 0 ? 0 : rd.colors[L_R] * (n + 2) * pow(cosphi, n),
						rd.colors[L_G] == 0 ? 0 : rd.colors[L_G] * (n + 2) * pow(cosphi, n),
						rd.colors[L_B] == 0 ? 0 : rd.colors[L_B] * (n + 2) * pow(cosphi, n)
						) / (2 * M_PI);
				}
			}

			return result;
		}

		const RandomDirection SampleDirection(Vector direction, Vector normal, double ksi) const
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
				const Vector R = direction + 2 * cosa * normal;
				return RandomDirection(rd, R);	
			}

			cosb = sqrt(cosb);

			double cosabs = abs(cosa);
			double Rs = (factor * cosabs - cosb) / (factor * cosabs  + cosb);
			Rs *= Rs;

			double Rt = (factor * cosb - cosabs) / (factor * cosb + cosabs);
			Rt *= Rt;
			
			double qreflect = (Rs + Rt) / 2;
			
			if(ksi < qreflect)
			{
				const Vector R = direction + 2 * cosa * normal;
				return RandomDirection(rd, R);	
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
				
				return RandomDirection(rd, R);
			}
		}

	private:
		const Luminance rd;
		const double refract;
	};
}