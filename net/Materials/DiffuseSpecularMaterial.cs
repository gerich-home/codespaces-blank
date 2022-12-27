using Engine;

namespace Materials;

public static class DiffuseSpecularMaterial
{
	public static IMaterial Create(Random rnd, Luminance rd, Luminance rs, Luminance n)
	{
		var qd = rd.Energy;
		var qs = rs.Energy;

		if (qs == 0)
		{
			if(qd == 0)
			{
				return new PureBlackMaterial();
			}
			else
			{
				return new DiffuseMaterial(rnd, rd);
			}
		}
		
		if(qd == 0)
		{
			// TODO split by n into separate materials
			return new SpecularMaterial(rnd, rs, n.Energy);
		}

		return new CompositeMaterial(
			new DiffuseMaterial(rnd, rd),
			// TODO split by n into separate materials
			new SpecularMaterial(rnd, rs, n.Energy),
			qd / (qd + qs)
		);
	}
}
