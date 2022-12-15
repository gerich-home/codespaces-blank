using Engine;

namespace Shapes
{
	public class Square: IShape
	{
		public readonly Vector a;
		public readonly Vector ba;
		public readonly Vector ca;
		public readonly Vector normal;
		public readonly Vector n;
		public ITexturedMaterial material;

		public Square(Vector a, Vector b, Vector c, ITexturedMaterial material)
		{
			this.a = a;
			this.ba = b - a;
			this.ca = c - a;
			this.normal = (b - a).CrossProduct(c - a);
			this.n = (b - a).CrossProduct(c - a).Normalized;
			this.material = material;
		}
			
		public Square(Vector a, Vector b, Vector c, IMaterial material)
		{
			this.a = a;
			this.ba = b - a;
			this.ca = c - a;
			this.normal = (b - a).CrossProduct(c - a);
			this.n = (b - a).CrossProduct(c - a).Normalized;
			this.material = new Materials.TexturedMaterialAdapter(material);
		}

		public HitPoint Intersection(Vector start, Vector direction)
		{
			double t = 0;
			double t1 = 0;
			double t2 = 0;
			

			double divident = -direction.DotProduct(normal);
			
			if(divident == 0)
			{
				return null;
			}

			double factor = 1 / divident;

			Vector sa = start - a;
			Vector saxdir = sa.CrossProduct(direction);
			t1 = -ba.DotProduct(saxdir) * factor;
			
			if((t1 < 0) || (1 < t1))
			{
				return null;
			}
						
			t2 = ca.DotProduct(saxdir) * factor;
			
			if((t2 < 0) || (1 < t2))
			{
				return null;
			}
			
			t = sa.DotProduct(normal) * factor;

			if(t < 100 * double.Epsilon)
			{
				return null;
			}

			return new HitPoint(t, n, material.MaterialAt(t1, t2));
		}
	}
}
