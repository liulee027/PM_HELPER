   M S B A M L   `   `   `  ���� M  IPowerMill_Helper, Version=0.3.654.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  ?$clr-namespace:PowerMill_Helper.ClassPowerMill_Helper.Class  !    PowerMill_Helper.Tool.NCout   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:PowerMill_Helper.Tool     3CS$clr-namespace:PowerMill_Helper.Class       �� Left$	  300��
 �� Top$	 200��$	��800��$	��700�� � 	Resources    RainbowBrush �� 
StartPoint �� EndPoint � Color � Offset 
 AppRes �� 
TargetType �� Property	 �� Value
 �� 
TargetType �� Triggers P� Property P� Value �� CornerRadius � Cursor
 J� Uid  AppTitle �� Triggers	 �� X1	 �� X2	 �� Y1	 �� Y2  EntitySelect ��Name J� Opacity J� Effect-  'System.Windows.Media.Effects.BlurEffect   Radius :� RoutedEvent �� 
TargetName �� TargetProperty	 W� To g� Duration  NormalRoundButton  lightbutton��%+
  &	        &	 �     &	     &	 -    7   &	 �    &	 L    �� $	 0,0&�$	 1,1&�.��� $	 Red��$	 0.0Q�� $ Orange��$
 0.17Q�� $ Yellow��$
 0.33Q�� $ Green��$	 0.5Q�� $
 Blue��$
 0.67Q�� $ Indigo��$
 0.83Q�� $ Violet��$	 1.0Q��� " ��.���  � ��
	 ���� ��  � ��
	 �   ���  � ��$	 24����  � ��$	 24����  � ��	 �� "
 �� P�  �@J�IsMouseOver . . ���  � ��	 �	#FFFFFA8F��  � ��
	 �����.R��� $ 12��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z �� " ��.���  � ��
	 ���� ��  � ��
	 �   ���  � ��$		 Top=���  � ��$	 Center=���  � ��$
	 Bold"���  � ��$		 150����  � ��$	 24����  � ��$	 0,5,0,0q���  � ��	 �� "
 ��.R��� ���	#FFAEC4C5$ 12��._�� .+��� -   $��20��$��20��$
��Left=�$��2,0,0,0q����� " ��.���  � ��
	 ���� ��  � ��$	 0q���  � ��
	 �   ���  � ��$	 
ExtraLight"� P�  �@J�IsMouseOver . . ���  � ��
	 ���_���  � ��
	 ��������� "
 ��.R��� $ 10��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�$	��♻���� #��z��$��Center=�$��Center=��� -   $��20��$��20��$��Right=�$��0,0,2,0q����� " ��.���  � ��
	 ���� ��  � ��
	 �   � P�  �@J�IsMouseOver . . ���  � ��
	 �  ����  � ��
	 ��������� "
 ��.R��� $ 10��#��z��$
 Hand��._�� .+��� $ 5��$ 15��$ 5��$ 15��#��z��$��2���� $ 15��$ 5��$ 5��$ 15��#��z��$��2��-   �� " ��.���  � ��	 �� "
 ��.R�� .+��� $ RainbowShadow��$ 5��$ 0Q�8��  $
 Hand��   $ 20Q��� $ 5��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z�� :� $ 
MouseEnter��.[��� .���� .��W� $ RainbowShadow��$ Opacity�$ 1X�$ 0:0:0.4D�:� $ 
MouseLeave��.[��� .���� .��W� $ RainbowShadow��$ Opacity�$ 0X�$ 0:0:0.4D��� " ��.���  � ��	 �� "
 ��.R�� .+��� $ 5��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z���� " ��.���  � ��	 �� "
 ��.R�� .+��� $ RainbowShadow��$ 5��$ 0Q�#��z��$
 Hand��   $ 20Q��� $ 5��#��z��$
 Hand��._��� #��z��$��Center=�$��Center=�#��z�� :� $ 
MouseEnter��.[��� .���� .��W� $ RainbowShadow��$ Opacity�$ 1X�$ 0:0:0.4D�:� $ 
MouseLeave��.[��� .���� .��W� $ RainbowShadow��$ Opacity�$ 0X�$ 0:0:0.4D�.����  � UseLayoutRounding . 
�������$��0q�$ 1Q�$ 14�� 3  -System.Windows.Media.Effects.DropShadowEffect      ShadowDepth$  0Q�!   RenderingBias$! Quality=�"   
BlurRadius$" 10Q�._�� .+��� -   $��NC输出��#��[ �� $��
10,30,10,0q�# �� Orientation$# 
Horizontal=�.+��� -   $ ��Name$$ LeftList��$��16(�$	��200��$
��Left=�% �� SelectionMode$% Extended=�
�������$��0,80,0,0q�$	��550��$	��Top=����� *NcOutToolpathCollection+���� .R��� $# 
Horizontal=�
������ $
 Hand��.+�� $
��Left=����� *	uiwidth+�� $��Center=�$��16��$��16��$��0,0,3,2q����� *
TypeIcon+�� $��Center=�$��5,0,0,0q�& �� Padding$& 5,0,5,0q����� *Name+���� *	BackColor+' � ToolTip' �� $��Center=�$��NoWrap=����� *Name+�� $��Center=�.+��� -   #��[ ���	#FF96DC93$��25��$��75��$	��→��$��5,0,5,0q�$
 Hand��$��0q�$��	10,0,10,0q��� -   #��[ 
����p��$��75��$	��❎��$��5,0,5,0q�$
 Hand��$��0q�$��
10,25,10,0q��� .+��� $# 
Horizontal=�$��Center=�$��0,10,0,0q�.+��� $��后处理：��$��85��$��17(�$��Center=��� ( �� DisplayMemberPath$
( Name��) �� SelectedValuePath$) FilePath��$	��200��$
 Hand������ *NcOpts+* �� SelectedItem* �� *NcoutOptSelectOpt+�� $# 
Horizontal=�$��Center=�$��0,10,0,0q�y��� *NcoutSetWorkplanesVisibility+.+��� $��输出基准：��$��85��$��17(�$��Center=��� -   #��[ $	��200��$��20��$��0q�
+ � Tag$$+ Ncout_Selected_OutputWorkplane��$ 	Workplane������ *Name+, �� UpdateSourceTrigger$, PropertyChanged=�- � DataContext- �� * Ncout_Selected_OutputWorkplane+$, PropertyChanged=��� -	   . ��Name$. RightTpListbox��$% Extended=�$��	0,15,0,20q�$	��550��$	��Top=����� *Ncout_ReadlyList+ ��%J   '��       �� " ��.���  � y�$	 Visible=���  � ��$
	 Left=�/ �� View/ �� .P��� $��刀号��0 �� Width$0 30��1 �� DisplayMemberBinding1 �� *NcoutToolNumber+�� $�� ��$0 25��2 �� CellTemplate2 �� .R��� $��Center=�$��16��$��16��$��0,0,0,2q����� *
TypeIcon+�� $��	坐标系��1 �� *NcoutMachineWorkplane+0 �� *NcoutShowWorkplanesVisibility+�� $��刀路名称��$	0 140��1 �� *Name+�� $��
10,100,0,0q�$��Right=�$	��230��.+�� $��0,5,0,0q����� *$"NcoutShowChangeworkplaneVisibility+.+��� -
   $��8,0,8,0q����� *.,NcoutOptSelectOpt.OptSelectMachineWkNameLIst+�� -   #��[ $
 Hand��$��更改坐标系��$��0q�� $��0,5,0,0q�$��23��.+��� -   $��5,0,5,0q�$��18(��� -   #��[ $
 Hand��$��更改刀号��$��0q��� $��0,10,0,5q�.+��� $# 
Horizontal=�$��Center=�.+��� $��合并输出���� $��5,0,0,0q�$
 Hand��3 d� 	IsChecked3 �� *Ncout_AllTpOutput+�� $��单独输出��$��10,0,0,0q��� $��5,0,0,0q�$
 Hand��3 �� *Ncout_SingleTpOutput+�� -   #��[ $��20��$
 Hand��$��0,3,0,0q�$��创建NC程序��$��0q��� -   #��[ $��20��$
 Hand��$��输出NC程序单��$��0,5,0,5q�$��0q��� -   #��[ $��20�����	#FF96DC93$��Nc写出��$��0,5,0,5q�$��0q��� -   #��[ $��20��$��打开Nc程序文件夹��$��0,5,0,0q�$��0q�