   M S B A M L   `   `   `  ���� M  IPowerMill_Helper, Version=0.3.654.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  *    $PowerMill_Helper.Tool.DynamicIslaned   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:PowerMill_Helper.Tool     $	��Top=�y��� *DynamicIslandOpen+  � 	Resources     	RBtnStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� TextAlignment �� Triggers P� Property	 P� Value
 N� EnterActions �� TargetProperty	 �� To g� Duration g� 	BeginTime N� ExitActions  BtnStyle3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth �� Triggers��%Z  &	        &	 �    �� " ��.���  � ��
 �������  � ��
 �   ���  �@�Cursor$
 Hand����  � �� �� " ��.R��� #��z��$ 8��._��� #��z��$ Center=�$��	10,0,10,0q�#��z�� P�  �@J�IsMouseOver	 . 
 �� .���� .���� $1 +(Border.Background).(SolidColorBrush.Color)�$ BlackX�$ 0:0:0.2D�$ 0:0:0X��� $4 .(TextBlock.Foreground).(SolidColorBrush.Color)�$ 
WhiteSmokeX�$ 0:0:0.2D�$ 0:0:0X� �� .���� .���� $1 +(Border.Background).(SolidColorBrush.Color)�$ 
WhiteSmokeX�$ 0:0:0.1D�$ 0:0:0X��� $4 .(TextBlock.Foreground).(SolidColorBrush.Color)�$ BlackX�$ 0:0:0.1D�$ 0:0:0X��� " ��.���  �@��CornerRadius$ 5����  � ��$ 2q���  � ��$	 110����  � ��$	 110����  � ��$ 	15,15,0,0q���  � ��
 ��İ���  �@�Cursor$
 Hand����  �@J�Effect   $ Black��$ 15Q�$ 0Q�-    P�  �@J�IsMouseOver	 . 
 �� .���� .���� $ Effect.Color�$ WhiteX�$ 0:0:0.2D� �� .���� .���� $ Effect.Color�$ BlackX�$ 0:0:0.2D�.��� .+��� -    ��Name$ DynamicIslanedAppBorder��
���   �$ 13,13,13,13��$��0,4,0,0q�$��28��$��28��$	��170��$	��900��$��Center=�$	��Top=� J� Effect   $ 0Q�   Opacity �� * ExpenDynamicIslandCloseEffect_+._�� $��0,30,0,0q�.+��� #��[ ._��� $��NC输出��$��20(�
�������
������ $��Center=�$��Center=��� #��[ ._��� $	��Log��$��20(�
�������
������ $��Center=�$��Center=��� 
���   �$ 13,13,13,13��$	��Top=�$��Center=�$	��900��$��28��$��0,4,0,0q�   $ Black��   	Direction$	 -45Q�$ 2Q�$ 0Q�._�� .+��� $
��Left=�$��0,0,0,0q� �� Orientation$ 
Horizontal=�.+��� $��57��$��16�� �� Padding$ 0q�$��0q�$
��Left=�$��Center=�$��5,0,0,0q�$ 8�� J� Clip ��  �� Rect$ 	0,0,57,16� �� RadiusX$ 8Q� �� RadiusY$ 8Q�._��� $��Image/Logo.png��$��UniformToFill=��� -   $��5,0,0,0q�$��15��$��15��$��Image/OpenFile.png�� � Cursor$
 Hand��� -   $��5,0,0,0q�.+��� $��18������ *SaveStateColor+ J� Opacity �� *SaveStateOpacity+   $ 0Q�$ 5Q� �� *SaveStateEffect+�� $��15��$��15��$��Image/SaveFile.png��$
 Hand���� -   $��15��$��12��$��5,0,0,0q�$��Image/SaveAnyWhere.png��$
 Hand���� $y�Hidden=�$
 Hand��$��15��$��13��$	��Top=�$
��Left=�$��5,7,0,0q�$��Image/addnewstargy.png��� .+���   �� Padding$  0q�$��15(�$��Bottom=�$��Center=�
�������$��5,0,0,2q�$
��Bold"����� *ProjectName+�� $��Right=�$ 
Horizontal=�.+��� -   $
 Hand��$	��⚙��
�������$��13(�$��Center=�$��5,0,5,2q�   $ White��$ 0Q�$ 5Q��� -   #��[  $��宏库��$��16��$��0,0,5,0q�$��Center=�$��Right=�! J� RenderTransformOrigin$	! 0,0&��� -   #��[  $��功能��$��16��$��0,0,5,0q�$��Center=�$��Right=�$	! 0,0&�