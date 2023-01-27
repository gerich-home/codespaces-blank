using Engine;

namespace Shapes;

public static class ShapeExtensions
{
    public static IShape WithTransform(this IShape shape, Matrix modelToWorldMatrix) =>
        shape switch
        {
            TransformedShape transformed => transformed.shape.WithTransform(modelToWorldMatrix * transformed.modelToWorldMatrix),
            CompositeShape composite => CompositeShape.Create(composite.shapes.Select(shape => shape.WithTransform(modelToWorldMatrix)).ToArray()),
            _ => new TransformedShape(shape, modelToWorldMatrix)
        };
}
