   M S B A M L   `   `   `  ����N  JCalculatorViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullX?clr-namespace:Calculator.ViewModel;assembly=CalculatorViewModelCalculator.ViewModel  5   	    SimpleCalculator!   SimpleCalculator.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Pcalc?clr-namespace:Calculator.ViewModel;assembly=CalculatorViewModel     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)    � 	Resources 5      .   (Calculator.ViewModel.CalculatorViewModel   o��%   &	          5   
   6#   5
      .��5      � .+�5   
   �� -   5        $��txtOperandOne��5   �   $
��Left=�6   $��23��6-   $��	48,67,0,0q�69    �� TextWrapping$
 Wrap=�6L   $	��Top=�6�   $	��120��6�   ��6`   �� *txtOperandOne+ �� Source# [  6�   �� 5   
   -   5         ��Name$ txtOperandTwo��5   �   $
��Left=�6   $��23��6-   $��
203,67,0,0q�69   $
 Wrap=�6M   $	��Top=�6�   $	��120��6�   ��6a   �� *txtOperandTwo+# [  6�   �� 5   
   -   5        $��btnAdd��5   �   $	��Add��6   $
��Left=�6   $��
48,160,0,0q�6:   $	��Top=�6N   $��75��6f    J� RenderTransformOrigin$ -0.205,0.588&�6q   ��6�   �� *
AddCommand+ �� Mode$ OneWay=�# [  �� 5   
   -   5        $��	lblResult��5   �   $
��Left=�6I   $��
48,109,0,0q�6d   $	��Top=�6x   $	��120��6�   $��24��6�   ��6   �� *	lblResult+# [  6�   �� 5   
   -   5         ��Name$ btnSub��5   �   $	��Sub��6   $
��Left=�6   $��147,160,0,0q�6:   $	��Top=�6O   $��75��6g   $ -0.205,0.588&�6r   ��6�   �� *
SubCommand+$ OneWay=�# [  �� 5   
   -   5        $ btnMul��5   �   $	��Mul��6   $
��Left=�6   $��260,160,0,0q�6:   $	��Top=�6O   $��75��6g   $ -0.205,0.588&�6r   ��6�   �� *
MulCommand+$ OneWay=�# [  �� 5   
   -   5        $ btnDiv��5   �   $	��Div��6   $
��Left=�6   $��363,160,0,0q�6:   $	��Top=�6O   $��75��6g   $ -0.205,0.588&�6r   ��6�   �� *
DivCommand+$ OneWay=�# [  5      5      