# Qlik Sense Cache Initializer 
Date:        Aug 2016

Version:     0.11

Author:      Joe Bickley, Roland Vecera

Summary:     This tool will "warm" the cache of a Qlik Sense server so that when using large apps the users get good performance right away.  You can use it to load all apps, a single app, and you can get it to just open the app to RAM or cycle through all the objects so that it will pre calculate expressions so users get rapid performance. You can also pass in selections too.

Credits:     Thanks to Øystein Kolsrud for helping with the Qlik Sense .net SDK steps.   Uses the commandline.codeplex.com for processing parameters

Usage:       cacheinitiazer.exe -s https://server.domain.com [-a appname] [-p virtualproxyprefix]

Notes:       This projects use the Qlik Sense .net SDK, you must use the right version of the SDK to match the server you are connecting too. To swap version   simply replace the .net SDK files in the BIN directory of this project, if you dont match them, it wont work.
