namespace Engine;

public record class Luminance(double r, double g, double b)
{
	public static Luminance operator +(Luminance a, Luminance b) =>
		new Luminance(a.r + b.r, a.g + b.g, a.b + b.b);
	
	public static Luminance operator -(Luminance a, Luminance b) =>
		new Luminance(a.r - b.r, a.g - b.g, a.b - b.b);

	public static Luminance operator *(Luminance a, double factor) =>
		new Luminance(factor * a.r, factor * a.g, factor * a.b);

	public static Luminance operator *(double factor, Luminance a) =>
		new Luminance(factor * a.r, factor * a.g, factor * a.b);

	public static Luminance operator *(Luminance a, Luminance b) =>
		new Luminance(a.r * b.r, a.g * b.g, a.b * b.b);

	public static Luminance operator /(Luminance a, double factor) =>
		a * (1 / factor);
}
