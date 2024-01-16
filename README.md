# POWERTREE
 
This is a hierarchical tree control designed specifically for MAUI. It uses .NET 8 and currently runs on Windows and SQL Server only.

Features include:
*  A data-driven control that utilizes its own dbcontext and creates and manages its own set of tables to support Hierarchies/Folders/Items.
*  A sort of in-process microservice that can provide separate hierarchies for multiple subsystems in an application.
*  While Folders have common icons, the items can use different icons per item
*  Context menu's are attached to the folders and items, allowing full control
*  The consumer application creates a ViewModel that implements IPowerTreeViewModel
*  This consumer ViewModel provides full control over the actions that take place based on context menu and tap gesture selections.
*  Drag and Drop capabilty allowing sorting of Folders and items
*  Consuming application handles their own entities, but shares these entityId's with the TreeView control
*  Included Sample application PowerTree.Sample demonstrates a bookmark type of component implementation.

This is based on Luis Beltran's https://dev.to/icebeam7/creating-a-treeview-control-in-net-maui-49mp work for Matt Goldman's https://twitter.com/mattgoldman initiative https://goforgoldman.com/posts/maui-ui-july/

For full details, see my blog at  (https://hopdev.hashnode.dev/)

This Tree Control is in need of some improvements, some enhancements, and ultimately a NuGet Package.
If anyone is interested in contributing please contact me at hopdev@outlook.com.

Future enhancements:
* Support for MacOS, iOS, and Chromebook
* Support for SQLite and Oracle
* Icons to be integrated into project and modified/enhanced
* Light Mode and Dark Mode color support
* Creating a NuGet package for general developer consumption
* Implementation of a "Node Only" mode, where the consumer, such as a Document Management System would be responsible for listing all the items; for scenarios where large numbers of items exist
* Modify Implementation to support XAML syntax for PowerTreeView control and perhaps Folder and Item Templates
* Variety of bug fixes and optimization
* Build in support for those users who only want to use the UI component of the control and not the full microservice
<img width="185" alt="PowerTreeFolderMenu" src="https://github.com/str37/PowerTree/assets/44349896/a73d0c8c-b806-4ba8-8a71-231b0dc8df63">
<img width="198" alt="PowerTreeItemMenu" src="https://github.com/str37/PowerTree/assets/44349896/1f652860-d9bd-4272-9f36-72950bcfa1d3">
 
