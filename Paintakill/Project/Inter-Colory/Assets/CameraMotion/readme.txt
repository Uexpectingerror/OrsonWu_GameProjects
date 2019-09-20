"English language"

Camera Motion the script is created to make the camera movement more cinematic.
The script is based on creating bezier curves. First, the user creates a path for the camera and then in the game by finding the location of the character
between the two end points of the curve camera finds his location on the curve and thereby moves on her.

In order to view the demo scene you need to:
	1) Start the scene
	2) to Control the scope using the buttons A, D on the keyboard

In order to use the script in your project you need to:
	1) Assign the CameraMotion script from the Scripts folder on the camera or add it using the Add component
	2) to Create a character and put in front of the camera
		2.1) Assign character in the Target field of the above script
		2.2) the Character must move strictly along the axes X and Y (and Z, but the character must not cross the path of the camera, otherwise it will not be seen :) )
	3) Create a curve (Along X axis)
		3.1) Create curve - creates a curve. All curve points have the coordinates of the camera, that is, the curve is created there 
 			 where the camera is (if you run the scene without creating, an error appears, stating that the curve is not created) 
			 Curve Add - adds a new curve to have already been created. 
			 After adding a new curve, all points will be created at the end point of the last curve
		3.2) Opposite the names of the curves in the inspector are buttons: S - select curve, X - delete curve.
		3.3) After you create or any changes in the curve it is necessary save the result 
			 (when you start the game without saving, you receive an error indicating that the curve is not saved)
	4) Start the scene and control the established character


"Russian language"

Camera Motion ������ ������ ��� �������� �������� ������ ������� �������������������.
������ ���������� �� �������� ������ �����. ������� ������������ ������ ���� ��� ������, � ����� � ���� ����������� ���������� �������������� ���������
����� ����� ��������� ������� ������ ������ ������� ��� �������������� �� ���� ������ � ��� ����� �������� �� ���.

��� ����, ����� ���������� ���� ����� �����:
	1) ��������� �����
	2) ��������� ������ � ������� ������ A, D �� ����������

��� ����, ����� ������������ ������ � ���� ������� �����:
1) ��������� ������ CameraMotion �� ����� Scripts �� ������ ��� �������� ����� Add component
2) ������� ��������� � ��������� ����� �������
	2.1) ��������� ��������� � ���� Target �������������� �������
	2.2) �������� ������ ��������� ������ �� ���� � � Y (����� � �� Z, �� �������� �� ������ ���������� ���� ������, ����� ��� �� ����� ����� :) )
3) ������� ������ (����� ��� �)
	3.1) ������ Create curve - ������ ������. ��� ����� ������ ����� ���������� ������, �� ���� ������ �������� ���, 
		 ��� ��������� ������ (���� ��������� ����� ��� ��������, �������� ������, ���������� � ��� ���, ������ �� �������), 
		 ������ Add curve - ��������� ����� ������ � ��� ���������. 
		 ����� ���������� ����� ������, ��� � ����� ���������� � �������� ����� ��������� ������
	3.2) �������� �������� ������ � ���������� ��������� ������: S - ��� ��������� ������, � - ��� ��������.
	3.3) ����� �������� ��� ������ ���� ��������� ������ ���������� ����������� ��������� ��������� 
		 (��� ������� ���� ��� ���������� ���������� ������, ���������� � ���, ��� ������ �� ���������)
4) ��������� ����� � ��������� ��������� ����������