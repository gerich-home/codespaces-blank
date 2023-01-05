using Engine;

namespace Materials;

public class SchlickCompositeMaterial : IMaterial
{
	public readonly double r0;
	public readonly IdealMirrorMaterial mirrorMaterial;
	public readonly IdealRefractorMaterial refractorMaterial;
	public readonly Random rnd;

	public SchlickCompositeMaterial(Random rnd, IdealRefractorMaterial refractorMaterial)
	{
		this.rnd = rnd;
		this.mirrorMaterial = new IdealMirrorMaterial();
		this.refractorMaterial = refractorMaterial;
		this.r0 = Math.Pow((1 - refractorMaterial.refractorness) / (1 + refractorMaterial.refractorness), 2);
	}

	public Luminance BRDF(BodyHitPoint hitPoint, in Vector directionToLight) =>
		Luminance.Zero;

	public RandomDirection SampleDirection(BodyHitPoint hitPoint)
	{
		var r = r0 + (1 - r0) * Math.Pow(1 - Math.Abs(hitPoint.IncomingDirection.DotProduct(hitPoint.Normal)), 5);
		var ksi = rnd.NextDouble();
		if (ksi < r)
		{
			return mirrorMaterial.SampleDirection(hitPoint);
		}
		else
		{
			return refractorMaterial.SampleDirection(hitPoint);
		}
	}

	public bool IsPerfect => true;
}
