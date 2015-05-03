; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Launchpad"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "Jarl Gullberg"
#define MyAppURL "https://github.com/Nihlus/Launchpad/"
#define MyAppExeName "Launchpad.exe"

;
 ; Fill this out with the path to your built launchpad binaries.
;
#define LaunchpadReleaseDir "C:\Users\Jarl\Desktop\launchpad-winpack"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B9676EA5-104E-4401-AF3F-EEDE45E3DA95}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=C:\Users\Jarl\Desktop
OutputBaseFilename=launchpad-setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
UninstallDisplayIcon={app}\{#MyAppExeName}

[UninstallDelete]
Type: filesandordirs; Name: "{app}"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: "{#LaunchpadReleaseDir}\Launchpad.exe"; DestDir: "{app}";
Source: "{#LaunchpadReleaseDir}\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs
; Extra libraries - GTK# must be included.
Source: "{#LaunchpadReleaseDir}\gtk-sharp-2.12.26.msi"; DestDir: "{tmp}";

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\gtk-sharp-2.12.26.msi"" /qn"
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
