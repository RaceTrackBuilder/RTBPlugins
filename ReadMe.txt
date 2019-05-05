Firstly, Congratulations on having an inquisitive mind. Personally I thrive on having some activity to keep me busy and it's great to have others that like to "dabble" with new concepts and explore their ideas.

Secondly, if you have questions, this is the place to ask ... https://steamcommunity.com/app/388980/discussions/0/1846946102856075595/

This is my first Plugin project so I might be making a few mistakes, but so far it's working ok. FYI, the intial design was borrowed from here ... https://code.msdn.microsoft.com/windowsdesktop/Creating-a-simple-plugin-b6174b62

The RTBPluins Solution consists of:
- RTBPlugins - defines the Interface from which all RTB plugins MUST be built.
- YourHeightPlugin - example code on how to make a plugin to manipulate the height.
- YourImagePlugin - example code to show a texture can be created.

After the Plugin has been created, it must be copied to the Steam folder. In the YourHeightPlugin example project I have added an Post-Build evcnt to perform this copy to "C:\Program Files (x86)\Steam\steamapps\common\Race Track Builder\Plugins\Height\"; you may wish to alter this location if your Steam folder exists elsewhere.
