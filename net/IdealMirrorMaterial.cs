using Engine;

namespace Materials
{
	public class IdealMirrorMaterial: IMaterial
	{
		public readonly Luminance[] rs; //koefficient specular reflection

		public IdealMirrorMaterial(double[] rs)
		{
			this.rs = rs;
		}

		public Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
		{
			return new Luminance();
		}

		public RandomDirection SampleDirection(Random rnd, Vector direction, Vector normal, double ksi)
		{
			Vector R = direction - 2 * normal.DotProduct(direction) * normal;

			return new RandomDirection(rs, R);
		}
	}
}