   M S B A M L   `   `   `  ����M  IPowerMill_Helper, Version=0.3.726.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PowerMill_Helper.ClassPowerMill_Helper.Class  5          PowerMill_Helper.MainForm   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:PowerMill_Helper.Class     5      -   5          �Name$  MainPage��5        � Title$
 NULL��5       � WindowStyle$
 None=�6    � AllowsTransparency . $��
ExtraLight"�6a   
���   ���6H   Z�  � DataContext 5
      #   PowerMill_Helper.Class.MainCS  5   
   .��5      � -   5         �Name$ MainFormGrid��5      .+�5   
   �� -   5         ��Name$ AppGrid��5       J� Effect 5      -  'System.Windows.Media.Effects.BlurEffect  5         Radius 6   �� *AppGirdBlurRadius+5      � 5   
   -   5        
�������	 J� Opacity$		 0.3Q�5   K   y�6   �� *IsolateTouch+6Y   F� 5   
   
���   �$	��Top=�6   $
��Left=�64   $��0,0,0,0q�6O   y�6`   �� *DynamicIslandOpen+��5      �� 5      
 �� GeometryCombineMode$
 Exclude=�6#    �� 	Geometry1 5      �� 5       �� Rect$ 	0,0,15,15�6,    �� 	Geometry2 5      @� 5        @� Center$ 15,15&�6*    @� RadiusX$ 15Q�69    @� RadiusY$ 15Q�6F   5"      5$      F� 5%   
   
���   �$	��Top=�6   $��Right=�64   $��0,0,0,0q�6P   y�6a   �� *DynamicIslandOpen+��5&      �� 5'      $
 Exclude=�6#    5(      �� 5)      $ 	0,0,15,15�6,    5+      @� 5,      $
 0,15&�6*   $ 15Q�68   $ 15Q�6E   5.      50      51      52      