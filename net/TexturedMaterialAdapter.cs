#pragma once


using namespace Engine;

namespace Materials
{
	public class TexturedMaterialAdapter : ITexturedMaterial
	{
		public readonly IMaterial m;

		public TexturedMaterialAdapter(IMaterial m)
		{
			this.m = m;
		}

		public IMaterial MaterialAt(double t1, double t2)
		{
			return m;
		}
	};
}