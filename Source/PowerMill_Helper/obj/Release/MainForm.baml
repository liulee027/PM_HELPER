   M S B A M L   `   `   `  ���� M  IPowerMill_Helper, Version=0.3.654.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PowerMill_Helper.ClassPowerMill_Helper.Class      PowerMill_Helper.MainForm   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:PowerMill_Helper.Class     -     �Name$  MainPage�� � Title$
 NULL�� � WindowStyle$
 None=� � AllowsTransparency . $��
ExtraLight"�
���   ���Z�  � DataContext #   PowerMill_Helper.Class.MainCS  .��� -    �Name$ MainFormGrid��.+��� -    ��Name$ AppGrid�� J� Effect -  'System.Windows.Media.Effects.BlurEffect     Radius �� *AppGirdBlurRadius+� -   
�������	 J� Opacity$		 0.3Q�y��� *IsolateTouch+F� 
���   �$	��Top=�$
��Left=�$��0,0,0,0q�y��� *DynamicIslandOpen+���� 
 �� GeometryCombineMode$
 Exclude=� �� 	Geometry1 ��  �� Rect$ 	0,0,15,15� �� 	Geometry2 @�  @� Center$ 15,15&� @� RadiusX$ 15Q� @� RadiusY$ 15Q�F� 
���   �$	��Top=�$��Right=�$��0,0,0,0q�y��� *DynamicIslandOpen+���� $
 Exclude=� �� $ 	0,0,15,15� @� $
 0,15&�$ 15Q�$ 15Q�