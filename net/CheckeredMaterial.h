#pragma once


using namespace Engine;

namespace Materials
{
	class CheckeredMaterial: public ITexturedMaterial
	{
		CheckeredMaterial(int N, int M, IMaterial m1, IMaterial m2) :
			N(N),
			M(M),
			m1(m1),
			m2(m2)
		{
		}

		IMaterial MaterialAt(double t1, double t2) const
		{
			if(((int)(t1 * N) + (int)(t2 * M)) % 2)
			{
				return m1;
			}

			return m2;
		}

	private:
		const int N;
		const int M;
		IMaterial m1;
		IMaterial m2;
	};
}