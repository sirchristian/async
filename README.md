Asynk
-----

Moved [Codeplex Asynk project]("https://asynk.codeplex.com/") to bitbucket. Note: this has suffered from inattention. There are still good ideas here so the project's not dead yet. 

Asynk is a framework/application that allows existing applications to easily be migrated to an asynchronous message based pattern by decorating existing calls with attributes. 

Asynk is developed using C#.

Example
-------
Asynk allows you to take an existing application with a function call like:
    public class MyLibrary
    {
         public void DoWork()
         {
              /* This method does work that takes a long time
                 and the application appears to freeze */
         }
    }
    
    static void Main()
    {
         MyLibrary lib = new MyLibrary();
         lib.DoWork(); 
    }

And change it into:
    public class MyLibrary
    {
         [AsynkCallable]
         public void DoWork()
         {
              /* This method does work that takes a long time
                 and the application appears to freeze */
         }
    }

    static void Main()
    {
         MyLibrary lib = new MyLibrary();
         Asynker.Process(lib, "DoWork");
    }
