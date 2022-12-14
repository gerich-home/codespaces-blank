namespace Engine
{
	public class HitPoint
	{
		public HitPoint(double t, Vector normal, IMaterial material)
		{
			this.t = t;
			this.normal = normal;
			this.material = material;
		}

		public readonly double t;
		public readonly Vector normal;
		public readonly IMaterial material;
	};
}