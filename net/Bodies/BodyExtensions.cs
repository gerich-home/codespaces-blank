using Engine;
using Materials;

namespace Bodies;

public static class BodyExtensions
{
    public static SimpleBody WithMaterial(this IShape shape, IMaterial material) =>
        new SimpleBody(shape, material);

    public static TexturedBody WithMaterial(this ITexturableShape shape, ITexturedMaterial material) =>
        new TexturedBody(shape, material);

    public static TexturedBody WithMaterial(this ITexturableShape shape, IMaterial material) =>
        new TexturedBody(shape, new TexturedMaterialAdapter(material));

}
