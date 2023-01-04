//#define HQ

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Materials;
using Lights;
using Shapes;
using Engines;
using Bodies;

namespace Engine;

public class Program
{
#if HQ
	const int W = 1280;
	const int H = 1280;
#else
	const int W = 320;
	const int H = 320;
#endif
	const double CAM_Z = 0.0000001;
	const double CAM_SIZE = (0.55 * CAM_Z / (1 + CAM_Z));
	const double PIXEL_SIZE = 1.05;
	const int NFRAMES = 1000;
	const int REFLECT_RAYS = 10;
	const int SHADOW_RAYS = 10;
	const double ABSOPTION = 0.9;

	public SceneSetup InitScene(Random rnd)
	{
		Luminance kd_black = Luminance.Zero;
		Luminance ks_black = Luminance.Zero;
		Luminance n_black = new Luminance(1, 1, 1);
		IMaterial m_black = DiffuseSpecularMaterial.Create(rnd, kd_black, ks_black, n_black);

		Luminance kd_white = new Luminance(1, 1, 1);
		Luminance ks_white = Luminance.Zero;
		Luminance n_white = new Luminance(1, 1, 1);
		IMaterial m_white = DiffuseSpecularMaterial.Create(rnd, kd_white, ks_white, n_white);

		ITexturedMaterial m_chess = new CheckeredMaterial(2, 2, m_white, m_black);
		
		Luminance kd_red = new Luminance(1, 0, 0);
		Luminance ks_red = Luminance.Zero;
		Luminance n_red = Luminance.Zero;
		IMaterial m_red = DiffuseSpecularMaterial.Create(rnd, kd_red, ks_red, n_red);
		
		Luminance kd_blue = new Luminance(0, 0, 1);
		Luminance ks_blue = Luminance.Zero;
		Luminance n_blue = Luminance.Zero;
		IMaterial m_blue = DiffuseSpecularMaterial.Create(rnd, kd_blue, ks_blue, n_blue);
		
		Luminance kd_green = new Luminance(0, 1, 0);
		Luminance ks_green = Luminance.Zero;
		Luminance n_green = Luminance.Zero;
		IMaterial m_green = DiffuseSpecularMaterial.Create(rnd, kd_green, ks_green, n_green);

		Luminance kd_yellow = new Luminance(1, 1, 0);
		Luminance ks_yellow = Luminance.Zero;
		Luminance n_yellow = Luminance.Zero;
		IMaterial m_yellow = DiffuseSpecularMaterial.Create(rnd, kd_yellow, ks_yellow, n_yellow);

		Luminance kd1 = new Luminance(0.9, 0.6, 0.3);
		Luminance ks1 = Luminance.Zero;
		Luminance n1 = Luminance.Zero;
		IMaterial m1 = DiffuseSpecularMaterial.Create(rnd, kd1, ks1, n1);

		Luminance kd2 = new Luminance(0.6, 0.1, 1);
		Luminance ks2 = Luminance.Zero;
		Luminance n2 = Luminance.Zero;
		IMaterial m2 = DiffuseSpecularMaterial.Create(rnd, kd2, ks2, n2);
		
		Luminance kd3 = Luminance.Zero;
		Luminance ks3 = new Luminance(1, 1, 1);
		Luminance n3 = new Luminance(6, 3, 1);
		IMaterial m3 = DiffuseSpecularMaterial.Create(rnd, kd3, ks3, n3);
		
		Luminance rrefract = new Luminance(1, 1, 1);
		IMaterial m_refractor = new IdealRefractorMaterial(1.5);
		
		ITexturedMaterial m_chess_wb = new CheckeredMaterial(4*2000, 4*200, m_white, m_black);

		Luminance Le1 = Luminance.Unit;
		
		IBody floor     = new Square(new Vector(-2000, -0.5, 1), new Vector(-2000, -0.5, 200), new Vector( 2000, -0.5, 1)).WithMaterial(m_chess_wb);
		//IShape floor     = new Square(new Vector(-0.5, -0.5, 1), new Vector(-0.5, -0.5, 2), new Vector( 0.5, -0.5, 1)).WithMaterial(m_chess_wb);
		IBody ceiling   = new Square(new Vector(-0.5,  0.5, 1), new Vector( 0.5,  0.5, 1), new Vector(-0.5,  0.5, 2)).WithMaterial(m_chess_wb);
		IBody backWall  = new Square(new Vector(-2000, -2000, 200), new Vector(-2000,  0.5, 200), new Vector(2000, -0.5, 200)).WithMaterial(m_yellow);
		IBody leftWall  = new Square(new Vector(-0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), new Vector(-0.5, -0.5, 1)).WithMaterial(m_green);
		IBody rightWall = new Square(new Vector( 0.5,  0.5, 1), new Vector( 0.5, -0.5, 1), new Vector( 0.5,  0.5, 2)).WithMaterial(m_blue);

		IBody ball1 = new Sphere(new Vector(-0.7 + 0.4, -0.5 + 0.4, 1.5), 0.4).WithMaterial(m_refractor);
		IBody ball2 = new Sphere(new Vector(0.2, 0, 3.2), 0.3).WithMaterial(m_refractor);
		IBody ball3 = new Sphere(new Vector(0.3, -0.3, 1.5), 0.15).WithMaterial(m_refractor);

		IBody[] shapes = {
			floor,
			//ceiling,
			//backWall,
			//leftWall,
			//rightWall,

			ball1,
			ball2,
			//ball3,
		};
		
		IBody[] glossyShapes = {
			//floor,
			//ceiling,
			//backWall,
			//leftWall,
			//rightWall,

			//ball1,
			//ball2,
			//ball3,
		};
		
		IBody[] diffuseShapes = {
			floor,
			//ceiling,
			//backWall,
			//leftWall,
			//rightWall,

			ball1,
			//ball2,
			//ball3,
		};
		
		ILightSource[] lightSources = {
			//new SquareLight(rnd, new Vector(-0.15, 0.5 - double.Epsilon, 1.35), new Vector(0.15,  0.5 - double.Epsilon, 1.35), new Vector(-0.15, 0.5 - double.Epsilon, 1.65), Le1),
			//new SphereLight(new Vector(-0.15, 0.45, 8.35), new Vector(0.15,  0.45, 8.35), new Vector(-0.15, 0.45, 8.65), Le1),
			new SphereLight(rnd, new Vector(   0.2, -0.4, 1.1), 0.1, Le1 * 4),
			new SphereLight(rnd, new Vector(-1, 1, 4), 0.2, Le1 * 20),
			new SphereLight(rnd, new Vector(2, 1, 6), 0.5, Le1 * 40),
			//new SphereLight(rnd, new Vector(-0.3, -0.3, 1.5), 0.05, Le1),
		};
		
		var scene = new CompositeBody(shapes);
		var glossy = new CompositeBody(glossyShapes);
		var diffuse = new CompositeBody(diffuseShapes);
		var lights = CompositeLightSource.Create(rnd, lightSources);

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

		var engineFactory = new SimpleTracingEngineFactory(REFLECT_RAYS, SHADOW_RAYS, ABSOPTION);

		var engine = engineFactory.CreateEngine(rnd, sceneSetup);
		var rasterizer = new Rasterizer(rnd, PIXEL_SIZE, W, H, CAM_Z, CAM_SIZE, engine);

		var L = new Luminance[W, H];
		Directory.CreateDirectory("result");
		for(int frame = 1; frame <= NFRAMES; frame++) {
			using(var image = new Image<Rgb24>(W, H))
			{
				image.ProcessPixelRows(accessor => {
					for(int y = 0; y < H; y++) {
						Console.WriteLine($"frame {frame} - row {y}");
						var pixelRow = accessor.GetRowSpan(y);
						for(int x = 0; x < W; x++)
						{
							L[x, y] = L[x, y] * (frame - 1) / frame + rasterizer.ColorAtPixel(x, y) / frame;
							var (r, g, b) = L[x, y] * 255;

							pixelRow[x] = new Rgb24(
								(byte)(Math.Min(r, 255)),
								(byte)(Math.Min(g, 255)),
								(byte)(Math.Min(b, 255))
							);
						}
					}

					image.SaveAsBmp($"result/{frame}.bmp");
				});
			}
		}
	}
}
