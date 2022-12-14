using System;
using Engine;

namespace Materials
{
	public class DuffuseSpecularMaterial : IMaterial
	{
		public readonly Luminance rd;
		public readonly Luminance rs;
		public readonly int[] n;

		public DuffuseSpecularMaterial(double[] rd, double[] rs, int[] n)
		{
			this.rd = new Luminance(rd);
			this.rs = new Luminance(rs);
			this.n = n;
		}

		public Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
		{
			Luminance result = rd / Math.PI; 
			
			Vector R = 2 * normal.DotProduct(ndirection) * normal - ndirection;
			double cosphi = -direction.DotProduct(R);
			
			if(cosphi > 0)
			{
				result += new Luminance(
					rs.r == 0 ? 0 : rs.r * (n[0] + 2) * Math.Pow(cosphi, n[0]),
					rs.g == 0 ? 0 : rs.g * (n[1] + 2) * Math.Pow(cosphi, n[1]),
					rs.b == 0 ? 0 : rs.b * (n[2] + 2) * Math.Pow(cosphi, n[2])
					) / (2 * Math.PI);
			}

			return result;
		}

		public RandomDirection SampleDirection(Random rnd, Vector direction, Vector normal, double ksi)
		{	
			double qd = (rd.r + rd.g + rd.b) / 3;
			double qs = (rs.r + rs.g + rs.b) / 3;

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
				double selectedn = Math.Min(n[0], Math.Min(n[1], n[2]));
		
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
					rs.r == 0 ? 0 : rs.r * (n[0] + 2) * Math.Pow(cosa, n[0] - selectedn),
					rs.g == 0 ? 0 : rs.g * (n[1] + 2) * Math.Pow(cosa, n[1] - selectedn),
					rs.b == 0 ? 0 : rs.b * (n[2] + 2) * Math.Pow(cosa, n[2] - selectedn)
					) * normal.DotProduct(ndirection) / (qs * (selectedn + 2)), ndirection);
			}
		}
	}
}