using Engine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Materials;

public class PictureMaterial: ITexturedMaterial
{
    public readonly string imagePath;
    public readonly Func<Luminance, IMaterial> materialFactory;
    public readonly Image<Rgba32> image;
    public readonly int size;
    
    public PictureMaterial(string imagePath, Func<Luminance, IMaterial> materialFactory)
    {
        this.imagePath = imagePath;
        this.materialFactory = materialFactory;
        this.image = Image.Load<Rgba32>(imagePath);
        this.size = Math.Min(image.Width, image.Height) - 1;
    }

	public IMaterial MaterialAt(double t1, double t2)
    {
        var pixel = image[(int)(t1 * size), (int)((1-t2) * size)];
        
        const double f = 1.0 / 255;

		return materialFactory(new Luminance(pixel.R * f, pixel.G * f, pixel.B * f));
    }
}
