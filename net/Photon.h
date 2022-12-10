#pragma once


namespace Engine
{
	class Photon
	{
		Photon()
		{
		}

		Photon(Vector point, Vector normal, Vector direction, Luminance energy) :
			point(point),
			normal(normal),
			direction(direction),
			energy(energy)
		{
		}
			
		Vector point;
		Vector normal;
		Vector direction;
		Luminance energy;
	};
}