using Engine;

namespace Materials;

public record class CheckeredMaterial(
	int N,
	int M,
	IMaterial m1,
	IMaterial m2
) : ITexturedMaterial {
	public IMaterial MaterialAt(double t1, double t2) =>
		((int)(t1 * N) + (int)(t2 * M)) % 2 == 0 ? m1 : m2;
}
