   M S B A M L   `   `   `  ���� S  OFacturatieKunstenboetiek, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:FacturatieKunstenboetiekFacturatieKunstenboetiek  *    $FacturatieKunstenboetiek.KlantWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:FacturatieKunstenboetiek     -     � 
ResizeMode$  NoResize=� � Title$ Kunstenboetiek - Klanten��$	��450��$	��560�� � Icon$ ..\Images\logo.ico��.��]� .+�o� -   $��menuHoofdmenu��$	��Top=�.@�m� -   $��menuItemNieuw��$��Nieuw�� m� Command$	 New��m� -   $��menuItemOpen��$
��Open��m� -   $��menuItemOpslaan��$��Opslaan��$
 Save��� -    �Name$
 grid��$��25q� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$
 Save��  -   $	 New�� � ColumnDefinitions �� $��90 ��� $	��110 ��� $��50 ��� $��90 ��� $	��160 � � RowDefinitions �� �� �� �� �� $
��1.3* ��� �� .+��� -	   $��labelKlantNr��$��0��$��0��.��
Klant nr.:�� -
   $��textBlockKlantNr��	 �� Padding$	 5q�$��1��$��0��$
��Left=�$	��Top=��� $��	Voornaam:��$��0��$��1��
 �� Target
 ��  �� ElementName$ 
tbVoornaam���� -    ��Name$ 
tbVoornaam��   
Validation#��[  $��1��$��1��$��2��$	��150������  �� UpdateSourceTrigger$ PropertyChanged=� �� Path$ Voornaam� �� ValidatesOnDataErrors .  �� NotifyOnValidationError . �� $��Familienaam:��$��3��$��1��
 �� $ tbFamilienaam���� -   $ tbFamilienaam��#��[  $��1��$��4��$	��150��$��0,0,0,20q�$��Stretch=����� $ PropertyChanged=�$ Familienaam� .  . �� $��Straat:��$��0��$��2��
 �� $ tbStraat���� -   $ tbStraat��#��[  $��2��$��1��$��2��$	��150������ $ PropertyChanged=�$ Straat� .  . �� $��	Huis nr.:��$��3��$��2��
 �� $ tbHuisNr���� -   $ tbHuisNr��#��[  $��2��$��4��$��80������ $ PropertyChanged=�$ HuisNr� .  . �� $��	Postcode:��$��0��$��3��
 �� $ 
tbPostcode���� -   $ 
tbPostcode��#��[  $��3��$��1��$	��100������ $ PropertyChanged=�$ Postcode� .  . �� $��	Gemeente:��$��3��$��3��
 �� $ 
tbGemeente���� -   $ 
tbGemeente��#��[  $��3��$��4��$	��150������ $ PropertyChanged=�$ Gemeente� .  . �� $��Land:��$��0��$��4��
 �� $ tbLand���� -    ��Name$ tbLand��#��[  $��4��$��1��$��2��$	��150�� �� Text �� $ PropertyChanged=�$
 Land� .  . �� $��	Telefoon:��$��0��$��5��
 �� $ 
tbTelefoon���� -   $ 
tbTelefoon��#��[  $��5��$��1��$	��100������ $ PropertyChanged=�$ Telefoon� .  . �� $��E-mail:��$��2��$��5��
 �� $ tbEmail���� -   $ tbEmail��#��[  $��5��$��3��$��2��$	��240������ $ PropertyChanged=�$ Email� .  . �� $��Btw nr.:��$��0��$��6��
 �� $ tbBtwNr���� -   $ tbBtwNr��#��[  $��6��$��1��$��4��$	��400������ $ PropertyChanged=�$ BtwNr� .  . 