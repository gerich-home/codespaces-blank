using Engine;

namespace Materials;

public record class GradientMaterial(
	Luminance leftBottomColor,
	Luminance leftTopColor,
	Luminance rightBottomColor,
	Luminance rightTopColor,
	Func<Luminance, IMaterial> materialFactory
) : ITexturedMaterial {
	public IMaterial MaterialAt(double t1, double t2) =>
		materialFactory(
			(1-t2) * ((1-t1)*leftBottomColor + t1 * leftTopColor) +
			t2 * ((1-t1)*rightBottomColor + t1 * rightTopColor)
		);
}
