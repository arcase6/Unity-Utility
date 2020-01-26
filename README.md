# Unity-Utility
Utility Scripts for Smoother Unity Development


This project exists as a personal toolkit to aid in future project development. 
The most work so far has been put into a set of databinding monobehaviors that can bind properties
in monobehavior scripts to other values using reflection.

The databinding classes that I created are

<hr/>
BindingSource:
There are two types of binding sources - 

one for monobehavior and ones derriving from scriptable object. The scriptable object ones
are similar to Scriptable Variables given at a Unite talk that I found helpful. The scriptable
objects in particular can be very useful for binding data to both a in-scene object and a UI display.

The monobehavior variant is a bit different from the scriptable object one in that it merely mirrors
some other piece of data (usually on some other component)

<hr/>
Drivers: 
These monobehaviors take a BindingSource monobehavior and use that value to set a target 
on a specified component (Either a custom script or a Unity script)

There are variants for most data types as well as few special ones that can take multiple sources
to produce the driver values
<hr/>
UIMediator -

This script is just a mediator between variables and UI controls. It can stand between legacy text as well as textmeshpro components.
It is used mainly so that I don't have to bind directly to something that is just a string. The mediator can use converter, formatter,
and validators to make sure that the text is set to a valid value in case something goes wrong.

THe typical pattern I use the UI Mediator is the following

-Create Mediator and link to target UI

- Create driver linking mediator to some binding source (HP or some stat)

- Create a second driver if necessary to drive the binding source back (this can be used on input- enabled controls
to get double binding)

<hr/>
Editor Scripts
There are a lot of editor scripts for this project. Most of the editor scripts are used to set binding sources and targets.
There are a few abstract and generic classes to somewhat reduce the code duplication, but I think I can improve it
a bit more perhaps. 

<hr/>
<b/>NOTE <br/>
There are also other scripts that take care of common tasks that are needed in most projects.
I plan to split up this github repository in the future in order to make the pieces more modular and maintainable.
Most of the scripts have been thoroughly tested through use, but I plan on adding unit tests at a later date.
<b/>
