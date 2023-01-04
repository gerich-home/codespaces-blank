using Engine;

namespace Materials;

public class IdealRefractorMaterial: IMaterial
{
    public readonly double refractorness;
    public readonly double refractornessInv;

	public IdealRefractorMaterial(double refractorness)
	{
		this.refractorness = refractorness;
		this.refractornessInv = 1 / refractorness;
	}

    public Luminance BRDF(BodyHitPoint hitPoint, in Vector directionToLight) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(BodyHitPoint hitPoint)
	{
		var cos1 = hitPoint.IncomingDirection.DotProduct(hitPoint.Normal);
		var x = hitPoint.Normal * cos1;
		var p = hitPoint.IncomingDirection - x;
		var sin1 = p.Length;
		var sin2 = sin1 * (cos1 > 0 ? refractorness : refractornessInv);
		if(sin2 > 1)
		{
			return new RandomDirection(Luminance.Zero, new Vector(1, 0, 0));
		}
		if (sin1 == 0 || cos1 == 0)
		{
			return new RandomDirection(Luminance.Unit, hitPoint.IncomingDirection);
		}

		var cos2 = Math.Sqrt(1 - sin2 * sin2);
		var v2 = p * (sin2 / sin1) + hitPoint.Normal * cos2 * Math.Sign(cos1);
		return new RandomDirection(Luminance.Unit, v2);
	}

	public bool IsPerfect => true;
}