   M S B A M L   `   `   `  ����M  IPowerMill_Helper, Version=0.3.726.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PowerMill_Helper.ToolPowerMill_Helper.Tool  5      ?$clr-namespace:PowerMill_Helper.ClassPowerMill_Helper.Class  5      $    PowerMill_Helper.Tool.MacroLib   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:PowerMill_Helper.Tool     5      3CS$clr-namespace:PowerMill_Helper.Class     5        �� Left$	  300��5       
 �� Top$	 200��62   $	��800��5	      $	��450��6    � 	Resources 5       
  AppRes �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property	 P� Value
 �� CornerRadius � Cursor
 J� Uid  AppTitle �� Triggers	 �� X1	 �� X2	 �� Y1	 �� Y2��%?  &	        &	 *    �� 5   
   " ��6    .�5      ��  � ��
 ���� 6+   �� 5       � ��
 �   �6+   �� 5       � ��$ 24��6&   �� 5       � ��$ 24��6'   �� 5       � �� 5      �� 5      " ��6&    5      P� 5       �@J�IsMouseOver	 . . �5   "   ��  � �� �	#FFFFFA8F6?   �� 5   "    � ��
 �����6?   5      .R�5      �� $
 12��6"   #��z��65   $
 Hand��6_   ._�5      �� #��z��6(   $��Center=�6R   $��Center=�6o   #��z 6�   5      5      5      5       �� 5!   
   " ��6!   .�5"      ��  � ��
 ���� 6+   �� 5#       � ��
 �   �6+   �� 5$       � ��$	 Top=�62   �� 5%       � ��$ Center=�64   �� 5&       � ��$
 Bold"�6+   �� 5'       � ��$	 150��6&   �� 5(       � ��$ 24��6'   �� 5)       � ��$ 0,5,0,0q�6'   �� 5*       � �� 5+      �� 5,      " ��6&   .R�5-      �� ���	#FFAEC4C5$
 12��68   ._�5.      � .+�5/   "   �� #��z��6,   $��Center=�6K   $��Center=�6h   �� 50   "   -   5        $��20��50   D   $��20��6O   $��Right=�6[   $��0,0,2,0q�6w   ��51   &   �� 52   *   " ��60   .�53   .   ��  � ��
 ���� 6K   �� 54   .    � ��
 �   �6K    55   .   P� 56   2    �@J�IsMouseOver	 . . �57   6   ��  � ��
 �  ��6S   �� 58   6    � ��
 �����6S   59   3   5;   +   ��5>   &   �� 5?   *   " ��6:   .R�5@   .   �� $
 10��66   #��z��6I   $
 Hand��6s   ._�5A   2   � .+�5B   6   �� $ 5��6;   $ 15��6B   $ 5��6J   $ 15��6Q   #��z��6Y   $��2��6   �� 5C   6   $ 15��6;   $ 5��6C   $ 5��6J   $ 15��6Q   #��z��6Y   $��2��6   5D   3   5E   /   5F   +   5I   #   5J      5K      5L      5N      -   5        5T      5U      .��5W      � .+�5X   
   ��  � UseLayoutRounding . 
�������$��0q�5Y       J� Opacity$ 1Q�6   $
 14��6$    J� Effect 5Z      3  -System.Windows.Media.Effects.DropShadowEffect  5[         ShadowDepth$ 1Q�6#      RenderingBias$ Quality=�63      
BlurRadius$ 10Q�6K   ._�5]      � .+�5^      �� -   5        $��宏库��5^      #��[ 6"   R� 5c      -   5        $��0q�5c      $��16(�61   $��10,40,10,10q�5d      
�������$��0,5,0,0q�5e      $��Disabled=�5f       J� RenderTransformOrigin$	 0,0&�5g      
���������5i      �� *MacorLibTreeViewList+ 5m       �� Source�� 5n      $P J pack://application:,,,/PowerMill_Helper;component/Theme/Win8TreeView.xaml?�6-   %    ��5p      �� 5q       �� DataType%   PowerMill_Helper.Class.NamePath"  63    �� ItemsSource 6S   ��  �� Path$ Children�.R�5r      ��  �� Orientation$ 
Horizontal=�6*   
������ $
 Hand��6\   .+�5s   "   �� $��Center=�6(   $��16��6_   $��16��6j   $��0,0,3,2q�6v   ��6C   �� *
IconPath+6v   �� 5t   "   $��Center=�6-   ��6I   �� *	PathStr+ � ToolTip 5u   "   �� 5v   &   $��Center=�60   $��NoWrap=�6d   ��6K   �� *	PathStr+6~   5x      5y      5{      5|      5}      5~      5�      