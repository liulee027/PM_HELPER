   M S B A M L   `   `   `  ���� M  IPowerMill_Helper, Version=0.3.654.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  ?$clr-namespace:PowerMill_Helper.ClassPowerMill_Helper.Class  $    PowerMill_Helper.Tool.MacroLib   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:PowerMill_Helper.Tool     3CS$clr-namespace:PowerMill_Helper.Class       �� Left$	  300��
 �� Top$	 200��$	��800��$	��450�� � 	Resources  
  AppRes �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property	 P� Value
 �� CornerRadius � Cursor
 J� Uid  AppTitle �� Triggers	 �� X1	 �� X2	 �� Y1	 �� Y2��%�  &	        &	 4    �� " ��.���  � ��
 ���� ��  � ��
 �   ���  � ��$ 24����  � ��$ 24����  � �� �� " �� P�  �@J�IsMouseOver	 . . ���  � �� �	#FFFFFA8F��  � ��
 �����.R��� $
 12��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z �� " ��.���  � ��
 ���� ��  � ��
 �   ���  � ��$	 Top=���  � ��$ Center=���  � ��$
 Bold"���  � ��$	 150����  � ��$ 24����  � ��$ 0,5,0,0q���  � �� �� " ��.R��� ���	#FFAEC4C5$
 12��._�� .+��� #��z��$��Center=�$��Center=��� -   $��20��$��20��$��Right=�$��0,0,2,0q����� " ��.���  � ��
 ���� ��  � ��
 �   � P�  �@J�IsMouseOver	 . . ���  � ��
 �  ����  � ��
 ��������� " ��.R��� $
 10��#��z��$
 Hand��._�� .+��� $ 5��$ 15��$ 5��$ 15��#��z��$��2���� $ 15��$ 5��$ 5��$ 15��#��z��$��2��-   .��� .+���  � UseLayoutRounding . 
�������$��0q� J� Opacity$ 1Q�$
 14�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 1Q�   RenderingBias$ Quality=�   
BlurRadius$ 10Q�._�� .+��� -   $��宏库��#��[ R� -   $��0q�$��16(�$��10,40,10,10q�
�������$��0,5,0,0q�$��Disabled=� J� RenderTransformOrigin$	 0,0&�
����������� *MacorLibTreeViewList+  �� Source�� $P J pack://application:,,,/PowerMill_Helper;component/Theme/Win8TreeView.xaml?�%    ����  �� DataType%   PowerMill_Helper.Class.NamePath"   �� ItemsSource ��  �� Path$ Children�.R���  �� Orientation$ 
Horizontal=�
������ $
 Hand��.+��� $��Center=�$��16��$��16��$��0,0,3,2q����� *
IconPath+�� $��Center=����� *	PathStr+ � ToolTip �� $��Center=�$��NoWrap=����� *	PathStr+