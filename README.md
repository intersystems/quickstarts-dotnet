# quickstarts-dotnet
This code shows how to connect a .NET application to an InterSystems server using your choice of ADO.NET, XEP, Native, Entity Framework, and multi-model access.. 
It is required for the [.NET QuickStart](https://learning.intersystems.com/course/view.php?name=.NET%20QS). 

## Contents

* `adoNETplaystocks.cs` to see how to store and retrieve data relationally
* `xepplaystocks.cs` to see how to quickly store objects
* `nativeplaystocks.cs` to see how to run methods within InterSystems IRIS
* `multiplay.cs` to see multi-model access using ADO.NET, XEP, and Native API
* `Program.cs` to see how to use Entity Framework, a third-party tool to work with objects

## Configuration files

* `config.txt`: contains connection details for **ADO.NET**, **XEP**, **Native API** and **multi-model**.
* `App.config`: located in EFPlay folder, contains connections details, parameters and initial settings for **Entity Framework**. 

## How to Run

1.  Verify you have an [<span class="urlformat">instance of InterSystems IRIS</span>](https://learning.intersystems.com/course/view.php?name=Get%20InterSystems%20IRIS), and an IDE that supports .NET (such as **Microsoft Visual Studio**). 

2.  Load the sample stock data into InterSystems IRIS:  
    `$ iris load http://github.com/intersystems/Samples-Stock-Data`  

3. Clone this repo and open it in your IDE.

4. Get the required drivers from the installation directory of your InterSystems instance and put them in the packages folder of this cloned repo, following [the instructions in documentation](https://docs.intersystems.com/components/csp/docbook/DocBook.UI.Page.cls?KEY=PAGE_extconnex#PAGE_extconnex_dotnet).
* For Entity Framework, also follow these instructions to get the Entity Framework pacakges in the first video of [Using Entity Framework with InterSystems IRIS](https://learning.intersystems.com/course/view.php?name=Entity%20Framework).

5. With Microsoft Visual Studio:

    * Select **File** → **Open** → **Project/Solution**. Choose the `quickstarts-dotnet.sln`. 
    * Select **View** → **Solution Explorer** to view project structure.
    * Right click on Solution. Select **Add** → **Existing item**. Choose `config.txt`.
    * Open `config.txt` file and modify the `IP` and `password` to be the correct values for your InterSystems IRIS instance. `Port` and `username` are most likely the defaults but you can verify those as well.

6. Due to its complexity, **Entity Framework** requires a few more steps to set up the IDE and configuration file. If you want to try using Entity Framework, please follow our instructions in section 6 - "Use Entity Framework, an example of a third-party API, to store objects to InterSystems IRIS", of [.NET QuickStart](https://learning.intersystems.com/course/view.php?name=.NET%20QS).

You should now have several classes for **ADO.NET**, **XEP**, **Native API**, **multi-model** and **Entity Framework**. 

Detailed instructions are included on the [.NET QuickStart page](https://learning.intersystems.com/course/view.php?name=.NET%20QS).
