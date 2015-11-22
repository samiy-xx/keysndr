# KeySndr

KeySndr is a server software for receiving signals from client applications and transforming these into keyboard input to be used in creating macros for games or other apps.

### Releases

* [Releases]

### Tech

KeySndr uses the following
* [Owin] (For running the webserver)
* [Nowin] (Avoiding running the app as an admin by using this instead of the default httplistener provided by Owin)
* [DBreeze] (for optional key/value storage)
* [Jint] For executing javascript

Plus many others

### Clients

* Any modern browser
* Android client at google play

By default, the webserver listens to port 45889 on your PC at all available interfaces

http://localhost:45889/manage/index.html for the admin interface

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
