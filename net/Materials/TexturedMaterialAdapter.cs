using Engine;

namespace Materials;

public record class TexturedMaterialAdapter(IMaterial m) : ITexturedMaterial
{
	public IMaterial MaterialAt(double t1, double t2) => m;
}
