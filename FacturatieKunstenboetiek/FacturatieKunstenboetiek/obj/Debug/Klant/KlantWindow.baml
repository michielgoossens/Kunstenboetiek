   M S B A M L   `   `   `  ����S  OFacturatieKunstenboetiek, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:FacturatieKunstenboetiekFacturatieKunstenboetiek  5   	   *    $FacturatieKunstenboetiek.KlantWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:FacturatieKunstenboetiek     5   	   -   5          � 
ResizeMode$  NoResize=�5       � Title$ Kunstenboetiek - Klanten��5   	   $	��450��6*   $	��560��67    � Icon$ ..\Images\logo.ico��6s   .��5	      ]� .+�5
   
   o� -   5        $��menuHoofdmenu��5
      $	��Top=�6$   .@�5      m� -   5        $��menuItemNieuw��5      $��Nieuw��6,    m� Command$	 New��6;   m� 5      -   5        $��menuItemOpen��5      $
��Open��6+   69   m� 5      -   5        $��menuItemOpslaan��5      $��Opslaan��6.   $
 Save��6?   5      � 5   
   -   5         �Name$
 grid��5      $��25q�6    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5      -   5           Command$
 Save��5   !   6Q     5      -   5        $	 New��5   !   6P    � ColumnDefinitions 5      �� 5      $��90 �6#   �� 5      $	��110 �6#   �� 5      $��50 �6#   �� 5      $��90 �6#   �� 5      $	��160 �6#    � RowDefinitions 5      �� 5      �� 5      �� 5      �� 5       �� 5!      $
��1.3* �6    �� 5"      �� 5#      .+�5%      �� -	   5        $��labelKlantNr��5%      $��0��6(   $��0��68   .��6E   
Klant nr.:6Q   �� 5'      -
   5        $��textBlockKlantNr��5'      	 �� Padding$	 5q�60   $��1��6<   $��0��6L   $
��Left=�6Y   $	��Top=�6t   �� 5)      $��	Voornaam:��6>   $��0��6R   $��1��6b   
 �� Target
 6   ��  �� ElementName$ 
tbVoornaam��6b   �� 5*      -   5         ��Name$ 
tbVoornaam��5*   :      
Validation#��[  6   $��1��6N   $��1��6[   $��2��6k   $	��150��5,   T   ��5+      ��  �� UpdateSourceTrigger$ PropertyChanged=� �� Path$ Voornaam� �� ValidatesOnDataErrors .  �� NotifyOnValidationError . 5,   `   �� 5.      $��Familienaam:��6A   $��3��6X   $��1��6h   
 6   �� $ tbFamilienaam��6h   �� 5/      -   5        $ tbFamilienaam��5/   :   #��[  6   $��1��6Q   $��4��6^   $	��150��51   T   $��0,0,0,20q�6`   $��Stretch=�6r   ��50      �� $ PropertyChanged=�$ Familienaam� .  . 51   �   �� 53      $��Straat:��6<   $��0��6N   $��2��6^   
 6   �� $ tbStraat��6^   �� 54      -   5        $ tbStraat��54   :   #��[  6   $��2��6L   $��1��6Y   $��2��6i   $	��150��56   T   ��55      �� $ PropertyChanged=�$ Straat� .  . 56   T   �� 58      $��	Huis nr.:��6<   $��3��6P   $��2��6`   
 6   �� $ tbHuisNr��6`   �� 59      -   5        $ tbHuisNr��59   :   #��[  6   $��2��6L   $��4��6Y   $��80��5;   T   ��5:      �� $ PropertyChanged=�$ HuisNr� .  . 5;   T   �� 5=      $��	Postcode:��6>   $��0��6R   $��3��6b   
 6   �� $ 
tbPostcode��6b   �� 5>      -   5        $ 
tbPostcode��5>   :   #��[  6   $��3��6N   $��1��6[   $	��100��5@   T   ��5?      �� $ PropertyChanged=�$ Postcode� .  . 5@   T   �� 5B      $��	Gemeente:��6>   $��3��6R   $��3��6b   
 6   �� $ 
tbGemeente��6b   �� 5C      -   5        $ 
tbGemeente��5C   :   #��[  6   $��3��6N   $��4��6[   $	��150��5E   T   ��5D      �� $ PropertyChanged=�$ Gemeente� .  . 5E   T   �� 5G      $��Land:��6:   $��0��6J   $��4��6Z   
 6   �� $ tbLand��6Z   �� 5H      -   5         ��Name$ tbLand��5H   <   #��[  6   $��4��6L   $��1��6Y   $��2��6i   $	��150��5J   T    �� Text 5I      �� $ PropertyChanged=�$
 Land� .  . 5J   T   �� 5L      $��	Telefoon:��6>   $��0��6R   $��5��6b   
 6   �� $ 
tbTelefoon��6b   �� 5M      -   5        $ 
tbTelefoon��5M   ;   #��[  6   $��5��6O   $��1��6\   $	��100��5O   T   ��5N      �� $ PropertyChanged=�$ Telefoon� .  . 5O   T   �� 5Q      $��E-mail:��6;   $��2��6M   $��5��6]   
 6   �� $ tbEmail��6]   �� 5R      -   5        $ tbEmail��5R   :   #��[  6   $��5��6K   $��3��6X   $��2��6h   $	��240��5T   T   ��5S      �� $ PropertyChanged=�$ Email� .  . 5T   T   �� 5V      $��Btw nr.:��6;   $��0��6N   $��6��6^   
 6   �� $ tbBtwNr��6^   �� 5W      -   5        $ tbBtwNr��5W   ;   #��[  6   $��6��6L   $��1��6Y   $��4��6i   $	��400��5Y   T   ��5X      �� $ PropertyChanged=�$ BtwNr� .  . 5Y   T   5Z      5[      5\      