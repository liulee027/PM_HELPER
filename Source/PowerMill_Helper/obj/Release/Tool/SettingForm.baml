   M S B A M L   `   `   `  ���� M  IPowerMill_Helper, Version=0.3.654.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  '    !PowerMill_Helper.Tool.SettingForm   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:PowerMill_Helper.Tool       �� Left$	  300��
 �� Top$	 240��$	��450��$	��700�� � 	Resources  
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
 Hand��._�� .+��� $ 5��$ 15��$ 5��$ 15��#��z��$��2���� $ 15��$ 5��$ 5��$ 15��#��z��$��2��-   .����  � UseLayoutRounding . 
�������$��0q� J� Opacity$ 1Q�$
 14�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 1Q�   RenderingBias$ Quality=�   
BlurRadius$ 10Q�._�� .+��� -   $��设置��#��[ �� $��15,35,15,15q�$
��Auto=�$��Hidden=�.���� $��	0,35,0,10q�.+��� .+��� .+��� $��	灵动岛��$
��Bold"���  �� Orientation$ 
Horizontal=�.+��� $��灵动岛显示��$��10,0,0,0q��� $��3,0,0,0q� d� 	IsChecked �� *DynamicIslandIsVisibility+�� $��0,10,0,0q�.+��� .+��� $��宏库��$
��Bold"��� $ 
Horizontal=�.+��� .+��� -   $��添加��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� -   $��移除��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� -   $��上移��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� -   $��下移��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� -   $��重载��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand����  �� SelectionMode$ Single=�$	��500��$	��100��$��15,5,0,0q�$	��Top=� �� SelectedValue �� * MacorLibFolderssettingSelected+���� *MacorLibFolders+���� .R��� ���� �� $��0,10,0,0q�.+��� .+��� $��Nc写出��$
��Bold"��� $ Vertical=�$��5,2,0,0q�.+��� $ 
Horizontal=�.+��� -	   $
 Hand��$ ��选择Nc写出文件夹：��$��0q��� $��5,0,0,0q����� *Ncout_OutputFolderPath+�� $ 
Horizontal=�$
��Left=�$��0,3,0,0q�.+��� $��合并输出���� $��5,0,0,0q�$
 Hand�� �� *Ncout_AllTpOutput+�� $��单独输出��$��10,0,0,0q��� $��5,0,0,0q�$
 Hand�� �� *Ncout_SingleTpOutput+�� $��0,3,0,0q�.+��� $ 
Horizontal=�.+��� -
   $
 Hand��$��选择程序单模板：��$��0q��� $��5,0,0,0q����� *Ncout_sheetTemplatePath+�� $��后处理配置��$��0,5,0,0q��� $ 
Horizontal=�.+��� -   $
��Left=�$��Center=�$��添加��$��0,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� -   $
��Left=�$��移除��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� -   $
��Left=�$��保存��$��5,5,0,0q�$��5,1,5,1q�$��0q�$
 Hand���� $ Extended=�$
��Left=�$	��130��$	��Top=�$	��550��$��1q����� *NcOpts+ �� SelectedItem �� *SettingNcoutSelectOpt+ �� View �� .P��� $��后处理名称�� �� Width$	 100�� �� CellTemplate �� .R��� ���� *Name+�� $��可指定输出基准��$	 100�� �� .R���  �� *OptCanSelectWkOutput+�� $��可指定输出坐标系��$	 100�� �� .R���  �� *OptCanSelectMachineWk+�� $��输出坐标系列表��$	 250�� �� .R��� ���� *OptSelectMachineWkName+�� $��0,10,0,0q�.+��� .+��� $��系统��$
��Bold"��� $��0,3,0,0q�.+��� -   $
 Hand��$��打开设置文本��$��0q�$
��Left=��� $ 
Horizontal=�$��0,3,0,0q�.+��� $��	版本：���� $��12.0.0.0��$��5,0,0,0q��� $��检查版本=>��$��15,0,0,0q��� -   $��3,0,0,0q�$��0q����� *Updatetitle+