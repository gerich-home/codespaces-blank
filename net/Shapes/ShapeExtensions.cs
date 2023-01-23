using Engine;

namespace Shapes;

public static class ShapeExtensions
{
    public static TransformedShape WithTransform(this IShape shape, Matrix modelToWorldMatrix) =>
        new TransformedShape(shape, modelToWorldMatrix);

    public static TransformedShape WithTransform(this TransformedShape shape, Matrix modelToWorldMatrix) =>
        new TransformedShape(shape.shape, modelToWorldMatrix * shape.modelToWorldMatrix);
}
