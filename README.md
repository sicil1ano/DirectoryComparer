Directory Comparer - Version 0.1

It represents my first work with WPF. So, if you find something that it could be improved, that's because I have to learn more.
After all, Rome wasn't built in a day. :P

Description:

This tool has been written in C# 4.0 and developed using the WPF (Windows Presentation Foundation) technology, available in the Microsoft .NET Framework.

It's a simple tool to compare directories. It allows the user to select different filters and displays changes whether different files are present.
Also, it displays useful details, about the number of files and subdirectories found.
It consists of two DataGrids, displaying the details of each file found in the directories selected. 
The DataGrids are displaying the name, the creation and edit date of the file. 
To improve the feedback of the UI, when the user is using the DataGrids' controls, the virtualization of the columns is marked enabled.

Actually, the program lets the user to do a recursive search,through a checkbox, and then comparing also the contents in the subdirectories.
It can display, through the GUI, the changes found, changing the color of the rows of the datagrids.
To improve the performance and show the contents of the directories in the right way, I used some BackGround Workers, different threads to execute the scans, and the Dispatcher, used to show the changes in the UI.

Remember: To use this tool properly, even if I'm using a multi-threading architecture to improve the performance, it's strictly required to select only directories without a huge number of files and subdirectories. 
For instance, don't select the directory "C:\Windows\System32" :-)



