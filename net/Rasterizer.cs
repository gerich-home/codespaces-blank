namespace Engine;

public static class Rasterizer
{ 	
	public static Luminance ColorAtPixel(Random rnd, double px, double py, int width, int height, double cam_z, double cam_size, IShape scene, IShape diffuse, IShape glossy, ILightSource lights, IEngine engine)
	{
		double lx = cam_size * ((double) 2 * px / width - 1);
		double ly = cam_size * ((double) (height - 2 * py) / width);
		Vector start = new Vector(lx, ly, 0);
		Vector direction = new Vector(lx, ly, cam_z).Normalized;
		HitPoint hp = scene.Intersection(start, direction);
	
		if(hp == null)
		{
			return Luminance.Zero;
		}

		Vector point = start + direction * hp.t;

		Luminance l = engine.L(rnd, hp, point, direction, scene, diffuse, glossy, lights);

		return l;
	}
}
