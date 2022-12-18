namespace Engine;

public static class RandomExtensions
{
    public static (double cosa, double sina) NextCosDistribution(this Random rnd)
    {
        var cosa = rnd.NextDouble();
        return (cosa, Math.Sqrt(1 - cosa * cosa));
    }
    public static (double cosa, double sina) NextSinDistribution(this Random rnd)
    {
        var sina = rnd.NextDouble();
        return (Math.Sqrt(1 - sina * sina), sina);
    }

    public static double NextDouble(this Random rnd, double max) =>
        rnd.NextDouble() * max;
}
