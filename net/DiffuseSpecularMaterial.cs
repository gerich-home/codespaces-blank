using Engine;

namespace Materials
{
	class DuffuseSpecularMaterial : IMaterial
	{
		readonly Luminance rd;
		readonly Luminance rs;
		readonly int[] n;

		DuffuseSpecularMaterial(double rd[], double rs[], int n[])
		{
			this.rd = rd;
			this.rs = rs;
			this.n = new int[3];
			this.n[L_R] = n[L_R];
			this.n[L_G] = n[L_G];
			this.n[L_B] = n[L_B];
		}

		Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
		{

			Luminance result = rd / Math.PI; 
			
			Vector R = 2 * normal.DotProduct(ndirection) * normal - ndirection;
			double cosphi = -direction.DotProduct(R);
			
			if(cosphi > 0)
			{
				result += new Luminance(
					rs.colors[L_R] == 0 ? 0 : rs.colors[L_R] * (n[L_R] + 2) * Math.Pow(cosphi, n[L_R]),
					rs.colors[L_G] == 0 ? 0 : rs.colors[L_G] * (n[L_G] + 2) * Math.Pow(cosphi, n[L_G]),
					rs.colors[L_B] == 0 ? 0 : rs.colors[L_B] * (n[L_B] + 2) * Math.Pow(cosphi, n[L_B])
					) / (2 * Math.PI);
			}

			return result;
		}

		RandomDirection SampleDirection(Vector direction, Vector normal, double ksi)
		{	
			double qd = (rd.colors[L_R] + rd.colors[L_G] + rd.colors[L_B]) / 3;
			double qs = (rs.colors[L_R] + rs.colors[L_G] + rs.colors[L_B]) / 3;

			if(qd + qs != 1)
			{
				if(qd + qs == 0)
					return new RandomDirection();

				double k = 1 / (qd + qs);
				qd *= k;
				qs *= k;
			}
			
			if(ksi < qd)
			{
				double cosa = rnd.NextDouble();
				double sina = Math.Sqrt(1 - cosa * cosa);
				double b = 2 * Math.PI * rnd.NextDouble();

				Vector ndirection = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(normal);

				return new RandomDirection(rd / (2 * qd), ndirection);
			}
			else
			{
				double selectedn = Math.Min(n[L_R], Math.Min(n[L_G], n[L_B]));
		
				double cosa = Math.Pow(rnd.NextDouble(), 1 / (selectedn + 1));
				double sina = Math.Sqrt(1 - cosa * cosa);
				double b = 2 * Math.PI * rnd.NextDouble();

				Vector R = direction - 2 * normal.DotProduct(direction) * normal;
				Vector ndirection = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(R);

				if(normal.DotProduct(ndirection) <= 0)
				{
					return new RandomDirection();
				}

				return new RandomDirection(new Luminance(
					rs.colors[L_R] == 0 ? 0 : rs.colors[L_R] * (n[L_R] + 2) * Math.Pow(cosa, n[L_R] - selectedn),
					rs.colors[L_G] == 0 ? 0 : rs.colors[L_G] * (n[L_G] + 2) * Math.Pow(cosa, n[L_G] - selectedn),
					rs.colors[L_B] == 0 ? 0 : rs.colors[L_B] * (n[L_B] + 2) * Math.Pow(cosa, n[L_B] - selectedn)
					) * normal.DotProduct(ndirection) / (qs * (selectedn + 2)), ndirection);
			}
		}
	}
}