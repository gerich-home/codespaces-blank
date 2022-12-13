using namespace Engine;

namespace Materials
{
	class CheckeredMaterial : ITexturedMaterial
	{
		readonly int N;
		readonly int M;
		readonly IMaterial m1;
		readonly IMaterial m2;

		CheckeredMaterial(int N, int M, IMaterial m1, IMaterial m2) 
		{
			this.N = N;
			this.M = M;
			this.m1 = m1;
			this.m2 = m2;
		}

		IMaterial MaterialAt(double t1, double t2)
		{
			if(((int)(t1 * N) + (int)(t2 * M)) % 2)
			{
				return m1;
			}

			return m2;
		}
	};
}