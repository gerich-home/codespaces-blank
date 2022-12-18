using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Materials;
using Lights;
using Shapes;
using Engines;

namespace Engine;

public class Program
{
	const int W = 640;
	const int H = 640;
	const double CAM_Z = 0.0000001;
	const double CAM_SIZE = (0.55 * CAM_Z / (1 + CAM_Z));
	const double PIXEL_SIZE = 1.05;
	const int NFRAMES = 10;

	public SceneSetup InitScene(Random rnd)
	{
		Luminance kd_black = Luminance.Zero;
		Luminance ks_black = Luminance.Zero;
		Luminance n_black = new Luminance(1, 1, 1);
		IMaterial m_black = new DuffuseSpecularMaterial(rnd, kd_black, ks_black, n_black);

		Luminance kd_white = new Luminance(1, 1, 1);
		Luminance ks_white = Luminance.Zero;
		Luminance n_white = new Luminance(1, 1, 1);
		IMaterial m_white = new DuffuseSpecularMaterial(rnd, kd_white, ks_white, n_white);

		ITexturedMaterial m_chess = new CheckeredMaterial(2, 2, m_white, m_black);
		
		Luminance kd_red = new Luminance(1, 0, 0);
		Luminance ks_red = Luminance.Zero;
		Luminance n_red = Luminance.Zero;
		IMaterial m_red = new DuffuseSpecularMaterial(rnd, kd_red, ks_red, n_red);
		
		Luminance kd_blue = new Luminance(0, 0, 1);
		Luminance ks_blue = Luminance.Zero;
		Luminance n_blue = Luminance.Zero;
		IMaterial m_blue = new DuffuseSpecularMaterial(rnd, kd_blue, ks_blue, n_blue);
		
		Luminance kd_green = new Luminance(0, 1, 0);
		Luminance ks_green = Luminance.Zero;
		Luminance n_green = Luminance.Zero;
		IMaterial m_green = new DuffuseSpecularMaterial(rnd, kd_green, ks_green, n_green);

		Luminance kd_yellow = new Luminance(1, 1, 0);
		Luminance ks_yellow = Luminance.Zero;
		Luminance n_yellow = Luminance.Zero;
		IMaterial m_yellow = new DuffuseSpecularMaterial(rnd, kd_yellow, ks_yellow, n_yellow);

		Luminance kd1 = new Luminance(0.9, 0.6, 0.3);
		Luminance ks1 = Luminance.Zero;
		Luminance n1 = Luminance.Zero;
		IMaterial m1 = new DuffuseSpecularMaterial(rnd, kd1, ks1, n1);

		Luminance kd2 = new Luminance(0.6, 0.1, 1);
		Luminance ks2 = Luminance.Zero;
		Luminance n2 = Luminance.Zero;
		IMaterial m2 = new DuffuseSpecularMaterial(rnd, kd2, ks2, n2);
		
		Luminance kd3 = Luminance.Zero;
		Luminance ks3 = new Luminance(1, 1, 1);
		Luminance n3 = new Luminance(1, 1, 1);
		IMaterial m3 = new DuffuseSpecularMaterial(rnd, kd3, ks3, n3);
		
		Luminance rrefract = new Luminance(1, 1, 1);
		double refract = 1 / 2.0;
		IMaterial m_refractor = new IdealRefractorMaterial(rrefract, refract);
		
		ITexturedMaterial m_chess2 = new CheckeredMaterial(5, 5, m_red, m_green);

		Luminance Le1 = new Luminance(150, 150, 150);
		
		IShape floor     = new Square(new Vector(-0.5, -0.5, 1), new Vector(-0.5, -0.5, 2), new Vector( 0.5, -0.5, 1), m_chess2);
		IShape ceiling   = new Square(new Vector(-0.5,  0.5, 1), new Vector( 0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), m_yellow);
		IShape backWall  = new Square(new Vector(-0.5, -0.5, 2), new Vector(-0.5,  0.5, 2), new Vector( 0.5, -0.5, 2), m_red);
		IShape leftWall  = new Square(new Vector(-0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), new Vector(-0.5, -0.5, 1), m_green);
		IShape rightWall = new Square(new Vector( 0.5,  0.5, 1), new Vector( 0.5, -0.5, 1), new Vector( 0.5,  0.5, 2), m_blue);

		IShape ball1 = new Sphere(new Vector(   0, -0.4, 1.3), 0.1,  m1);
		IShape ball2 = new Sphere(new Vector(-0.23, 0, 1.3), 0.1, m2);
		IShape ball3 = new Sphere(new Vector(0.3, -0.3, 1.5), 0.15, m_refractor);

		IShape[] shapes = {
			floor,
			ceiling,
			backWall,
			leftWall,
			rightWall,

			ball1,
			//ball2,
			//ball3,
		};
		
		IShape[] glossyShapes = {
			//floor,
			//ceiling,
			//backWall,
			//leftWall,
			//rightWall,

			//ball1,
			//ball2,
			//ball3,
		};
		
		IShape[] diffuseShapes = {
			floor,
			ceiling,
			backWall,
			leftWall,
			rightWall,

			ball1,
			//ball2,
			//ball3,
		};
		
		ILightSource[] lightSources = {
			new SquareLight(rnd, new Vector(-0.15, 0.5 - double.Epsilon, 1.35), new Vector(0.15,  0.5 - double.Epsilon, 1.35), new Vector(-0.15, 0.5 - double.Epsilon, 1.65), Le1),
			//new SphereLight(new Vector(-0.15, 0.45, 8.35), new Vector(0.15,  0.45, 8.35), new Vector(-0.15, 0.45, 8.65), Le1),
			//new SphereLight(rnd, new Vector(0, 0.5, 1.5), 0.1, Le1),
			//new SphereLight(new Vector(-0.3, -0.3, 1.5), 0.05, Le1),
		};
		
		var scene = new Scene(shapes);
		var glossy = new Scene(glossyShapes);
		var diffuse = new Scene(diffuseShapes);
		var lights = new CompositeLightSource(rnd, lightSources);

		return new SceneSetup(scene, diffuse, glossy, lights);
	}

	public static void Main()
	{
		new Program().Run();
	}

	public void Run()
	{
		var rnd = new Random();
		var sceneSetup = InitScene(rnd);

		var engineFactory = new SimpleTracingEngineFactory();

		var engine = engineFactory.CreateEngine(rnd, sceneSetup);
		var rasterizer = new Rasterizer(rnd, PIXEL_SIZE, W, H, CAM_Z, CAM_SIZE, engine, sceneSetup);

		using(var image = new Image<Rgb24>(W, H))
		{
			image.ProcessPixelRows(accessor => {
				for(int y = 0; y < H; y++) {
					Console.WriteLine($"Row {y}");
					var pixelRow = accessor.GetRowSpan(y);
					for(int x = 0; x < W; x++)
					{
						var l = (255.0 / NFRAMES) *
							Enumerable.Range(0, NFRAMES)
								.Select(i => rasterizer.ColorAtPixel(x, y))
								.Aggregate(Luminance.Zero, (l1, l2) => l1 + l2);

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
