#pragma once


namespace Engine
{
						
	Luminance ColorAtPixel(double px, double py, int width, int height, double cam_z, double cam_size, IShape scene, IShape diffuse, IShape glossy, ILightSource lights, IEngine engine)
	{
		double lx = cam_size * ((double) 2 * px / width - 1);
		double ly = cam_size * ((double) (height - 2 * py) / width);
		Vector start(lx, ly, 0);
		Vector direction(new Vector(lx, ly, cam_z).Normalized);
		HitPoint hp = scene.Intersection(start, direction);
	
		if(!hp)
		{
			return new Luminance(0, 0, 0);
		}

		Vector point(start + direction * hp.t);

		Luminance l = engine.L(*hp, point, direction, scene, diffuse, glossy, lights);
	
		delete hp;

		return l;
	}
}