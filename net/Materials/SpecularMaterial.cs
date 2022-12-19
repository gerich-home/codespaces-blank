using Engine;

namespace Materials;

public class SpecularMaterial : IMaterial
{
	public readonly Random rnd;
	public readonly Luminance rs;
	public readonly Luminance n;

	public readonly Luminance rsMulNPlus2;
	public readonly Luminance rsMulNPlus2Div2Pi;

	public readonly double selectedN;
	public readonly double selectedNPlus1Inv;
	public readonly double selectedNPlus2Inv;
	public readonly Luminance nMinusSelectedN;

	public static readonly double TwoPi = 2 * Math.PI;

	public SpecularMaterial(Random rnd, Luminance rs, Luminance n)
	{
		this.rnd = rnd;
		this.rs = rs;
		this.n = n;

		this.rsMulNPlus2 = (n + 2 * Luminance.Unit) * rs;
		this.rsMulNPlus2Div2Pi = this.rsMulNPlus2 / TwoPi;
		
		this.selectedN = Math.Min(n.r, Math.Min(n.g, n.b));
		this.nMinusSelectedN = n - selectedN * Luminance.Unit;
		this.selectedNPlus1Inv = 1 / (selectedN + 1);
		this.selectedNPlus2Inv = 1 / (selectedN + 2);
	}

	public Luminance BRDF(HitPoint hitPoint, in Vector ndirection)
	{
		var R = 2 * hitPoint.Normal.DotProduct(ndirection) * hitPoint.Normal - ndirection;
		var cosphi = -hitPoint.IncomingDirection.DotProduct(R);
		
		if (cosphi <= 0)
		{
			return Luminance.Zero;
		}

		return new Luminance(
			rsMulNPlus2Div2Pi.r == 0 ? 0 : rsMulNPlus2Div2Pi.r * Math.Pow(cosphi, n.r),
			rsMulNPlus2Div2Pi.g == 0 ? 0 : rsMulNPlus2Div2Pi.g * Math.Pow(cosphi, n.g),
			rsMulNPlus2Div2Pi.b == 0 ? 0 : rsMulNPlus2Div2Pi.b * Math.Pow(cosphi, n.b)
		);
	}

	public RandomDirection SampleDirection(HitPoint hitPoint, double ksi)
	{
		var cosa = Math.Pow(rnd.NextDouble(), selectedNPlus1Inv);
		var sina = Math.Sqrt(1 - cosa * cosa);
		var b = rnd.NextDouble(TwoPi);
		var (sinb, cosb) = Math.SinCos(b);

		var ndirection = new Vector(
			sina * cosb,
			sina * sinb,
			cosa
		).Transform(hitPoint.Reflection);

		if(hitPoint.Normal.DotProduct(ndirection) <= 0)
		{
			return new RandomDirection(Luminance.Zero, Vector.Zero);
		}

		return new RandomDirection(new Luminance(
			rs.r == 0 ? 0 : rsMulNPlus2.r * Math.Pow(cosa, nMinusSelectedN.r),
			rs.g == 0 ? 0 : rsMulNPlus2.g * Math.Pow(cosa, nMinusSelectedN.g),
			rs.b == 0 ? 0 : rsMulNPlus2.b * Math.Pow(cosa, nMinusSelectedN.b)
			) * (selectedNPlus2Inv * hitPoint.Normal.DotProduct(ndirection)), ndirection);
	}
}
