using System;

namespace Engine
{
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

		public Luminance[] L = new Luminance[W * H];


		public void InitScene()
		{
			Luminance kd_black = new Luminance(0, 0, 0);
			Luminance ks_black = new Luminance(0, 0, 0.2);
			int[]    n_black  = {1, 1, 1};
			IMaterial m_black = new Materials.DuffuseSpecularMaterial(kd_black, ks_black, n_black);

			Luminance kd_white = new Luminance(1, 1, 0.8);
			Luminance ks_white = new Luminance(0, 0, 0.2);
			int[]    n_white  = {1, 1, 1};
			IMaterial m_white = new Materials.DuffuseSpecularMaterial(kd_white, ks_white, n_white);

			ITexturedMaterial m_chess = new Materials.CheckeredMaterial(10, 10, m_white, m_black);
			
			Luminance kd_red = new Luminance(1, 0, 0);
			Luminance ks_red = new Luminance(0, 0, 0);
			int[]    n_red  = {0, 0, 0};
			IMaterial m_red = new Materials.DuffuseSpecularMaterial(kd_red, ks_red, n_red);
			
			Luminance kd_blue = new Luminance(0, 0, 1);
			Luminance ks_blue = new Luminance(0, 0, 0);
			int[]    n_blue  = {0, 0, 0};
			IMaterial m_blue = new Materials.DuffuseSpecularMaterial(kd_blue, ks_blue, n_blue);
			
			Luminance kd_green = new Luminance(0, 1, 0);
			Luminance ks_green = new Luminance(0, 0, 0);
			int[]    n_green  = {0, 0, 0};
			IMaterial m_green = new Materials.DuffuseSpecularMaterial(kd_green, ks_green, n_green);

			Luminance kd_yellow = new Luminance(1, 1, 0);
			Luminance ks_yellow = new Luminance(0, 0, 0);
			int[]    n_yellow  = {0, 0, 0};
			IMaterial m_yellow = new Materials.DuffuseSpecularMaterial(kd_yellow, ks_yellow, n_yellow);

			Luminance kd1 = new Luminance(0.9, 0.6, 0.3);
			Luminance ks1 = new Luminance(0, 0, 0);
			int[]    n1  = {0, 0, 0};
			IMaterial m1 = new Materials.DuffuseSpecularMaterial(kd1, ks1, n1);

			Luminance kd2 = new Luminance(0.6, 0.1, 1);
			Luminance ks2 = new Luminance(0, 0, 0);
			int[]    n2  = {0, 0, 0};
			IMaterial m2 = new Materials.DuffuseSpecularMaterial(kd2, ks2, n2);
			
			Luminance kd3 = new Luminance(0, 0, 0);
			Luminance ks3 = new Luminance(1, 1, 1);
			int[]    n3  = {1, 1, 1};
			IMaterial m3 = new Materials.DuffuseSpecularMaterial(kd3, ks3, n3);
			
			Luminance rrefract = new Luminance(1, 1, 1);
			double refract = 1 / 2.0;
			IMaterial m_refractor = new Materials.IdealRefractorMaterial(rrefract, refract);
			
			ITexturedMaterial m_chess2 = new Materials.CheckeredMaterial(10, 1, m_red, m_green);

			Luminance Le1 = new Luminance(25, 25, 25);
			
			IShape floor     = new Shapes.Square(new Vector(-0.5, -0.5, 1), new Vector(-0.5, -0.5, 2), new Vector( 0.5, -0.5, 1), m_chess);
			IShape ceiling   = new Shapes.Square(new Vector(-0.5,  0.5, 1), new Vector( 0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), m_yellow);
			IShape backWall  = new Shapes.Square(new Vector(-0.5, -0.5, 2), new Vector(-0.5,  0.5, 2), new Vector( 0.5, -0.5, 2), m_refractor);
			IShape leftWall  = new Shapes.Square(new Vector(-0.5,  0.5, 1), new Vector(-0.5,  0.5, 2), new Vector(-0.5, -0.5, 1), m_green);
			IShape rightWall = new Shapes.Square(new Vector( 0.5,  0.5, 1), new Vector( 0.5, -0.5, 1), new Vector( 0.5,  0.5, 2), m_refractor);

			IShape ball1 = new Shapes.Sphere(new Vector(   0, -0.4, 1.3), 0.1,  m1);
			IShape ball2 = new Shapes.Sphere(new Vector(-0.23, 0, 1.3), 0.1, m2);
			IShape ball3 = new Shapes.Sphere(new Vector(0.3, -0.3, 1.5), 0.15, m_refractor);

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
				new Lights.Square(new Vector(-0.15, 0.5 - double.Epsilon, 1.35), new Vector(0.15,  0.5 - double.Epsilon, 1.35), new Vector(-0.15, 0.5 - double.Epsilon, 1.65), Le1),
				//new Lights.Square(new Vector(-0.15, 0.45, 8.35), new Vector(0.15,  0.45, 8.35), new Vector(-0.15, 0.45, 8.65), new Luminance(Le1)),
				//new Lights.Sphere(new Vector(0, 0.5, 1.5), 0.1, new Luminance(Le1)),
				//new Lights.Sphere(new Vector(-0.3, -0.3, 1.5), 0.05, new Luminance(Le1)),
			};
			
			scene = new Shapes.Scene(shapes);
			glossy = new Shapes.Scene(glossyShapes);
			diffuse = new Shapes.Scene(diffuseShapes);
			lights = new Lights.CompositeLightSource(lightSources);
		}

		public static void Main()
		{
			new Program().Run();
		}

		public void Run()
		{
			InitScene();

			engine = new Engines.SimpleTracing();
			var rnd = new Random();

			for(int j = 0; j < H; j++)
			{
				for(int i = 0; i < W; i++)
				{
					L[i * H + j] = new Luminance(0, 0, 0);
				}
			}

			for(int frame = 1; frame < 100; frame++)
			for(int j = 0; j < H; j++)
			{
				for(int i = 0; i < W; i++)
				{
					L[i * H + j] += Rasterizer.ColorAtPixel(rnd, i + PIXEL_SIZE * rnd.NextDouble() - PIXEL_SIZE * 0.5, j + PIXEL_SIZE * rnd.NextDouble() - PIXEL_SIZE * 0.5, W, H, CAM_Z, CAM_SIZE, scene, diffuse, glossy, lights, engine);
					Luminance l = L[i * H + j] * (255.0 / frame);
					Console.WriteLine($"{i},{j} -> {(l.r > 255 ? 255 : (int)l.r)} {(l.g > 255 ? 255 : (int)l.g)} {(l.b > 255 ? 255 : (int)l.b)}");
				}
			}
		}
	}
}
