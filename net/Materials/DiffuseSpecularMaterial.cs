using Engine;

namespace Materials;

public record class DuffuseSpecularMaterial(
	Random rnd,
	Luminance rd,
	Luminance rs,
	int[] n
) : IMaterial {
	public Luminance BRDF(HitPoint hitPoint, Vector ndirection)
	{
		Luminance result = rd / Math.PI; 
		
		Vector R = 2 * hitPoint.normal.DotProduct(ndirection) * hitPoint.normal - ndirection;
		double cosphi = -hitPoint.direction.DotProduct(R);
		
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

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{	
		double qd = rd.Energy;
		double qs = rs.Energy;

		if(qd + qs != 1)
		{
			if(qd + qs == 0)
			{
				return new RandomDirection(Luminance.Zero, Vector.Zero);
			}
			
			double k = 1 / (qd + qs);
			qd *= k;
			qs *= k;
		}
		
		if(ksi < qd)
		{
			var (cosa, sina) = rnd.NextCosDistribution();
			double b = rnd.NextDouble(2 * Math.PI);

			var ndirection = new Vector(
				sina * Math.Cos(b),
				sina * Math.Sin(b),
				cosa
			).Transform(hitPoint.normal);

			return new RandomDirection(rd / (2 * qd), ndirection);
		}
		else
		{
			double selectedn = Math.Min(n[0], Math.Min(n[1], n[2]));
	
			double cosa = Math.Pow(rnd.NextDouble(), 1 / (selectedn + 1));
			double sina = Math.Sqrt(1 - cosa * cosa);
			double b = rnd.NextDouble(2 * Math.PI);

			var ndirection = new Vector(
				sina * Math.Cos(b),
				sina * Math.Sin(b),
				cosa
			).Transform(hitPoint.r);

			if(hitPoint.normal.DotProduct(ndirection) <= 0)
			{
				return new RandomDirection(Luminance.Zero, Vector.Zero);
			}

			return new RandomDirection(new Luminance(
				rs.r == 0 ? 0 : rs.r * (n[0] + 2) * Math.Pow(cosa, n[0] - selectedn),
				rs.g == 0 ? 0 : rs.g * (n[1] + 2) * Math.Pow(cosa, n[1] - selectedn),
				rs.b == 0 ? 0 : rs.b * (n[2] + 2) * Math.Pow(cosa, n[2] - selectedn)
				) * hitPoint.normal.DotProduct(ndirection) / (qs * (selectedn + 2)), ndirection);
		}
	}
}
