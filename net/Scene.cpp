
using namespace Engine;

Shapes.Scene.Scene(int nshapes, IShape shapes[]) :
	nshapes(nshapes)
{
	this.shapes = new IShape[nshapes];

	for(int i = 0; i < nshapes; i++)
	{
		this.shapes[i] = shapes[i];
	}
}

Shapes.Scene.~Scene()
{
	delete[] shapes;
}

HitPoint Shapes.Scene.Intersection(Vector start, Vector direction)
{
	HitPoint bestHitPoint = NULL;
	HitPoint hitPoint;

	for(int i = 0; i < nshapes; i++)
	{
		hitPoint = shapes[i].Intersection(start, direction);
		if(hitPoint != NULL && (bestHitPoint ? hitPoint.t < bestHitPoint.t : TRUE))
		{
			delete bestHitPoint;
			bestHitPoint = hitPoint;
		}
		else
		{
			delete hitPoint;
		}
	}

	return bestHitPoint;
}
