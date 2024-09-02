<div align="center">
  <br>
  <a href="https://twitter.com/intent/follow?screen_name=ProcessusT" title="Follow"><img src="https://img.shields.io/twitter/follow/ProcessusT?label=ProcessusT&style=social"></a>
  <br>
  <h1>
    The RemClip project
  </h1>
  <br><br>
  forked from <a href="https://gist.github.com/emoacht/c75bab17a4584c77cf64">https://gist.github.com/emoacht/c75bab17a4584c77cf64</a>
</div> <br><br>
> RemClip is a C# project which permits to steal user clipboard data and send it to a remote web server under attacker control
<br />
<br>
<div align="center">
<img src="https://raw.githubusercontent.com/ProcessusT/RemClip/main/.assets/background.PNG" width="80%;">
</div>
<br>
<br />
<br />

## Installation
<br>
- Just put the "web server" folder content in your own web server folder with PDO and Sqlite installed<br>
<br>
- In the "svchost - source code" folder, which is the stealer source code, modify the "server_ip" in Main function to your own web server IP address<br>
- Then compile it with Visual Studio Code with the following command line :<br><br>


```python
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true
```

- Move the new compiled binary from svchost\bin\Release\net7.0\win-x64\publish to the python dropper folder<br>
- Remotely upload your compiled stealer to your target computer with the dropper script (need admin privs on target)<br>
- The binary will be started on next user logon<br>
