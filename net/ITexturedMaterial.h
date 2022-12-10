#pragma once

namespace Engine
{
	
	interface ITexturedMaterial
	{
		IMaterial MaterialAt(double t1, double t2);
	};
}