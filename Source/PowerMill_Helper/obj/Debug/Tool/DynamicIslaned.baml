   M S B A M L   `   `   `  ����M  IPowerMill_Helper, Version=0.2.147.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  5      *    $PowerMill_Helper.Tool.DynamicIslaned   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:PowerMill_Helper.Tool     5      $	��Top=�5      .��5      �� 
���   �$��4q�6$     �� CornerRadius$  12,12,12,12��5   
   $��24��6%   $��Center=�5      $	��900��6*    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6       	Direction$	 -45Q�6.      ShadowDepth$ 2Q�6>      Opacity$ 0Q�6N   ._�5   
   � .+�5      �� $
��Left=�6   $��0,0,0,0q�65    �� Orientation$ 
Horizontal=�6F   .+�5      �� $��57��6   $��16��6$    �� Padding$ 0q�60   $��0q�6<   $
��Left=�5      $��Center=�6)   $��5,0,0,0q�5      $  8��6    J� Clip 5      �� 5      	 �� Rect$	 	0,0,57,16�6,   
 �� RadiusX$
 8Q�6=    �� RadiusY$ 8Q�6I   ._�5      �� $��Image/Logo.png��6   $��UniformToFill=�64   5      �� 5       -   5        $��5,0,0,0q�5    >   $��15��6O   $��15��6[   $��Image/OpenFile.png��6h    � Cursor$
 Hand��6�   5"      � 5$      -   5        $��5,0,0,0q�5$   5   .+�5%      �� $��18��6    ��6-   �� *SaveStateColor+ J� Opacity 6T   �� *SaveStateOpacity+ 5&        5'      $ 0Q�6R      
BlurRadius$ 5Q�6b    60   �� *SaveStateEffect+6b   5)      �� 5*      $��15��6   $��15��6'   $��Image/SaveFile.png��64   $
 Hand��6R   5+      �� 5-      -   5        $��15��5-   <   $��12��6G   $��5,0,0,0q�6U   $��Image/SaveAnyWhere.png��6g   $
 Hand��6�   �� 5/      $y�Hidden=�6   $
 Hand��6,   $��15��6;   $��13��6F   $	��Top=�6T   $
��Left=�6l   $��5,7,0,0q�6�   $��Image/addnewstargy.png��6�   50      � 52      .+�53      ��  �� Padding$ 0q�6<   $��15(�6H   $��Bottom=�54      $��Center=�66   
�������$��5,0,0,2q�55   /   $
��Bold"�6@   ��53      �� *ProjectName+58      5:      �� 5;      $��Right=�6   $ 
Horizontal=�65   .+�5=      �� -   5        $
 Hand��5=      $	��⚙��6T   
�������$��13(�6q   $��Center=�6   $��5,0,5,2q�6�    5>        5?      $ White��6,   $ 0Q�6:   $ 5Q�6J   5A      �� 5D      -   5        $
 Hand��5E      $��16��6)   $��55��65   $��0,0,5,0q�6@   $��Center=�5F      $��Right=�6>    J� RenderTransformOrigin$	 0,0&�6Z   ��5G      �� 5H       �� 
TargetType" ��6    .�5I      ��  �� Property � �� �� Value
 �����6;    �� Triggers 5J      P� 5K   "    P� Property �@J�IsMouseOver P� Value . . �5L   &   ��  � ��
 �   �6C   �� 5M   &    � ��
 �����6C   5N   #   5P      ��5R      �� 5S       �� 
TargetType" ��6*   .R�5T      �� #��z��6%   $  8��6P   ._�5U   "   �� $��宏库��6,    �� TextAlignment$ Center=�66   5V      5W      5Z      5\      5^      5_      5`      