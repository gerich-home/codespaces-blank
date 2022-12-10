#pragma once


namespace Shapes
{
	using namespace Engine;

	class Scene : public IShape
	{
		Scene(int nshapes, IShape shapes[]);
		~Scene();
	
		HitPoint Intersection(Vector start, Vector direction) const;

	private:
		IShape* shapes;
		int nshapes;
	};
}