   M S B A M L   `   `   `  ���� M  IPowerMill_Helper, Version=0.3.654.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  #    PowerMill_Helper.Tool.CheckTP   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:PowerMill_Helper.Tool       �� Left$	  300��
 �� Top$	 200��$	��330��$	��600�� � 	Resources    RainbowBrush �� 
StartPoint �� EndPoint � Color � Offset ! SegmentedLeftRadioButtonStyle �� 
TargetType �� Property	 �� Value
 �� 
TargetType ��Name �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth  SegmentedRadioButtonStyle " SegmentedRightRadioButtonStyle 
 AppRes � Cursor
 J� Uid  AppTitle �� Triggers	 �� X1	 �� X2	 �� Y1	 �� Y2  EntitySelect J� Opacity J� Effect-  'System.Windows.Media.Effects.BlurEffect   Radius :� RoutedEvent �� 
TargetName  �� TargetProperty	! W� To" g� Duration  lightbutton��%  &	        &	 �     &	 �    &	 �    &	 �    &	 �    &	 �
    7   &	 "    �� $	 0,0&�$	 1,1&�.��� $	 Red��$	 0.0Q�� $ Orange��$
 0.17Q�� $ Yellow��$
 0.33Q�� $ Green��$	 0.5Q�� $
 Blue��$
 0.67Q�� $ Indigo��$
 0.83Q�� $ Violet��$	 1.0Q��� " 	�.���  � ��$	 40����  � ��$	 20����  �@�Cursor$
	 Hand����  � ��
	 �������  � ��
	 �   ���  � ��$	 0q���  � ��	 �� "
 	�.R��� $ border��#��z��#��z��#��z��$ 5,0,0,5��._��� $��Center=�$��Center=� P�  �@d�	IsChecked$
 TrueY�. ��� $ border�� � ��	 �	#FF96DC93�� $ border�� �@J�Effect	   $ 	#FF96DC93��$ 10Q�$ 0Q�P�  �@J�IsMouseOver . . ���  � ��
	 ������� " 	�.���  � ��$	 40����  � ��$	 20����  �@�Cursor$
	 Hand����  � ��
	 �������  � ��
	 �   ���  � ��$	 0q���  � ��	 �� "
 	�.R��� $ border��#��z��#��z��#��z��$ 0��._��� $��Center=�$��Center=� P�  �@d�	IsChecked$
 TrueY�. ��� $ border�� � ��	 �	#FF96DC93�� $ border�� �@J�Effect	   $ 	#FF96DC93��$ 3Q�$ 0Q�P�  �@J�IsMouseOver . . ���  � ��
	 ������� " 	�.���  � ��$	 40����  � ��$	 20����  �@�Cursor$
	 Hand����  � ��
	 �������  � ��
	 �   ���  � ��$	 0q���  � ��	 �� "
 	�.R��� $ border��#��z��#��z��#��z��$ 0,5,5,0��._��� $��Center=�$��Center=� P�  �@d�	IsChecked$
 TrueY�. ��� $ border�� � ��	 �	#FF96DC93�� $ border�� �@J�Effect	   $ 	#FF96DC93��$ 5Q�$ 0Q�P�  �@J�IsMouseOver . . ���  � ��
	 ������� " ��.���  � ��
	 ���� ��  � ��
	 �   ���  � ��$	 24����  � ��$	 24����  � ��	 �� "
 �� P�  �@J�IsMouseOver . . ���  � ��	 �	#FFFFFA8F��  � ��
	 �����.R��� $ 12��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z �� " ��.���  � ��
	 ���� ��  � ��
	 �   ���  � ��$		 Top=���  � ��$	 Center=���  � ��$
	 Bold"���  � ��$		 150����  � ��$	 24����  � ��$	 0,5,0,0q���  � ��	 �� "
 ��.R��� ���	#FFAEC4C5$ 12��._�� .+��� #��z��$��Center=�$��Center=��� -   $��20��$��20��$��Right=�$��0,0,2,0q����� " ��.���  � ��
	 ���� ��  � ��
	 �   � P�  �@J�IsMouseOver . . ���  � ��
	 �  ����  � ��
	 ��������� "
 ��.R��� $ 10��#��z��$
 Hand��._�� .+��� $ 5��$ 15��$ 5��$ 15��#��z��$��2���� $ 15��$ 5��$ 5��$ 15��#��z��$��2��-   �� " ��.���  � ��	 �� "
 ��.R�� .+��� $ RainbowShadow��$ 5��$ 0Q�8��  $
 Hand��   $ 20Q��� $ 5��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z�� :� $ 
MouseEnter��.[��� .���� .��W� $ RainbowShadow��$  Opacity�$! 1X�$" 0:0:0.4D�:� $ 
MouseLeave��.[��� .���� .��W� $ RainbowShadow��$  Opacity�$! 0X�$" 0:0:0.4D��� " ��.���  � ��	 �� "
 ��.R�� .+��� $ RainbowShadow��$ 5��$ 0Q����	#FF96DC93$
 Hand��   $ 20Q��� $ 5��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z�� :� $ 
MouseEnter��.[��� .���� .��W� $ RainbowShadow��$  Opacity�$! 1X�$" 0:0:0.4D�:� $ 
MouseLeave��.[��� .���� .��W� $ RainbowShadow��$  Opacity�$! 0X�$" 0:0:0.4D�.���� # � UseLayoutRounding# . 
�������$��0q�$ 1Q�$ 14��   $ 0Q�$   RenderingBias$$ Quality=�$ 10Q�._�� .+��� -   $��碰撞过切检查��#��[ � $��0,20,0,0q�.+��� $��检查列表��$
��Left=�$	��Top=�$��	10,10,0,0q��� -   $
��Left=�$	��500��$��	10,36,0,0q�$��0,20,0,0q�$	��Top=�% �� SelectionMode$% Single=�$	��165������ *CheckTPToolpathCollection+���� .R��� & �� Orientation$& 
Horizontal=�
������ .+��� $��15��$��Black"�$��5,0,0,0q�$
��Left=����� *Check1Result+���� *	CheckTP1C+�� $��15��$��Black"�$��2,0,0,0q�$
��Left=����� *Check2Result+���� *	CheckTP2C+�� $
��Left=�$��Black"�$��5,0,0,0q����� *Name+�� $��Right=�$��	0,50,10,0q�$	��135��.+��� $& 
Horizontal=�$��0,0,0,0q�$��Center=�.+�	� -   
' � Tag$' 0��( 	� 	GroupName$( CheckOptions��#��[ $��碰撞��$
��Left=�$	��Top=�	� -   $' 1��) d� 	IsChecked$
) TrueY�$( CheckOptions��#��[ $��两者��$
��Left=�$	��Top=�	� -   $' 2��$( CheckOptions��#��[ $��过切��$
��Left=�$	��Top=��� $��对照检查��$��11(�$	��120��$��Center=�$��39��$	��Top=�$��0,5,0,0q�.���� $& 
Horizontal=�.+��� $
��Left=�$��50��* �� SelectedIndex* �� *CheckTP_refer_Selectindex+.@��� $��模型���� $��残留���� -   $ 
Stockmodel��$"' CheckTP_refer_StockmodelName��#��[ $��45��$��Center=�$��	10,0,0 ,0q�y��� *CheckTP_refer_UseSTockmodel+���� *#!CheckTP_refer_StockmodelName.Name+�� $��11(�$	��120��$��Center=�$��63��$��0,5,0,0q����� $��分割刀具路径��) �� *CheckTP_SplitTP+.���� $��0,5,5,5q�$��Right=�.+��� $& 
Horizontal=�$��Right=�.+��� $��输出安全移动���� ) �� *CheckTP_SplitTP_usesafe+�� $& 
Horizontal=�.+��� $��输出不安全路径���� ) �� *CheckTP_SplitTP_useunsafe+�� $��碰撞选项��$��11(�$	��120��$��Center=�$��76��$��0,5,0,0q�.��� .+��� $��夹持间隙��$
��Left=�$	��Top=�$��0,5,0,0q��� $��刀柄间隙��$
��Left=�$	��Top=�$��0,29,0,0q��� $
��Left=�$��23��+ �� TextWrapping$+ NoWrap=�$��Center=�$��Center=�$	��Top=�$��45��$��59,5,0,0q����� *CheckTP_CkeckToolHolderGAP+�� $
��Left=�$��23��$+ NoWrap=�$��Center=�$��Center=�$	��Top=�$��45��$��	59,29,0,0q����� *CheckTP_CkeckToolGAP+�� $
��Left=�$��8,0,0,0q�.+��� $��计算碰撞深度��$	��Top=�$
��Left=�$��18��$��Center=�$��0,5,0,0q�) �� *CheckTP_CheckSafeDepth+�� $��显示不安全移动��$	��Top=�$
��Left=�$��18��$��Center=�$��0,5,0,0q�) �� *CheckTP_ShowUnSafe+�� $��对照检查��$��11(�$	��120��$��Center=�$��88��$��0,5,0,0q�.���� .+��� $& 
Horizontal=�$��0,0,0,0q�$��Center=�.+�	� -	   $' 0��$��35��$
) TrueY�$( 	CheckType��#��[ $	��原��$
��Left=�$	��Top=�	� -
   $' 1��$��35��$( 	CheckType��#��[ $	��对��$
��Left=�$	��Top=�	� -   $' 2��$��35��$( 	CheckType��#��[ $��5Axis��$
��Left=�$	��Top=��� y��� *CheckTP_transform_Enable+.+��� $��0q�$��镜像基准��$��Center=�$	��Top=�$��0,8,0,0q��� -   #��[ $!' CheckTP_transform_Workplane��$ 	Workplane��$��20��$��Center=�$	��105��$��0,3,0 ,0q����� *" CheckTP_transform_Workplane.Name+�� -   #��[ $��检查��$��Center=�$��0,15,0,0q�$��Bottom=�$	��100��$��18��