namespace Engine;

public static class RandomExtensions
{
	public static readonly double TwoPi = 2 * Math.PI;

    public static Vector NextSemisphereDirectionUniform(this Random rnd)
    {
        var cosa = rnd.NextDouble();
        var sina = Math.Sqrt(1 - cosa * cosa);
        var b = rnd.NextDouble(TwoPi);
        var (sinb, cosb) = Math.SinCos(b);

        return new Vector(
            cosa * cosb,
            cosa * sinb,
            sina
        );
    }

    public static Vector NextSemisphereDirectionCos(this Random rnd, double r2divd2)
    {
        var cosa = Math.Sqrt(rnd.NextDouble(r2divd2, 1));
        var sina = Math.Sqrt(1 - cosa * cosa);
        var b = rnd.NextDouble(TwoPi);
        var (sinb, cosb) = Math.SinCos(b);

        return new Vector(
            sina * cosb,
            sina * sinb,
            cosa
        );
    }

    public static Vector NextSemisphereDirectionCos(this Random rnd)
    {
        var cosa = Math.Sqrt(rnd.NextDouble());
        var sina = Math.Sqrt(1 - cosa * cosa);
        var b = rnd.NextDouble(TwoPi);
        var (sinb, cosb) = Math.SinCos(b);

        return new Vector(
            sina * cosb,
            sina * sinb,
            cosa
        );
    }

    public static double NextDouble(this Random rnd, double max) =>
        rnd.NextDouble() * max;

    public static double NextDouble(this Random rnd, double min, double max) =>
        rnd.NextDouble() * (max - min) + min;
}
