namespace Engine;

public readonly record struct Luminance(double r, double g, double b)
{
	public static Luminance Zero => new Luminance(0, 0, 0);
	public static Luminance Unit => new Luminance(1, 1, 1);

	public bool IsZero => r == 0 && g == 0 && b == 0;

	public double Energy => (r + g + b) / 3;

	public static Luminance operator +(in Luminance a, in Luminance b) =>
		new Luminance(a.r + b.r, a.g + b.g, a.b + b.b);
	
	public static Luminance operator -(in Luminance a, in Luminance b) =>
		new Luminance(a.r - b.r, a.g - b.g, a.b - b.b);

	public static Luminance operator *(in Luminance a, double factor) =>
		new Luminance(factor * a.r, factor * a.g, factor * a.b);

	public static Luminance operator *(double factor, in Luminance a) =>
		new Luminance(factor * a.r, factor * a.g, factor * a.b);

	public static Luminance operator *(in Luminance a, in Luminance b) =>
		new Luminance(a.r * b.r, a.g * b.g, a.b * b.b);

	public static Luminance operator /(in Luminance a, double factor) =>
		a * (1 / factor);

	public static Luminance Max(in Luminance a, in Luminance b) =>
		new Luminance(Math.Max(a.r, b.r), Math.Max(a.g, b.g), Math.Max(a.b, b.b));
}
