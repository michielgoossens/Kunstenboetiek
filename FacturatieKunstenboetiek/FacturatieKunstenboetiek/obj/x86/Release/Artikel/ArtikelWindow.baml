   M S B A M L   `   `   `  ���� S  OFacturatieKunstenboetiek, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:FacturatieKunstenboetiekFacturatieKunstenboetiek  ,    &FacturatieKunstenboetiek.ArtikelWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:FacturatieKunstenboetiek     -     � 
ResizeMode$  NoResize=� � Title$ Kunstenboetiek - Artikels��$	��400��$	��800�� � Icon$ ..\Images\logo.ico��.��]� .+�o� -   $��menuHoofdmenu��$	��Top=�.@�m� -   $��menuItemNieuw��$��Nieuw�� m� Command$	 New��m� -   $��menuItemOpen��$
��Open��m� -   $��menuItemOpslaan��$��Opslaan��$
 Save��� -    �Name$
 grid��$��25 25 12 25q� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$	 New��  -   $
 Save�� � ColumnDefinitions �� $��90 ��� $	��160 ���  � RowDefinitions �� �� �� �� �� .+��� -	   $��labelArtikelNr��$��0��$��0��.��Artikel nr.:�� -
   $��textBlockArtikelNr��	 �� Padding$	 5q�$��1��$��0��$
��Left=�$	��Top=��� $��Naam:��$��0��$��1��
 �� Target
 ��  �� ElementName$ tbNaam���� -    ��Name$ tbNaam��   
Validation#��[  $��1��$��1��$	��150������  �� UpdateSourceTrigger$ PropertyChanged=� �� Path$
 Naam� �� ValidatesOnDataErrors .  �� NotifyOnValidationError . �� $��Kleur:��$��0��$��2��
 �� $ tbKleur���� -   $ tbKleur��#��[  $��2��$��1��$	��150������ $ PropertyChanged=�$ Kleur� .  . �� $��Soort:��$��0��$��3��
 �� $ tbSoort���� -    ��Name$ tbSoort��#��[  $��3��$��1��$	��150�� �� Text �� $ PropertyChanged=�$ Soort� .  . �� $��Prijs excl BTW:��$��0��$��4��
 �� $ tbPrijs���� -   $ tbPrijs��#��[  $��4��$��1��$	��150������ $ PropertyChanged=�$ Prijs� .  .  �� StringFormat$ c�� �� ConverterCulture$ be-BE����  �� Orientation$ Vertical=�$��12 25 25 25q�$��2��$��5��.+��� $��80 0 0 0q�.��Afbeeldingen:�� $ 
Horizontal=�.+��� $
��0 25q�.+��� -   $��buttonAfbeeldingToevoegen��$��5q�$��5 5 5 0q�$��75��.��	Toevoegen�� -   $!��buttonAfbeeldingVerwijderen��$��5q�$��5q�$��75��.��Verwijderen�� -    ��Name$ listBoxAfbeeldingen��$	��140������ .R�]� ��. .+�� $	��Top=�$��30�� � ShowGridLines .  �� $	��150 ��� $	��250 �.+��� $��0��.��File�� $��1��.��Path�� ���� .R�� $	��0,2q� .  � ToolTip _� ����  �� 
TargetType" _�.R��� $	��150������ *AfbeeldingLink+ �� $	��145 ��� $	��250 �.+��� $��0������ *File+�� $��1��$��CharacterEllipsis=�$	 5 0 0 0q����� *AfbeeldingLink+�� $ 
Horizontal=�$��5q�.+��� $��Info:��$��0��$��4��$��80��
 �� $ tbInfo���� -   $ tbInfo��#��[   �� TextWrapping$
 Wrap=�$��4��$��1��$	��350��$��70������ $ PropertyChanged=�$
 Info� .  . $ c��$ be-BE��