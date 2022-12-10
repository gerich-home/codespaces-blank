// GlobalLighting.cpp: ���������� ����� ����� ��� ����������.
//







using namespace Engine;

IShape* scene;
IShape* diffuse;
IShape* glossy;
ILightSource* lights;
IEngine* engine;

#define W 640
#define H 640
#define CAM_Z 0.0000001
#define CAM_SIZE (0.55 * CAM_Z / (1 + CAM_Z))
#define PIXEL_SIZE 1.05
#define WORKERS 8
//#define SEED 0
#define SEED time(0)

Luminance L[W * H];

DWORD ThreadProc(LPVOID lpdwThreadParam);
CRITICAL_SECTION CriticalSection;
bool destroyed = false;
bool inited = false;
bool busy[H];
int frame[H];


#define MAX_LOADSTRING 100

// ���������� ����������:
HINSTANCE hInst;								// ������� ���������
volatile HWND hWnd;
TCHAR szTitle[MAX_LOADSTRING];					// ����� ������ ���������
TCHAR szWindowClass[MAX_LOADSTRING];			// ��� ������ �������� ����

// ��������� ���������� �������, ���������� � ���� ������ ����:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);


void InitScene()
{
	double kd_black[] = {0, 0, 0};
	double ks_black[] = {0, 0, 0.2};
	int      n_black[]  = {1, 1, 1};
	IMaterial m_black = new Materials::DuffuseSpecularMaterial(kd_black, ks_black, n_black);

	double kd_white[] = {1, 1, 0.8};
	double ks_white[] = {0, 0, 0.2};
	int      n_white[]  = {1, 1, 1};
	IMaterial m_white = new Materials::DuffuseSpecularMaterial(kd_white, ks_white, n_white);

	ITexturedMaterial m_chess = new Materials::CheckeredMaterial(10, 10, m_white, m_black);
	
	double kd_red[] = {1, 0, 0};
	double ks_red[] = {0, 0, 0};
	int      n_red[]  = {0, 0, 0};
	IMaterial m_red = new Materials::DuffuseSpecularMaterial(kd_red, ks_red, n_red);
	
	double kd_blue[] = {0, 0, 1};
	double ks_blue[] = {0, 0, 0};
	int      n_blue[]  = {0, 0, 0};
	IMaterial m_blue = new Materials::DuffuseSpecularMaterial(kd_blue, ks_blue, n_blue);
	
	double kd_green[] = {0, 1, 0};
	double ks_green[] = {0, 0, 0};
	int      n_green[]  = {0, 0, 0};
	IMaterial m_green = new Materials::DuffuseSpecularMaterial(kd_green, ks_green, n_green);

	double kd_yellow[] = {1, 1, 0};
	double ks_yellow[] = {0, 0, 0};
	int      n_yellow[]  = {0, 0, 0};
	IMaterial m_yellow = new Materials::DuffuseSpecularMaterial(kd_yellow, ks_yellow, n_yellow);

	double kd1[] = {0.9, 0.6, 0.3};
	double ks1[] = {0, 0, 0};
	int      n1[]  = {0, 0, 0};
	IMaterial m1 = new Materials::DuffuseSpecularMaterial(kd1, ks1, n1);

	double kd2[] = {0.6, 0.1, 1};
	double ks2[] = {0, 0, 0};
	int      n2[]  = {0, 0, 0};
	IMaterial m2 = new Materials::DuffuseSpecularMaterial(kd2, ks2, n2);
	
	double kd3[] = {0, 0, 0};
	double ks3[] = {1, 1, 1};
	int      n3[]  = {1, 1, 1};
	IMaterial m3 = new Materials::DuffuseSpecularMaterial(kd3, ks3, n3);
	
	double rrefract[] = {1, 1, 1};
	double refract = 1 / 2.0;
	IMaterial m_refractor = new Materials::IdealRefractorMaterial(rrefract, refract);
	
	ITexturedMaterial m_chess2 = new Materials::CheckeredMaterial(10, 1, m_red, m_green);

	double Le1[] = {25, 25, 25};
	
	IShape floor     = new Shapes::Square(Vector(-0.5, -0.5, 1), Vector(-0.5, -0.5, 2), Vector( 0.5, -0.5, 1), m_chess);
	IShape ceiling   = new Shapes::Square(Vector(-0.5,  0.5, 1), Vector( 0.5,  0.5, 1), Vector(-0.5,  0.5, 2), m_yellow);
	IShape backWall  = new Shapes::Square(Vector(-0.5, -0.5, 2), Vector(-0.5,  0.5, 2), Vector( 0.5, -0.5, 2), m_refractor);
	IShape leftWall  = new Shapes::Square(Vector(-0.5,  0.5, 1), Vector(-0.5,  0.5, 2), Vector(-0.5, -0.5, 1), m_green);
	IShape rightWall = new Shapes::Square(Vector( 0.5,  0.5, 1), Vector( 0.5, -0.5, 1), Vector( 0.5,  0.5, 2), m_refractor);

	IShape ball1 = new Shapes::Sphere(Vector(   0, -0.4, 1.3), 0.1,  m1);
	IShape ball2 = new Shapes::Sphere(Vector(-0.23, 0, 1.3), 0.1, m2);
	IShape ball3 = new Shapes::Sphere(Vector(0.3, -0.3, 1.5), 0.15, m_refractor);

	IShape shapes[] = {
		floor,
		ceiling,
		backWall,
		leftWall,
		rightWall,

		//ball1,
		ball2,
		ball3,
	};
	
	IShape glossyShapes[] = {
		//floor,
		//ceiling,
		//backWall,
		//leftWall,
		//rightWall,

		//ball1,
		//ball2,
		ball3,
	};
	
	IShape diffuseShapes[] = {
		floor,
		ceiling,
		backWall,
		leftWall,
		rightWall,

		ball1,
		ball2,
		//ball3,
	};
	
	ILightSource lightSources[] = {
		new Lights::Square(Vector(-0.15, 0.5 - double_EPSILON, 1.35), Vector(0.15,  0.5 - double_EPSILON, 1.35), Vector(-0.15, 0.5 - double_EPSILON, 1.65), Luminance(Le1)),
		//new Lights::Square(Vector(-0.15, 0.45, 8.35), Vector(0.15,  0.45, 8.35), Vector(-0.15, 0.45, 8.65), Luminance(Le1)),
		//new Lights::Sphere(Vector(0, 0.5, 1.5), 0.1, Luminance(Le1)),
		//new Lights::Sphere(Vector(-0.3, -0.3, 1.5), 0.05, Luminance(Le1)),
	};
	
	scene = new Shapes::Scene(sizeof(shapes) / sizeof(IShape*), shapes);
	glossy = new Shapes::Scene(sizeof(glossyShapes) / sizeof(IShape*), glossyShapes);
	diffuse = new Shapes::Scene(sizeof(diffuseShapes) / sizeof(IShape*), diffuseShapes);
	lights = new Lights::CompositeLightSource(sizeof(lightSources) / sizeof(ILightSource*), lightSources);
}

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
	wcex.lpszMenuName	= 0; //MAKEINTRESOURCE(IDC_GLOBALLIGHTING);
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
	hInst = hInstance; // ��������� ���������� ���������� � ���������� ����������

	hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		50, 20, W, H + 20, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{
		return FALSE;
	}
	
	InitScene();

	engine = new Engines::SimpleTracing();

	srand(SEED);
	
	ZeroMemory(frame, sizeof(frame));
	ZeroMemory(busy, sizeof(busy));
	ZeroMemory(L, sizeof(L));

	InitializeCriticalSection(&CriticalSection);

	for(int i = 0; i< WORKERS; i++)
	{
		CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&ThreadProc, 0, 0, 0);
	}
	
	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	inited = true;
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
	RECT r;

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
		if(GetUpdateRect(hWnd, &r, false))
		{
			hdc = BeginPaint(hWnd, &ps);

			if(inited)
			{
				for(int j = r.top; j <= r.bottom && j < H; j++)
				{
					for(int i = r.left; i <= r.right && i < W; i++)
					{
						Luminance l = L[i * H + j] * (255.0 / frame[j]);
						SetPixel(hdc, i, j, RGB(l.r() > 255 ? 255 : l.r(),
												l.g() > 255 ? 255 : l.g(),
												l.b() > 255 ? 255 : l.b()
												));
					}

					SetPixel(hdc, 10, j, RGB((frame[j] % 2) * 255, 0, ((frame[j] + 1) % 2) * 255));
				}
			}
			else
			{
				r.left = 0;
				r.top = 0;
				r.bottom = H - 1;
				r.right = W - 1;
				FillRect(hdc, &r, (HBRUSH) (GetStockObject(BLACK_BRUSH)));
			}
			EndPaint(hWnd, &ps);
		}

		break;
	case WM_DESTROY:
		destroyed = true;
		PrintMemoryTable();
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



DWORD ThreadProc(LPVOID lpdwThreadParam)
{
	int j = -1;

	while(!destroyed)
	{
		EnterCriticalSection(&CriticalSection);
		
		if(j >= 0)
		{
			busy[j] = false;
			frame[j]++;

			RECT r;
			r.left   = 0;
			r.top    = j;
			r.right  = W;
			r.bottom = j + 1;
			InvalidateRect(hWnd, &r, false);
		}
		
		j = -1;

		for(int i = 0; i < H; i++)
		{
			if(!busy[i])
			{
				if(j < 0 || frame[i] < frame[j])
				{
					j = i;
				}

			}
		}

		busy[j] = true;

		LeaveCriticalSection(&CriticalSection);

		for(int i = 0; i < W && !destroyed; i++)
		{
			L[i * H + j] += ColorAtPixel(i + PIXEL_SIZE * (float)rand() / RAND_MAX - PIXEL_SIZE * 0.5, j + PIXEL_SIZE * (float)rand() / RAND_MAX - PIXEL_SIZE * 0.5, W, H, CAM_Z, CAM_SIZE, *scene, *diffuse, *glossy, *lights, *engine);
		}
	}

	return 0;
}