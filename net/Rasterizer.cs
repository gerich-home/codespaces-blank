namespace Engine;

public record class Rasterizer(
	Random rnd,
	double pixelSize,
	int width,
	int height,
	double cam_z,
	double cam_size,
	IEngine engine,
	SceneSetup sceneSetup
)
{
	public Luminance ColorAtPixel(double x, double y)
	{
		double px = x + pixelSize * (rnd.NextDouble() - 0.5);
		double py = y + pixelSize * (rnd.NextDouble() - 0.5);

		double lx = cam_size * ((double) 2 * px / width - 1);
		double ly = cam_size * ((double) (height - 2 * py) / width);
		Vector start = new Vector(lx, ly, 0);
		Vector direction = new Vector(lx, ly, cam_z).Normalized;

		return engine.L(start.RayAlong(direction));
	}
}
