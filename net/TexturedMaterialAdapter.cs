#pragma once


using namespace Engine;

namespace Materials
{
	class TexturedMaterialAdapter: public ITexturedMaterial
	{
		TexturedMaterialAdapter(IMaterial m) :
			m(m)
		{
		}

		IMaterial MaterialAt(double t1, double t2) const
		{
			return m;
		}

	private:
		IMaterial m;
	};
}