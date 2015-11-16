;NSIS Modern User Interface
;Basic Example Script
;Written by Joost Verburg
 
  !define MUI_PRODUCT "KeySndrServer"


;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General
  RequestExecutionLevel admin
  
  !define MUI_ICON "app.ico"
  ;Name and file
  Name "KeySndr"
  OutFile "keysndr_win_installer.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\KeySndr"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "Software\Blockz3D\KeySndr" ""

  
  SetOverWrite try
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
!macro VerifyUserIsAdmin
UserInfo::GetAccountType
pop $0
${If} $0 != "admin" ;Require admin rights on NT4+
        messageBox mb_iconstop "Administrator rights required!"
        setErrorLevel 740 ;ERROR_ELEVATION_REQUIRED
        quit
${EndIf}
!macroend
 
function .onInit
	setShellVarContext all
	!insertmacro VerifyUserIsAdmin
functionEnd
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

  Function UninstallPrevious

    ; Check for uninstaller.
    ReadRegStr $R0 HKLM "Software\Blockz3D\KeySndr" "InstallDir"

    ${If} $R0 == ""        
        Goto Done
    ${EndIf}

    DetailPrint "Removing previous installation."    

    ; Run the uninstaller silently.
    ExecWait '"$R0\Uninstall.exe /S"'

    Done:

FunctionEnd
;--------------------------------
;Installer Sections
Section "Uninstall Previous version" SecUninstallPrevious

    Call UninstallPrevious

SectionEnd

Section "Server" SecDummy

  SetOutPath "$INSTDIR"
  
  ;ADD YOUR OWN FILES HERE...
  File "..\KeySndr.Win\bin\release\Jint.dll"
  File "..\KeySndr.Win\bin\release\KeySndr.Base.dll"
  File "..\KeySndr.Win\bin\release\KeySndr.Common.dll"
  File "..\KeySndr.Win\bin\release\KeySndr.InputManager.dll"
  File "..\KeySndr.Win\bin\release\KeySndr.Win.exe"
  File "..\KeySndr.Win\bin\release\KeySndr.Win.exe.config"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.dll"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.xml"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.FileSystems.dll"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.FileSystems.xml"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.Hosting.dll"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.Hosting.xml"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.StaticFiles.dll"
  File "..\KeySndr.Win\bin\release\Microsoft.Owin.StaticFiles.xml"
  File "..\KeySndr.Win\bin\release\Newtonsoft.Json.dll"
  File "..\KeySndr.Win\bin\release\Newtonsoft.Json.xml"
  File "..\KeySndr.Win\bin\release\Owin.dll"
  File "..\KeySndr.Win\bin\release\System.Net.Http.Formatting.dll"
  File "..\KeySndr.Win\bin\release\System.Net.Http.Formatting.xml"
  File "..\KeySndr.Win\bin\release\System.Web.Cors.dll"
    File "..\KeySndr.Win\bin\release\System.Web.Http.Cors.dll"
	  File "..\KeySndr.Win\bin\release\System.Web.Http.Cors.xml"
  File "..\KeySndr.Win\bin\release\System.Web.Http.dll"
  File "..\KeySndr.Win\bin\release\System.Web.Http.Owin.dll"
  File "..\KeySndr.Win\bin\release\System.Web.Http.Owin.xml"
  File "..\KeySndr.Win\bin\release\System.Web.Http.xml"
  File /r "..\KeySndr.Win\bin\release\Portal\*.*"
  
  
  CreateShortCut "$DESKTOP\${MUI_PRODUCT}.lnk" "$INSTDIR\${MUI_PRODUCT}.exe" ""
  ;create start-menu items
  CreateDirectory "$SMPROGRAMS\${MUI_PRODUCT}"
  CreateShortCut "$SMPROGRAMS\${MUI_PRODUCT}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${MUI_PRODUCT}\${MUI_PRODUCT}.lnk" "$INSTDIR\${MUI_PRODUCT}.exe" "" "$INSTDIR\${MUI_PRODUCT}.exe" 0
  ;Store installation folder
  WriteRegStr HKLM "Software\Blockz3D\KeySndr" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecDummy ${LANG_ENGLISH} "A test section."

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...
  Delete "$INSTDIR\*"

  RMDir "$INSTDIR"
  Delete "$DESKTOP\${MUI_PRODUCT}.lnk"
  Delete "$SMPROGRAMS\${MUI_PRODUCT}\*.*"
  RmDir  "$SMPROGRAMS\${MUI_PRODUCT}"
  DeleteRegKey /ifempty HKLM "Software\Blockz3D\KeySndr"

SectionEnd