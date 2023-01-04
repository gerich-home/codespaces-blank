using Engine;

namespace Materials;

public static class DiffuseSpecularMaterial
{
	public static IMaterial Create(Random rnd, Luminance rd, Luminance rs, Luminance n) =>
		CompositeMaterial.Create(
			rnd,
			new DiffuseMaterial(rnd, rd),
			SpecularMaterial.Create(rnd, rs, n),
			rd.Energy,
			rs.Energy
		);
}
