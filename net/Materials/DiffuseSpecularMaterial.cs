using Engine;

namespace Materials;

public class DuffuseSpecularMaterial : IMaterial
{
	public readonly Random rnd;
	public readonly Luminance rd;
	public readonly Luminance rs;
	public readonly Luminance n;

	public readonly Luminance diffuseResult;
	public readonly Luminance rsDiv2PiMulNPlus2;
	public readonly bool IsPureDiffuse;

	public DuffuseSpecularMaterial(Random rnd, Luminance rd, Luminance rs, Luminance n)
	{
		this.rnd = rnd;
		this.rd = rd;
		this.rs = rs;
		this.n = n;


		this.diffuseResult = rd / Math.PI;
		this.rsDiv2PiMulNPlus2 = (n + new Luminance(2, 2, 2)) * rs / (2 * Math.PI);
		this.IsPureDiffuse = rs.IsZero;
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector ndirection)
	{
		if (IsPureDiffuse)
		{
			return diffuseResult;
		}

		Vector R = 2 * hitPoint.normal.DotProduct(ndirection) * hitPoint.normal - ndirection;
		double cosphi = -hitPoint.direction.DotProduct(R);
		
		if (cosphi <= 0)
		{
			return diffuseResult;
		}

		var specularResult = new Luminance(
			rsDiv2PiMulNPlus2.r == 0 ? 0 : rsDiv2PiMulNPlus2.r * Math.Pow(cosphi, n.r),
			rsDiv2PiMulNPlus2.g == 0 ? 0 : rsDiv2PiMulNPlus2.g * Math.Pow(cosphi, n.g),
			rsDiv2PiMulNPlus2.b == 0 ? 0 : rsDiv2PiMulNPlus2.b * Math.Pow(cosphi, n.b)
		);

		return diffuseResult + specularResult;
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
			double selectedn = Math.Min(n.r, Math.Min(n.g, n.b));
	
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
				rs.r == 0 ? 0 : rs.r * (n.r + 2) * Math.Pow(cosa, n.r - selectedn),
				rs.g == 0 ? 0 : rs.g * (n.g + 2) * Math.Pow(cosa, n.g - selectedn),
				rs.b == 0 ? 0 : rs.b * (n.b + 2) * Math.Pow(cosa, n.b - selectedn)
				) * hitPoint.normal.DotProduct(ndirection) / (qs * (selectedn + 2)), ndirection);
		}
	}
}
