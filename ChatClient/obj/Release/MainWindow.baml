   M S B A M L   `   `   `  ���� E  AChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:ChatClient.PropertiesChatClient.Properties  'clr-namespace:ChatClient
ChatClient      ChatClient.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4resx#clr-namespace:ChatClient.Properties     *localclr-namespace:ChatClient     -     �Name$  Window�� � Title$ 
ChatClient��$	��350��$	��525�� � Icon$ icon.ico�� � 	Resources    DownloadStatusTrigger �� Triggers �� Binding �� Path �� RelativeSource �� AncestorType   ChatClient.DownloadButton	 �� Value
 �� Property �� Value  DownloadTracker �� 
TargetType   ChatClient.DownloadButton   DownloadStarted   DownloadEnded  TopExpanderTemplate �� 
TargetType � RowDefinitions �Name �� CornerRadius � ColumnDefinitions � OverridesDefaultStyle d� 	IsChecked �� Mode �� Triggers P� Property P� Value �� 
TargetName �� ElementName  RightExpanderTemplate��%�  &	        &	 �     &	     &	 D    ��  ��  �� $ IsDownloading� �� "  	 True.a��� 
 � ��$ Downloading...����  �� $ IsDownloading� �� "  		 False.a��� 
 � ��$ 
Downloaded���� "  �;�  OnDownloadStarted;�  OnDownloadEnded�� " 9�.R��  �� $
��Auto ��� $ 
ContentRow��$��0 �.+��� $��Border��$��0��$��0,0,0,1q�
����  �$ 0����Z� ._��  �� $��20 ��� $��* �.+�d�  .  �� $ 
IsExpanded�$ TwoWay=� �� *TemplatedParent+���� " d�.R��� $��Border��$ 0��
������ $��0q�._�F� $��Arrow��
����  �$��Center=�$��Center=�"����        � 	=  	= � z     	 P�  �@J�IsMouseOver . . ��� $ Border��
 � ��
 ��  ��� $ Arrow��
 � ��
 �����P�  �@d�	IsChecked$
 trueY�. ��� $ Arrow��
 � ��" ��     	= � 	=     � z  	= 	�� $��1��$��4q�$��Header����. $��Center=��� $��Content��$��1��._��� $��4q� P�  �@9�
IsExpanded . . ��� $ 
ContentRow�� �� $ Content��$ DesiredHeight�
 � ���� " 9�.R��  �� $ ContentColumn��$��0 ��� $
��Auto �.+��� $��Border��$��1��$��1,0,0,0q�
����  �$ 0����Z� ._��  �� $��20 ��� $
��Auto �.+�d�  . $��20�� �� $ 
IsExpanded�$ TwoWay=� �� *TemplatedParent+���� " d�.R��� $��Border��$ 0��
������ $��0q�._�F� $��Arrow��
����  �$��Center=�$��Center=�"���� 	=     �     	= � 	=  z 	 P�  �@J�IsMouseOver . . ��� $ Border��
 � ��
 ��  ��� $ Arrow��
 � ��
 �����P�  �@d�	IsChecked$
 trueY�. ��� $ Arrow��
 � ��" ��        � 	=  	= �     z 	�� $��1��$��4q�$��Header����. $��Center=��� $��Content��$��0��._��� $��4q� P�  �@9�
IsExpanded . . ��� $ ContentColumn�� �� $ Content��$ DesiredWidth�
 � ��.��]� .+�9� $��Connection Setting��$	��Top=�$��Center=�#��[ .���  �� $
��Auto ��� $	��250 ��� $	��100 � �� $��* ��� $��* �.+��� $��Adress:��$��0��$��0��$��18(�$��Times New Roman)��� $��	UserName:��$��1��$��0��$��18(�$��Times New Roman)��� -    ��Name$ 	AdressBox��$��0��$��1��$��0q�$��18(�$��Times New Roman)�
���   � �� 
IsEditable . �� -    ��Name$ UserNameBox��$��1��$��1��$��0,0,0,1q�$��Bottom=�$��18(�$��Times New Roman)��� -    ��Name$ Connect_btn��$��Connect��$��0��$��3��$��Times New Roman)�$��16(�$
��Bold"����	#FFF1F58E$��0q��� -   $ Disconnect_btn��$��
Dissconect��$��1��$��3��$��Times New Roman)�$��16(�$
��Bold"����	#FFF1F58E$��0q��� $��Bottom=�.+��� $��0q�$)��#©Created by Yevhenii Serdiuk, 2016��$��Calibri)�$��9(�$��Right=�
����  �� $��2�� �� $
��Auto ��� $��6* ��� $
��Auto ��� $��* � �� $
��Auto ��� $
��Auto �.+��� -     ��Name$  UserNameLabel��$��0��$��0��
��� � �$��Times New Roman)�$��18(��� -   $ MessageField��$��0��$��1��$��Times New Roman)�$��18(�$��Center=�$��0q��� -   $ UploadButton��
����  �$��0��$��2��$��30��$��0q���Z� ��Z� ��F� $��Center=�$��Center=����� $ UploadButton��$ 
Foreground���� ! � FillRule$! Nonzero=�.���� " �� Rect$" 5,0,5,20�# �� RadiusX$# 3Q�$ �� RadiusY$$ 3Q��� $" 5,0,5,12�$# 3Q�$$ 3Q�% � 	Transform% �� & �� CenterX$& 8Q�' �� CenterY$' 1Q�( �� Angle$( 40Q��� $" 7,0,5,12�$# 3Q�$$ 3Q�% �� $& 8Q�$' 4Q�$	( -40Q����� " �� P�  �@J�IsMouseOver . . ��� 
 � ��
 ��  ��� -	   $ SendMessagebtn��$��0��$��3��$
��Send��$��Times New Roman)�$��18(�$
��Bold"����	#FFF1F58E$��0q��� -
   ) ��Name$) DataHandler��$��1��$��5��$y�	Collapsed=�$
��Auto��$��20��* J� 	AllowDrop* . + � ContextMenu+ �� .@�m� -   $��Remove��� $��3��
����  � �� $
��Auto ��� $��* �.+��� -   $  StatusLabel��$��Ready��
��� ���$��0��$��Arial)�$��10(�$
��Left=�$
��Bold"�#��[  �� -   $  ConnectionLabel��$��Disconnected��
��� ���$��1��$��Arial)�$��Right=�$
��Bold"�$��10(�9� , 9� ExpandDirection$
, Left=�$��Right=�$	��Top=�$��Right=�- � ToolTip$- UserList��#��[ .���� -   $) 	UsersList��+ �� .@�m� $��PrivateMessage���� -   . ��Name$. Scroll��$
��Left=�$��Hidden=�$��Disabled=�.���� -   / ��Name$/ 
MessageBox��$
��Wrap=�$��Times New Roman)�$��16(�
���**����Z� 