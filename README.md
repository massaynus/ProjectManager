# ProjectManager

The project Manager currently has four projects

## 1. Data Access Library
The data access library has all the models and the context for the DB connection
They were created using the **Code First** approach, and then tweaked for some added convinience

## 2. Local server API (RESTful)
>This API is built using a WebAPI project with the MVC pattern

This is the brains of the operation, it has a controller for each model
It can manage access to controllers or actions via Role or ordinary basic Auth
It will only send you back the data that concerns the account you are currently using

_If you are logged in as Josh and you've asked to see the payments you will only get the payments that were sent or recieved by Josh_

Same goes for tasks, projects, issues... etc

## 3. Windows Client
>The windows Client is built using the WPF platform and the MVVM pattern

It uses JSON to talk to the Local Server API to get/send and display the data
The windows client will attempt to call the **AuthController** to verify the identity of the user and his role
Then it will Open one of three main views depending on his role :

1. ManagersMainView 		(Manager)
2. TeamLeadersMainView 		(Team Leader)
3. TasksMainView		(Member)

The client Role isn't supported in this client

## 4. Database project
This is just a project that makes table scripts from the existing DB
It only exists as a backup and **isn't updated frequently**
The maintained DB creation script is **MSSQLServer_DB.sql** in the root folder


