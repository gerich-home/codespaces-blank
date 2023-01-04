using Engine;
using Materials;

namespace Bodies;

public class TexturedBody: IBody
{
	public readonly ITexturedMaterial material;
	public readonly ITexturableShape shape;

	public TexturedBody(ITexturableShape shape, ITexturedMaterial material)
	{
		this.shape = shape;
		this.material = material;
	}
		
	public TexturedBody(ITexturableShape shape, IMaterial material)
	{
		this.shape = shape;
		this.material = new TexturedMaterialAdapter(material);
	}

	public ref readonly AABB AABB => ref shape.AABB;

	public BodyHitPoint Intersection(in Ray ray)
	{
		var shapeHitPoint = shape.TexturableIntersection(ray);

		if(shapeHitPoint == null) 
		{
			return null;
		}

		return new BodyHitPoint(shapeHitPoint, material.MaterialAt(shapeHitPoint.T1, shapeHitPoint.T2));
	}
}
