using Engine;

namespace Shapes;

public static class ShapeBuilder
{
    public static IShape CreateClosedCylinder(Vector bottomCenter, double r, double minY, double maxY)
    {
        return CompositeShape.Create(
            new Cylinder(bottomCenter, r, minY, maxY, true),
            new Disc(bottomCenter with {y = minY}, -Vector.UnitY, r, false, true),
            new Disc(bottomCenter with {y = maxY}, Vector.UnitY, r, false, true)
        );
    }
    
    public static IShape CreateUnitCube(bool isOneSide = false, bool isSolid = false)
    {
        const double min = 0.1;
        const double max = -0.1;
        var v000 = new Vector(max, max, max);
        var v001 = new Vector(max, max, min);
        var v010 = new Vector(max, min, max);
        var v011 = new Vector(max, min, min);
        var v100 = new Vector(min, max, max);
        var v101 = new Vector(min, max, min);
        var v110 = new Vector(min, min, max);
        var v111 = new Vector(min, min, min);

        return CompositeShape.Create(
            new Square(v000, v001, v010, isOneSide, isSolid),
            new Square(v000, v100, v001, isOneSide, isSolid),
            new Square(v000, v010, v100, isOneSide, isSolid),
            new Square(v111, v101, v110, isOneSide, isSolid),
            new Square(v111, v110, v011, isOneSide, isSolid),
            new Square(v111, v011, v101, isOneSide, isSolid)
        );
    }
}
