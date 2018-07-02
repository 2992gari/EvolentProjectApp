# EvolentProjectApp
1 >Setup DataBase Project:
I have provided the sql scripts to create the database in Sql Server, you can use the same to create one.I have given Contacts as my database name. My database contains one tables for now, Contact.
In this project we’ll only be dealing with conatct table to perform CURD operations using Web API and Entity framework.

2 >Setup Data Access Layer:
. using Entity Framework  to talk to database.Use Generic Repository Pattern and Unit of work pattern to standardize our layer.
.Create a new class library in your visual studio, and name it DataModel.
. Generic Repository and Unit of Work
...Just to list down the benefits of Repository pattern,

It centralizes the data logic or Web service access logic.
It provides a substitution point for the unit tests.
It provides a flexible architecture that can be adapted as the overall design of the application evolves.

1>GenericRepository
 Add a folder named GenericRepository in DataModel project and to that folder add a class named Generic Repository.
 that servers as a template based generic code for all the entities that will interact with databse

 2>Unit of Work
 To give a heads up, again from my existing article, the important responsibilities of Unit of Work are,
 To manage transactions.
To order the database inserts, deletes, and updates.
To prevent duplicate updates. 
Inside a single usage of a Unit of Work object, different parts of the code may mark the same Invoice object as changed, but the Unit of Work class will only issue a single UPDATE command to the database. 
 Create a folder named DbOperation, add a class to that folder named DbOperation.cs
 
Setup Business Entities:
.BusinessEntities class library project.
use Business Entities as transfer objects to communicate between business logic and Web API project. 
So business entities may have different names but, their properties remains same as database entities. 
In our case we’ll add same name business entity classes appendint word "Entity" to them in our BusinessEntity project. So we’ll end up having one classes as follows,
 class ContactEntity

 
 Setup Business Services Project: 
 Add a new class library to the solution named BusinessServices. This layer will act as our business logic layer.
 Note that, we can make use of our API controllers to write business logic,
 but I am trying to segregate my business logic in an extra layer so that if in future I want to use any other application as my presentation layer then
I can easily integrate my Business logic layer in it.

Introduction to Unity:

Our Layers should not be that tightly coupled and should not be dependant to each other.
We’ll assign this role to a third party that will be called our container. 
Fortunately Unity provides that help to us, to get rid of this dependency problem and invert the control flow by injecting dependency not by creating objects by new but through constructors or properties.
...Bootstrapper class

Creating a Dependency Resolver with Unity and MEF:
new project named Resolver.
: Before we declare our Setup method, just add one more interface responsible for serving as a contract to register types.
  I name this interface as IRegisterComponent,
 :Now declare Setup method on our previously created IComponent interface, that takes instance of IRegisterComponent as a parameter,
:Now we’ll write a packager or you can say a wrapper over MEF and Unity to register types/ components. 
This is the core MEF implementation. Create a class named ComponentLoader
: Resolver wrapper is ready. Build the project and add its reference to DataModel, BusinessServices and WebApi project 


Setup WebAPI project:

Just add the reference of BusinessEntity and BusinessService in the WebAPI project(API.ContactManager).

Running the Application:


Before running the application, put some test data in our contact table.

Just hit F5, you get the same page as you got earlier, just append "/api/contact" in its url, and you’ll get the data,
eg:  Using  Advance Rest client

Get:
http://localhost:60086/api/Contact/ 

Post:
http://localhost:60086/api/Contact/

Body:{      
"FirstName": "Tommy",
"LastName": "Sey",
"Address": "UK 16",
"Email": "Tommy.Sey@gmail.com",
"PhoneNumber": "7799654599",
"Status": "Inactive"
}


Put:
http://localhost:60086/api/Contact/1

Body:{      
"FirstName": "Sam",
"LastName": "Sey",
"Address": "USA 16",
"Email": "Tommy.Sey@gmail.com",
"PhoneNumber": "7799654577",
"Status": "Inactive"
}


Delete:

http://localhost:60086/api/Contact/1


