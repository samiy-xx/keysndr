# KeySndr

KeySndr is a server software for receiving signals from client applications and transforming these into keyboard input to be used in creating macros for games or other apps.

### Client

* [KeySndr Android] client
* Windows Phone 8.1 client eventually
* Modern Browser on any device. ie. dont try with older IE versions

### Releases

* [Releases]

### Tech

KeySndr uses the following
* [Owin] For running the webserver
* [Nowin] Avoiding running the app as an admin by using this instead of the default httplistener provided by Owin
* [Jint] For executing javascript
* [Beacon] For server discovery

Plus many others

### Client info


By default, the webserver listens to port 45889 on your PC at all available interfaces

http://localhost:45889/manage/index.html for the admin interface

With the admin, you can create new configurations. Available are grid based configurations and new html view based configurations that allow more visual customisation.

Example view configuration coming soon.

### Typical url scheme

* For grid based configs: http://YOUR_PUBLIC_IP_IN_LOCAL_NETWORK:YOUR_PORT/play-grid.html?name=CONFIGURATION_NAME
* For view based configs: http://YOUR_PUBLIC_IP_IN_LOCAL_NETWORK:YOUR_PORT/Views/CONFIG_NAME/index.html

### Installation

The [Installer] will install the app to program files.

Installer and the application exe are not signed and will cause all sorts of warnings from UAC and other defender applications.

If this troubles you, feel free to clone the repo and build your own.



License
----
MIT


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [releases]: <https://github.com/samiy-xx/keysndr/releases>
   [Nowin]: <https://github.com/Bobris/Nowin>
   [DBreeze]: <https://github.com/hhblaze/DBreeze>
   [Owin]: <https://github.com/owin/owin>
   [Jint]: <https://github.com/sebastienros/jint>
   [Installer]: <https://github.com/samiy-xx/keysndr/releases/download/v0.8.0/keysndr_win_installer.exe>
   [KeySndr Android]: <https://github.com/samiy-xx/KeySndr.Clients> 
   [Beacon]: <https://github.com/rix0rrr/beacon>