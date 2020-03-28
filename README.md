![windows build](https://github.com/bmenees/MegaBuild/workflows/windows%20build/badge.svg)

# MegaBuild

MegaBuild is an automated build utility. It is primarily intended to build multiple Visual Studio solutions (versions 2002 – 2019), but it also includes steps to run batch files, run PowerShell scripts, build MSBuild projects, send email, add output, wait a specified amount of time, or play sounds. You can also write your own custom step types that integrate into MegaBuild using the provided MegaBuild SDK.

MegaBuild was written in C# and requires the .NET Framework or .NET Core. To add custom step types you can use any .NET language to inherit from the MegaBuild.ExecutableStep type.

This software is CharityWare. If you use it, I ask that you donate something to the charity of your choice.

![MegaBuild](http://www.menees.com/Images/MegaBuild.png)

## Installation
Make sure you have the required version of the .NET Framework or .NET Core installed.  Then unzip the MegaBuild release to a directory of your choice.  Once the files are unzipped into a directory, then MegaBuild.exe should be ready to run.

To uninstall MegaBuild, delete the directory you unzipped it to.

## Command Line Switches
MegaBuild supports command line switches to open a project, execute a build, and exit the process after building.
```
Usage:		MegaBuild [/build [/exit]] [project]

/build	Builds the specified project.  Optional.  If this is specified,
        then a project must be specified as well.
/exit	Exits the program after building.  The process exit code is 0 for a successful build
        or 1 for a failed build.  This switch can only be used if /build is used.
project	The name of the MegaBuild project to open.
```
Any other command line arguments (e.g. /?) will cause a help message to display about the valid command line switches.

_Note_:  To launch MegaBuild minimized from the command line use the OS’s start command with the /min switch.  For example:
```
C:\>start /min MegaBuild /build /exit Test.mgb
```
## Output Commands
MegaBuild supports parsing the output for MegaBuild-specific commands.  Currently, only the “Megabuild.Set” command is supported.  Its syntax and semantics are similar to the Windows SET command except “MegaBuild.Set” lets you set a MegaBuild application variable instead of an environment variable.  This makes the variable available to MegaBuild so it can pass the value into any spawned process if you put it on that process’s command-line.

```
Usage:		MegaBuild.Set VariableName=VariableValue

Example:	MegaBuild.Set ZipFileOutput=C:\Dev\WidgetMgr\Release.zip
```

Using “MegaBuild.Set” allows you to set a MegaBuild application variable from any batch file, console application, or MegaBuild OutputStep.  Then later steps can use this variable to affect their output.  If an individual command step only sets a Windows environment variable using SET, then that environment variable won’t be available to later steps because the command process won’t let you set an environment variable in the parent process or sibling processes.  MegaBuild application variables can be used to work around that limitation.

_Note_: You shouldn’t enclose the VariableName in ‘%’ characters if you’re executing in a batch file.  If you do, then the Windows batch file processor will try to expand the name using the Windows environment variables, and if it doesn’t find a match, it will replace it with an empty string.

## MegaBuild SDK
The MegaBuildSdk.dll assembly contains several types that can be used create custom build step types for MegaBuild.  When MegaBuild is run, it uses .NET reflection to load and scan all of the assemblies in the directory where the MegaBuild.exe resides.  Any assembly that has the MegaBuildExtension attribute applied to it will be scanned for types that inherit from MegaBuild.Step.  These custom types will then be available to the Add, Insert, and Edit Step dialogs, and the steps can be used in a build just as if they were built-in step types.

The SampleStep project provides an example of a custom step type and an editor control for that step type.  The SampleStep example assumes you’re using C#, but other .NET languages will also work.  Since the full source code is provided for MegaBuild, the standard steps like VSStep, CommandStep, and EmailStep are good samples to look at too.

## Frequently Asked Questions
1.	When I execute an EmailStep with a blank SMTP server (i.e. using the SMTP service running on the same machine as MegaBuild), it says the step succeeded, but no email ever shows up.  What’s the deal?
 > The local SMTP service returns success when a message is queued up, not when the message is successfully delivered.  If the SMTP service can’t locate a destination address using DNS, then it will log errors to the System event log.  You may need to have your network administrator setup the correct names/routes before the SMTP service can use your local DNS server to resolve destination addresses.  MegaBuild uses the email classes in .NET’s System.Net.Mail namespace, so you can read about them on http://msdn.microsoft.com for more information.

2.	When I use a SoundStep configured to play a system sound, why don’t I hear any output?
> You won’t hear output for system sounds if you’ve selected the “No Sounds” scheme or if you have no sound selected for the given event in Windows’s Sounds control panel.

3.	Why should I use MegaBuild instead of MSBuild?
> MegaBuild is a GUI tool for executing a single sequence of build steps inside of a try..catch (i.e., if a build step fails it will run the failure steps).  Its emphasis is on ease of use for handling a multi-step build.  In contrast, MSBuild is a non-GUI tool that requires you to learn an XML declarative language to use it.  It’s more flexible than MegaBuild, but it has a much steeper learning curve.  MegaBuild was released two years before MSBuild, but it now supports an MSBuild step type in case you need to integrate MSBuild projects into your MegaBuild build processes.
