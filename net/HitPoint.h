#pragma once


namespace Engine
{
	class HitPoint
	{
		HitPoint(double t, Vector normal, IMaterial material) :
			t(t),
			normal(normal),
			material(material)
		{
		}

		double t;
		Vector normal;
		IMaterial material;
	};
}