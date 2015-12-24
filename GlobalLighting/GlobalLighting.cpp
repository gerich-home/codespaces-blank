// GlobalLighting.cpp: ���������� ����� ����� ��� ����������.
//

#include "stdafx.h"
#include "GlobalLighting.h"
#include "Sphere.h"
#include "Plane.h"
#include "Triangle.h"
#include "SimpleTracing.h"
#include "Rasterizer.h"
#include "Scene.h"
#include <time.h>

//#include "MemoryManager.h"

#define MAX_LOADSTRING 100

// ���������� ����������:
HINSTANCE hInst;								// ������� ���������
TCHAR szTitle[MAX_LOADSTRING];					// ����� ������ ���������
TCHAR szWindowClass[MAX_LOADSTRING];			// ��� ������ �������� ����

// ��������� ���������� �������, ���������� � ���� ������ ����:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);

IShape* scene;
IEngine* engine;

#define W 640
#define H 480
#define NRAYS 50

int APIENTRY _tWinMain(HINSTANCE hInstance,
	HINSTANCE hPrevInstance,
	LPTSTR    lpCmdLine,
	int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	// TODO: ���������� ��� �����.
	MSG msg;
	HACCEL hAccelTable;

	// ������������� ���������� �����
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_GLOBALLIGHTING, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// ��������� ������������� ����������:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_GLOBALLIGHTING));

	// ���� ��������� ���������:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  �������: MyRegisterClass()
//
//  ����������: ������������ ����� ����.
//
//  �����������:
//
//    ��� ������� � �� ������������� ���������� ������ � ������, ���� �����, ����� ������ ���
//    ��� ��������� � ��������� Win32, �� �������� ������� RegisterClassEx'
//    ������� ���� ��������� � Windows 95. ����� ���� ������� ����� ��� ����,
//    ����� ���������� �������� "������������" ������ ������ � ���������� �����
//    � ����.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_GLOBALLIGHTING));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_GLOBALLIGHTING);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   �������: InitInstance(HINSTANCE, int)
//
//   ����������: ��������� ��������� ���������� � ������� ������� ����.
//
//   �����������:
//
//        � ������ ������� ���������� ���������� ����������� � ���������� ����������, � �����
//        ��������� � ��������� �� ����� ������� ���� ���������.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND hWnd;

	hInst = hInstance; // ��������� ���������� ���������� � ���������� ����������

	hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{
		return FALSE;
	}


	GO_FLOAT kd1[] = {0.5, 0.5, 0.5};
	GO_FLOAT ks1[] = {0.5, 0.5, 0.5};
	GO_FLOAT Le1[] = {0, 0, 0};
	int      n1[]  = {100, 100, 100};

	GO_FLOAT kd2[] = {0, 0, 0};
	GO_FLOAT ks2[] = {0, 0, 0};
	GO_FLOAT Le2[] = {100, 100, 100};
	int      n2[]  = {0, 0, 0};
	
	GO_FLOAT kd3[] = {1, 1, 1};
	GO_FLOAT ks3[] = {0, 0, 0};
	GO_FLOAT Le3[] = {0, 0, 0};
	int      n3[]  = {0, 0, 0};

	const IShape* shapes[] = {
		//new Triangle(Vector(0, 0, 2), Vector(0, 1, 1), Vector(1, 0, 1), new Material(kd3, ks3, Le3, n3)),
		new Sphere(Vector(0, 0, 3), 1, new Material(kd1, ks1, Le1, n1)),
		new Sphere(Vector(2, 0, 5), 1, new Material(kd1, ks1, Le1, n1)),
		new Sphere(Vector(-2, 0, 7), 1, new Material(kd1, ks1, Le1, n1)),
		new Sphere(Vector(0, 0, -5), 1, new Material(kd3, ks3, Le3, n3)),
		new Sphere(Vector(3, 5, -10), 1, new Material(kd2, ks2, Le2, n2)),
		//new Sphere(Vector(-6, 0, -12), 1, new Material(kd2, ks2, Le2, n2))
	};

	scene = new Scene(sizeof(shapes) / sizeof(IShape*), shapes);
	engine = new SimpleTracing();


	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	return TRUE;
}

//
//  �������: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  ����������:  ������������ ��������� � ������� ����.
//
//  WM_COMMAND	- ��������� ���� ����������
//  WM_PAINT	-��������� ������� ����
//  WM_DESTROY	 - ������ ��������� � ������ � ���������.
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// ��������� ����� � ����:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);

		srand(time(0));
		//PrintMemoryTable();
		for(int j = 0; j < H; j++)
		{
			for(int i = 0; i < W; i++)
			{

				Luminance l = Luminance(0, 0, 0);

				for(int k = 0; k < NRAYS; k++)
				{
					l += ColorAtPixel(i + (float)rand() / RAND_MAX - 0.5, j + (float)rand() / RAND_MAX - 0.5, W, H, 3, scene, engine);
				}

				l /= NRAYS;

				SetPixel(hdc, i, j, RGB(l.r > 1 ? 255 : l.r * 255,
										l.g > 1 ? 255 : l.g * 255,
										l.b > 1 ? 255 : l.b * 255
										));
			}
			/*if(i % 60)
			{
			PrintMemoryTable();
			}*/
		}

		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// ���������� ��������� ��� ���� "� ���������".
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
