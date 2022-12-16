using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Materials;
using Lights;
using Shapes;

namespace Engine;

public class Program
{
	public IShape scene;
	public IShape diffuse;
	public IShape glossy;
	public ILightSource lights;
	public IEngine engine;

	const int W = 640;
	const int H = 640;
	const double CAM_Z = 0.0000001;
	const double CAM_SIZE = (0.55 * CAM_Z / (1 + CAM_Z));
	const double PIXEL_SIZE = 1.05;
	const int NFRAMES = 10;


	public void InitScene(Random rnd)
	{
		Luminance kd_black = Luminance.Zero;
		Luminance ks_black = new Luminance(0, 0, 0.2);
		int[]    n_black  = {1, 1, 1};
		IMaterial m_black = new DuffuseSpecularMaterial(rnd, kd_black, ks_black, n_black);

		Luminance kd_white = new Luminance(1, 1, 0.8);
		Luminance ks_white = new Luminance(0, 0, 0.2);
		int[]    n_white  = {1, 1, 1};
		IMaterial m_white = new DuffuseSpecularMaterial(rnd, kd_white, ks_white, n_white);

		ITexturedMaterial m_chess = new CheckeredMaterial(10, 10, m_white, m_black);
		
		Luminance kd_red = new Luminance(1, 0, 0);
		Luminance ks_red = Luminance.Zero;
		int[]    n_red  = {0, 0, 0};
		IMaterial m_red = new DuffuseSpecularMaterial(rnd, kd_red, ks_red, n_red);
		
		Luminance kd_blue = new Luminance(0, 0, 1);
		Luminance ks_blue = Luminance.Zero;
		int[]    n_blue  = {0, 0, 0};
		IMaterial m_blue = new DuffuseSpecularMaterial(rnd, kd_blue, ks_blue, n_blue);
		
		Luminance kd_green = new Luminance(0, 1, 0);
		Luminance ks_green = Luminance.Zero;
		int[]    n_green  = {0, 0, 0};
		IMaterial m_green = new DuffuseSpecularMaterial(rnd, kd_green, ks_green, n_green);

		Luminance kd_yellow = new Luminance(1, 1, 0);
		Luminance ks_yellow = Luminance.Zero;
		int[]    n_yellow  = {0, 0, 0};
		IMaterial m_yellow = new DuffuseSpecularMaterial(rnd, kd_yellow, ks_yellow, n_yellow);

		Luminance kd1 = new Luminance(0.9, 0.6, 0.3);
		Luminance ks1 = Luminance.Zero;
		int[]    n1  = {0, 0, 0};
		IMaterial m1 = new DuffuseSpecularMaterial(rnd, kd1, ks1, n1);

		Luminance kd2 = new Luminance(0.6, 0.1, 1);
		Luminance ks2 = Luminance.Zero;
		int[]    n2  = {0, 0, 0};
		IMaterial m2 = new DuffuseSpecularMaterial(rnd, kd2, ks2, n2);
		
		Luminance kd3 = Luminance.Zero;
		Luminance ks3 = new Luminance(1, 1, 1);
		int[]    n3  = {1, 1, 1};
		IMaterial m3 = new DuffuseSpecularMaterial(rnd, kd3, ks3, n3);
		
		Luminance rrefract = new Luminance(1, 1, 1);
		double refract = 1 / 2.0;
		IMaterial m_refractor = new IdealRefractorMaterial(rrefract, refract);
		
		ITexturedMaterial m_chess2 = new CheckeredMaterial(10, 1, m_red, m_green);

		Luminance Le1 = new Luminance(50, 50, 50);
		
		IShape floor     = new Square(new Vector(-0.5, -0.5, 1), new Vector(-0.5, -0.5, 2), new Vector( 0.5, -0.5, 1), m_chess);
		IShape ceiling   = new Square(new Vector(-0.5,  0.5, 1), new Vector( 0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), m_yellow);
		IShape backWall  = new Square(new Vector(-0.5, -0.5, 2), new Vector(-0.5,  0.5, 2), new Vector( 0.5, -0.5, 2), m_refractor);
		IShape leftWall  = new Square(new Vector(-0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), new Vector(-0.5, -0.5, 1), m_green);
		IShape rightWall = new Square(new Vector( 0.5,  0.5, 1), new Vector( 0.5, -0.5, 1), new Vector( 0.5,  0.5, 2), m_refractor);

		IShape ball1 = new Sphere(new Vector(   0, -0.4, 1.3), 0.1,  m1);
		IShape ball2 = new Sphere(new Vector(-0.23, 0, 1.3), 0.1, m2);
		IShape ball3 = new Sphere(new Vector(0.3, -0.3, 1.5), 0.15, m_refractor);

		IShape[] shapes = {
			floor,
			ceiling,
			backWall,
			leftWall,
			rightWall,

			//ball1,
			ball2,
			ball3,
		};
		
		IShape[] glossyShapes = {
			//floor,
			//ceiling,
			//backWall,
			//leftWall,
			//rightWall,

			//ball1,
			//ball2,
			ball3,
		};
		
		IShape[] diffuseShapes = {
			floor,
			ceiling,
			backWall,
			leftWall,
			rightWall,

			ball1,
			ball2,
			//ball3,
		};
		
		ILightSource[] lightSources = {
			new SquareLight(rnd, new Vector(-0.15, 0.5 - double.Epsilon, 1.35), new Vector(0.15,  0.5 - double.Epsilon, 1.35), new Vector(-0.15, 0.5 - double.Epsilon, 1.65), Le1),
			//new SphereLight(new Vector(-0.15, 0.45, 8.35), new Vector(0.15,  0.45, 8.35), new Vector(-0.15, 0.45, 8.65), Le1),
			new SphereLight(rnd, new Vector(0, 0.5, 1.5), 0.1, Le1),
			//new SphereLight(new Vector(-0.3, -0.3, 1.5), 0.05, Le1),
		};
		
		scene = new Scene(shapes);
		glossy = new Scene(glossyShapes);
		diffuse = new Scene(diffuseShapes);
		lights = new CompositeLightSource(rnd, lightSources);
	}

	public static void Main()
	{
		new Program().Run();
	}

	public void Run()
	{
		var rnd = new Random();
		InitScene(rnd);

		engine = new Engines.SimpleTracing(rnd);
		var rasterizer = new Rasterizer(rnd, PIXEL_SIZE, W, H, CAM_Z, CAM_SIZE, scene, diffuse, glossy, lights, engine);

		using(var image = new Image<Rgb24>(W, H))
		{
			image.ProcessPixelRows(accessor => {
				for(int y = 0; y < H; y++) {
					Console.WriteLine($"Row {y}");
					var pixelRow = accessor.GetRowSpan(y);
					for(int x = 0; x < W; x++)
					{
						var l = Luminance.Zero;
						for(int frame = 1; frame <= NFRAMES; frame++)
						{
							l += rasterizer.ColorAtPixel(x, y);
						}
						l *= (255.0 / NFRAMES);

						pixelRow[x] = new Rgb24(
							Math.Min((byte)l.r, (byte)255),
							Math.Min((byte)l.g, (byte)255),
							Math.Min((byte)l.b, (byte)255)
						);
					}
				}
			});

			image.SaveAsBmp("result.bmp");
		}
	}
}
