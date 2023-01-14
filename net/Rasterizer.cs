namespace Engine;

public record class Rasterizer(
	Random rnd,
	double pixelSize,   // 1 - no antialising, >1 - antialising
	int width,          // width of output image in pixels
	int height,         // height of output image in pixels
	double zoom,        // > 0. View rays are thrown from (0, 0, -zoom) through pixel (lx, ly, 0) into scene
	double camSize,     // camSize = 1 means image plane will have width*height size in z=0 plane
	double focalLength, // >0. Objects with z = focalLength look perfectly focused, other are blurry
	double aperture,    // >=0. 0 - every object looks focused. >0 objects with z far from focalLength look blurry
	IEngine engine
)
{
	public Luminance ColorAtPixel(int x, int y)
	{
		var px = x + pixelSize * (rnd.NextDouble() - 0.5);
		var py = y + pixelSize * (rnd.NextDouble() - 0.5);

		var lx = camSize * ((double) 2 * px / width - 1);
		var ly = camSize * ((double) (height - 2 * py) / width);

		var cameraPlanePixelPosition = new Vector(lx, ly, 0);

		// https://medium.com/@elope139/depth-of-field-in-path-tracing-e61180417027
		var focalPoint = (1 + focalLength / zoom) * cameraPlanePixelPosition + focalLength * Vector.UnitZ;

		var olx = lx + aperture * (rnd.NextDouble() - 0.5);
		var oly = ly + aperture * (rnd.NextDouble() - 0.5);

		var start = new Vector(olx, oly, 0);

		var direction = (focalPoint - start).Normalized;

		return engine.L(start.RayAlong(direction));
	}
}
