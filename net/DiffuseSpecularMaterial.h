#pragma once


#define _USE_MATH_DEFINES

using namespace Engine;

namespace Materials
{
	class DuffuseSpecularMaterial: public IMaterial
	{
		DuffuseSpecularMaterial(double rd[], double rs[], int n[]) :
			rd(rd),
			rs(rs)
		{
			this.n[L_R] = n[L_R];
			this.n[L_G] = n[L_G];
			this.n[L_B] = n[L_B];
		}

		const Luminance BRDF(Vector direction, Vector ndirection, Vector normal) const
		{

			Luminance result = rd / M_PI; 
			
			const Vector R = 2 * normal.DotProduct(ndirection) * normal - ndirection;
			const double cosphi = -direction.DotProduct(R);
			
			if(cosphi > 0)
			{
				result += Luminance(
					rs.colors[L_R] == 0 ? 0 : rs.colors[L_R] * (n[L_R] + 2) * pow(cosphi, n[L_R]),
					rs.colors[L_G] == 0 ? 0 : rs.colors[L_G] * (n[L_G] + 2) * pow(cosphi, n[L_G]),
					rs.colors[L_B] == 0 ? 0 : rs.colors[L_B] * (n[L_B] + 2) * pow(cosphi, n[L_B])
					) / (2 * M_PI);
			}

			return result;
		}

		const RandomDirection SampleDirection(Vector direction, Vector normal, double ksi) const
		{	
			double qd = (rd.colors[L_R] + rd.colors[L_G] + rd.colors[L_B]) / 3;
			double qs = (rs.colors[L_R] + rs.colors[L_G] + rs.colors[L_B]) / 3;

			if(qd + qs != 1)
			{
				if(qd + qs == 0)
					return RandomDirection();

				double k = 1 / (qd + qs);
				qd *= k;
				qs *= k;
			}
			
			if(ksi < qd)
			{
				double cosa = (double) rand() / RAND_MAX;
				double sina = sqrt(1 - cosa * cosa);
				double b = 2 * M_PI * (double) rand() / RAND_MAX;

				Vector ndirection = Vector(sina * cos(b), sina * sin(b), cosa).Transform(normal);

				return RandomDirection(rd / (2 * qd), ndirection);
			}
			else
			{
				double selectedn = min(n[L_R], min(n[L_G], n[L_B]));
		
				double cosa = pow((double) rand() / RAND_MAX, 1 / (selectedn + 1));
				double sina = sqrt(1 - cosa * cosa);
				double b = 2 * M_PI * (double) rand() / RAND_MAX;

				const Vector R = direction - 2 * normal.DotProduct(direction) * normal;
				const Vector ndirection = Vector(sina * cos(b), sina * sin(b), cosa).Transform(R);

				if(normal.DotProduct(ndirection) <= 0)
				{
					return RandomDirection();
				}

				return RandomDirection(Luminance(
					rs.colors[L_R] == 0 ? 0 : rs.colors[L_R] * (n[L_R] + 2) * pow(cosa, n[L_R] - selectedn),
					rs.colors[L_G] == 0 ? 0 : rs.colors[L_G] * (n[L_G] + 2) * pow(cosa, n[L_G] - selectedn),
					rs.colors[L_B] == 0 ? 0 : rs.colors[L_B] * (n[L_B] + 2) * pow(cosa, n[L_B] - selectedn)
					) * normal.DotProduct(ndirection) / (qs * (selectedn + 2)), ndirection);
			}
		}

	private:
		const Luminance rd;
		const Luminance rs;
		int n[3];
	};
}