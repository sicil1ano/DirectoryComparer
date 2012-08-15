# Directory Comparer

It represents my first work with WPF. So, if you find something that it could be improved, that's because I have to learn more about .NET.
After all, Rome wasn't built in a day. :P

## Description

This tool has been written in C# 4.0 and developed using the [WPF](http://msdn.microsoft.com/en-us/library/ms754130.aspx)(Windows Presentation Foundation) technology, available in the Microsoft .NET Framework.

It's a simple tool to compare directories. It allows the user to select different filters and displays changes whether different files are present.
Also, it displays useful details, about the number of files and subdirectories found.
It consists of two [DataGrids](http://msdn.microsoft.com/en-us/library/system.windows.controls.datagrid.aspx), displaying the details of each file found in the directories selected. 
The DataGrids are displaying the name, the creation and edit date of the file. 
To improve the feedback of the UI, when the user is using the DataGrids' controls, the virtualization of the columns is marked enabled.

Actually, the program lets the user to do a recursive search,through a checkbox, and then comparing also the contents in the subdirectories.
It can display, through the GUI, the changes found, changing the color of the rows of the datagrids.
To improve the performance and show the contents of the directories in the right way, I used some BackGroundWorkers, different threads to execute the scans, and the Dispatcher, used to show the changes in the UI.

## Correct execution of the application
To use this tool properly, even if I'm using a multi-threading architecture to improve the performance, it's strictly required to select only directories without a huge number of files and subdirectories. 
For instance, don't select the directory "C:\Windows\System32" :-)

## Running the application
To run the application, just download the package and open it on Visual Studio. Then compile the program to get the executable.

## Tools required to develop

 - Visual C# 2010 Express or Visual Studio.

## Contributing
With "contributing", I mean anything that could help to improve the application and solve bugs/issues.
You can suggest a new feature for the application or just raise an issue.

If you want to write some code:
 - Fork the repository.
 - Make the changes to the codebase.
 - Send a pull request once you're sure everything is working.
 




