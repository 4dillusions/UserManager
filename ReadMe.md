<h1 align="center">User Manager (senior C#/.NET interview test)  </br></br>

<img src="https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white"> 
<img src="https://img.shields.io/badge/-.NET%2010.0-blueviolet">

<a href="https://github.com/4dillusions/UserManager/actions/workflows/dotnet-desktop.yml">
  <img src="https://github.com/4dillusions/UserManager/actions/workflows/dotnet-desktop.yml/badge.svg" alt=".NET Desktop CI">
</a>
</h1>

<p align="center">
  <img src="Doc/Doxygen/Res/userManager.jpg">
</p>

Create a WPF application what can do
------------------------------------
-create three views for user management <br/>
-user data: UserId, LoginName, Password, Surname, FirstName, BirthDate, BirthPlace, AddressCity <br/>
-store data in file <br/>
1. Login view: user/password (check it in file) <br/>
2. User list view: users in grid, filter options: city combo box, search box with word searching in all data, "edit" button for 3rd view <br/>
3. User detail view: user list item in new window, edit all data except UserId, data validation, save/cancel buttons, back to 2nd view after save data to file (refresh user list) <br/>
-bonus: XML export from grid (or JSON export if the file stored in XML) <br/>
&ensp;	what is json: https://en.wikipedia.org/wiki/JSON <br/>

Clone
-----
Clone the entire project including the submodules:<br>
```bash
git clone --recurse-submodules https://github.com/4dillusions/UserManager.git
```

If the project is already cloned and you forgot to fetch the submodules:<br>
```bash
git submodule update --init --recursive
```

If the submodules have been updated and you want to fetch the latest changes:<br>
```bash
git submodule update --remote --merge
```

Architecture
------------
<p align="center">
  <img src="Doc/architecture.svg">
</p>

Helper libraries
----------------
Json.NET: http://www.newtonsoft.com/json
