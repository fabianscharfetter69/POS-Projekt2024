   M S B A M L   `   `   `  ����K  GGeburtstagsliste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:GeburtstagslisteGeburtstagsliste  5   	   !    Geburtstagsliste.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Geburtstagsliste     5   	     � Title$  GeburtstagsListe��5   	   $	��450��6"   $	��700��6/    � Icon$ /calenderIcon.png��6;   .��5	      � .+�5
   
   �� .+�5      �� $��Center=�6   $��20q�61   $
��Bold"�6=   $��20(�6O   .��6]   Geburtstagsliste6o   �� 5       �� Orientation$ 
Horizontal=�6   $	��250��62   .+�5      &   System.Windows.Controls.Calendar  -   5          Name$ Calendar��5      $��15q�6^      DisplayMode$ Month=�6j      SelectionMode$ 
SingleDate=�6~   6�   �� 5      $ Vertical=�6   $��15q�64   .+�5      �� -   5         ��Name$ textBlockTag��5       $	��170��66   $
��Left=�6E   $��0,0,0,10q�6`   $ ��Tag im Kalender auswählen��6r   $
��Bold"�6�   6�   �� 5      -   5         ��Name$ 	labelName��5      $y�Hidden=�6/   $
��Left=�6C   .��6^   Name eingeben:6n   �� 5      -   5         ��Name$ textBoxName��5      $y�Hidden=�63   $
��Left=�6G   $	��170��6b   $��0,0,0,10q�6n   6�   �� 5      $ 
Horizontal=�6!   .+�5      �� -   5        $ labelGeburtsjahr��5       $y�Hidden=�6:   $
��Left=�6N   .��6i   Geburtsjahr hinzufügen?6�   �� 5      -   5        	 ��Name$	 checkBoxGeburtsjahr��5   #   $y�Hidden=�6�   $
��Left=�6�   $��0,6,0,0q�6�   6�   5      �� 5      -   5        
 ��Name$
 comboBoxGeburtsjahr��5      $y�Hidden=�6<   $
��Left=�6P   $	��170��6k   $��0,0,0,10q�6w   6�   �� 5      -   5         ��Name$ 
buttonName��5      $
��Left=�6J   $y�Hidden=�6e   $	��170��6y   $��10q�6�   .��6�   Hinzufügen6�   5      �� 5       .+�5!      �� -	   5        $ labelGeburtstage��5!      $��15,0,0,0q�66   .��6H   Alle Geburtstage:6[   �� 5#      $
��Auto=�6#   $
��Auto=�6F   .��5$      �� -
   5         ��Name$ listView��5$   0   $	��200��6#   $	��200��6o   $��15,0,0,0q�6~   5%      �� 5'      -   5        $	 checkBox��5'      $��Right=�62   $��Monats-Ansicht��6N   $��	0,10,10,0q�6g   6z   5)      5-      50      51      52      