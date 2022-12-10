#pragma once


namespace Engine
{
		
	
	class PhotonMap
	{
		PhotonMap(int nphotons);
		~PhotonMap();

		void Build();
		void Clear();
		bool Add(Photon photon);
		double FindNearest(Vector x, int count, Photon** result) const;

	private:
		void QSort(int left, int right, short axis);
		PhotonMapNode* CreateSubTree(int left, int right);
		void GoDown(PhotonMapNode* node, ParamsForFind& paramsForFind) const;
		PhotonMapNode* root;
		int nphotons;
		Photon* photons;
		int current;
		int* indexes[3];
	};
}