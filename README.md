# <img src="res/logo.png" height="28">

![AppVeyor branch](https://img.shields.io/appveyor/build/Srul1k/Northwind/master?logo=appveyor) ![AppVeyor tests (branch)](https://img.shields.io/appveyor/tests/Srul1k/Northwind/master?logo=appveyor)

Northwind is a simple MVC application with CRUD operations for working with Products and Categories designed on a Clean Architecture.

The application was created using the framework ASP.NET Core 5.0. For the frontend part, Razor with Bootstrap are used. To work with databases, MS SQL is used in conjunction with EF Core.

## How to run ▶️
<details>
<summary><b>Instruction</b> <i>(Click to expand!)</i></summary>  

**The following developer tools are recommended for running the project:**

* **[IDE Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/)**  
* **[MS SQL Server Express 2019](https://www.microsoft.com/en-us/Download/details.aspx?id=101064)**  
* **[Microsoft SQL Server Managment Studio 18](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15 "")**
* **[Git Bash for Windows](https://gitforwindows.org/)**

### Steps for running project:

```diff
- ⚠️ If you have some problems with project, please write to me.
```

#### Installation:  

1. Download and install MS Visual Studio. When you install VS 2019 Community make sure you select **“ASP.NET and web development”** package.  
**[Please follow the link for more information](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019)**
2. Download and install MS SQL Server Express. **[Guide](https://www.sqlshack.com/how-to-install-sql-server-express-edition/)**
3. Download and install SQL Server Express LocalDB. **[Guide](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15)**
4. Download and install Git Bash for windows with default installation settings.  
5. Open Git Bash and set-up your name and e-mail by using next commands:

    ```bash
    git config --global user.name <your full name>  
    git config --global user.email <your email>
    ```  

    **[Please follow the link for more information](https://git-scm.com/book/en/v2/Getting-Started-First-Time-Git-Setup)**

6. Open Git console, push and select the path where the project will be located by using the command:

    ```bash
    cd /d <your full path>
    ```  

7. Clone this repository by using the command:

    ```bash
    git clone https://github.com/Srul1k/Northwind.git
    ```

#### Configuration:

1. Open the [query](res/northwind.sql) that you can find in the [res](res) folder using **Microsoft SQL Server Managment Studio 18**.
2. Enter in the server name: ```(localdb)\mssqllocaldb```
3. Execute the query.  
**[Please follow the link for more information](https://docs.microsoft.com/en-us/sql/ssms/quickstarts/ssms-connect-query-sql-server?view=sql-server-ver15)**  

4. Open the ```appsettings.Development.json``` file that is located on this path: ```src/Northwind.Web```
    * Configure AzureAD section if you want to log in with a Microsoft account *(optional)*.  
    **[Please follow the link for more information](https://docs.microsoft.com/en-us/azure/active-directory-domain-services/tutorial-create-instance)**  
    * Configure the EmailService section by entering an e-mail and password. Make sure that the permission to use third-party services is set in your mail service. Passwords for recovery will be sent to users via this mail *(optional, it is recommended to use Gmail)*.  
    **[Please follow the link for more information](https://support.google.com/accounts/answer/3466521?hl=en)**  
    * Configure the AdminInitializer section by entering an e-mail and password. This data will be used to log in as administrator.  

#### Launch:

1. Open the file ```Northwind.sln``` in the root directory using **MS Visual Studio**.
2. Select ```Northwind.Web``` as a start-up project.
3. Click on the run button *(CTRL + F5)*. Please note, the first running can be long.
4. Enjoy! :sparkles:

</details>

## Contact me :unicorn:

<a href="https://t.me/Srul1k">
    <img src="https://img.shields.io/badge/Telegram-2CA5E0?&logo=telegram&logoColor=white"></a>
<a href="https://discord.gg/2MXtSumMAb">
    <img src="https://img.shields.io/badge/Discord-%237289DA.svg?&logo=discord&logoColor=white"></a>  
<a href="mailto:srul1k@protonmail.com">
    <img src="https://img.shields.io/badge/ProtonMail-8B89CC?&logo=protonmail&logoColor=white"></a>  

If you have any good tips on the code or architecture of the application, please contact me and share it :purple_heart:
