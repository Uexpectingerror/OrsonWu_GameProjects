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

Camera Motion скрипт создан для придания движению камеры большей кинематографичности.
Скрипт базируется на создании кривых Безье. Сначала пользователь создаёт путь для камеры, а затем в игре посредством нахождения местоположения персонажа
между двумя конечными точками кривой камера находит своё местоположение на этой кривой и тем самым движется по ней.

Для того, чтобы посмотреть демо сцену нужно:
	1) Запустить сцену
	2) Управлять сферой с помощью кнопок A, D на клавиатуре

Для того, чтобы использовать скрипт в своём проекте нужно:
1) Назначить скрипт CameraMotion из папки Scripts на камеру или добавить через Add component
2) Создать персонажа и поставить перед камерой
	2.1) Назначить персонажа в поле Target вышеописанного скрипта
	2.2) Персонаж должен двигаться строго по осям Х и Y (Можно и по Z, но персонаж не должен пересекать путь камеры, иначе его не будет видно :) )
3) Создать кривую (Вдоль оси Х)
	3.1) Кнопка Create curve - создаёт кривую. Все точки кривой имеют координаты камеры, то есть кривая создаётся там, 
		 где находится камера (если запустить сцену без создания, появится ошибка, сообщающая о том что, кривая не создана), 
		 кнопка Add curve - добавляет новую кривую к уже созданной. 
		 После добавления новой кривой, все её точки создадутся в конечной точке последней кривой
	3.2) Напротив названий кривых в инспекторе находятся кнопки: S - для выделений кривой, Х - для удаления.
	3.3) После создания или какого либо изменения кривой необходимо обязательно сохранить результат 
		 (при запуске игры без сохранения появляется ошибка, сообщающая о том, что кривая не сохранена)
4) Запустить сцену и управлять созданным персонажем