# NppDB
   NppDB is a Notepad++ Plugin for supporting that connect to different type databases, execute sql statements and show the result.

## Prerequisites
   * Notepad++ 6.5 (unicode)
   * .Net Framework 4.0

## Currently Supported Databases
   MS SQL Server (test only 2008 R2), SQLite

## Structure
![structure image](https://raw.githubusercontent.com/gutkyu/NppDB/gh-pages/images/NppDB_All_n.png)
   1. Database Connect Manager
      * register, remove, connect and close a Database Server 
      * represent the database's elements in hierarchy sturcture.
      * make a environment for executing a sql statement
   2. SQL Result
      * show a result of sql query.
   3. Document
      * place to writing a sql statement.
      * a block of sql statement must be selected before 'Execute SQL'
  
## Install
   1. download a [zip file containing binaries](https://github.com/gutkyu/NppDB/releases/download/v2.0/NppDBv2.0.zip) and extracts into temporary directory
   2. copy NppDB.dll and NppDB directory under notepad++ plugins directory

      ![plugins directory](https://raw.githubusercontent.com/gutkyu/NppDB/gh-pages/images/NppDB_Plugin_Dir.png)

## Quick Start Guide
   1. open 'Database Connect Manaer'.
   2. register a database-connect in 'Database Connect Manager'.
   3. expand sub nodes that are databases by double click database-connect node which appear as root node.
   4. perform a right-click on node indicating target database and select 'Open' from the node's popup menu to create new document and sql-result.
   5. write a sql statement in the document and select blocks of the statement.
   6. perform 'Plugins/NppDB/Execute SQL' from notepade++ toolbar (or F9 shortcut key)

## Usage
### Open Database Connect Manager
   select 'NppDB/Database Connect Manager' from Notepad++ plugin menu
   or
   click icon ![Database Connect Manager Icon](https://raw.githubusercontent.com/gutkyu/NppDB/gh-pages/images/DBPPManage16.png) from a toolbar 

### Register new database server
   1. click icon ![Regiser Icon](https://raw.githubusercontent.com/gutkyu/NppDB/master/NppDB.Core/Resources/add16.png) from  Database-Connect-Manager's toolbar
   2. select one of database types
   3. Database Connect Node is registered in Database Connect Manager by pass authentication which selected database module produce.

	![Select Database Type](https://raw.githubusercontent.com/gutkyu/NppDB/gh-pages/images/NppDB_Sel_DBType.png)
   4. connect to database server
   	* [MS SQL Server](https://github.com/gutkyu/NppDB.MSSQL) 
      
### Getting into detail about sub elements
   perform double-click on the node to expands sub elements.
   because all of connect database manager's nodes are represented in hierarchy, can also use this way for other sub elements 

   two method to make a environment to execute sql statements
   * first, select 'Open' from database node's popup

   * second, select prepared sql statements as 'Select … Top 100' or 'Select … Limit 100' from table node's popup
	
	![SQL Linked Database Node](https://raw.githubusercontent.com/gutkyu/NppDB/gh-pages/images/NppDB_Node_SQL.png)

### Executing sql statement
   1. check that current document can execute sql statement. (ok if with sql-result )
   2. write a sql statement and then select a block of the statement.

	![Select Blocks](https://raw.githubusercontent.com/gutkyu/NppDB/gh-pages/images/NppDB_SQL_Block.png)

   3. perform menu 'Execute SQL (F9 shortcut key)' to display result of the sql statement.
	
## License
MIT
