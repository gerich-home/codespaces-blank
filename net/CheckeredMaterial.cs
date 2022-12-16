using Engine;

namespace Materials;

public class CheckeredMaterial : ITexturedMaterial
{
	public readonly int N;
	public readonly int M;
	public readonly IMaterial m1;
	public readonly IMaterial m2;

	public CheckeredMaterial(int N, int M, IMaterial m1, IMaterial m2) 
	{
		this.N = N;
		this.M = M;
		this.m1 = m1;
		this.m2 = m2;
	}

	public IMaterial MaterialAt(double t1, double t2)
	{
		if(((int)(t1 * N) + (int)(t2 * M)) % 2 == 1)
		{
			return m1;
		}

		return m2;
	}
}
