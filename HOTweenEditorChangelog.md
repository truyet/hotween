## v1.1.280 ##
#### Bugfixes ####
  * Fixed issue where editor threw an error if visible and a SendMessage was set

## v1.1.275 ##
#### New Features ####
  * Fixed obsolete warnings being thrown in more recent Unity versions

## v1.1.270 ##
#### New Features ####
  * Added buttons to sort tweens in various modes

## v1.1.260 ##
#### New Features ####
  * Implemented "Restart Tweens By Id" option in OnComplete dropdown

## v1.1.250 ##
#### New Features ####
  * Added AnimationCurve field as ease option
#### Bugfixes ####
  * Fixed issue where editor couldn't find all attached Components inside a prefab

## v1.1.201 ##
#### Bugfixes ####
  * Fixed incompatibility issues with Windows 8

## v1.1.130 ##
#### Bugfixes ####
  * Fixed bug where Rects couldn't be targeted for tweening

## v1.1.120 ##
#### Bugfixes ####
  * Fixed bug where manager window generated errors and couldn't be drawn

## v1.1.110 ##
#### Bugfixes ####
  * Fixed bug with Unity 4, which in some cases could crash Unity or the editor panel

## v1.1.100 ##
#### Optimizations ####
  * Added fixes for chinese characters, optimizations, and new icons by Breno Azevedo
#### Bugfixes ####
  * Fixed a bug where the HOTweenComponent panel didn't work correctly when used with a prefab inside the project panel

## v1.1.010 ##
#### New Features ####
  * Added Autokill option (next to Autoplay button)

## v1.1.001 ##
### Upgrade instructions ###
**IMPORTANT** updating from a previous version requires that you:
  1. Import the new HOTweenEditor package as usual (a bunch of errors will appear in the console - just ignore them)
  1. Proceed to delete the following directories:
    * Holoville/Editor
    * Holoville/GUI
  1. Proceed to delete the following files:
    * Holoville/HOTween Extensions/HOTweenManager/Core/HOTweenPanelMode (pay attention not to delete the file with the same name in the.../HOTweenManager/Editor/Core directory)
#### New features ####
  * Inspector and window interface is much more neat and functional, and occupies less space

## v1.0.102 ##
#### Bugfixes ####
  * Fixed conflicts that could derive from having NGUI installed

## v1.0.100 ##
#### New Features ####
  * Added **OnComplete options** to the tweens

## v1.0.031 ##
#### Bugfixes ####
  * Fixed rare "cannot convert `Holoville.HOTween.Tweener' expression to type `Tweener'" error (in case other classes named "Tweener" - and not related to HOTween - were present in a project)

## v1.0.030 ##
#### New Features ####
  * Added eventual id to tween description (next to tween target) for better usability
  * Added duplicate tween option to HOTweenComponent
  * HOTweenComponent now stores a list of the Tweeners it generated (**generatedTweeners**) until it's destroyed
  * HOTweenManager component is now destroyed after initialization
  * Optimized destroy of HOTweenComponent
#### Changes ####
  * HOTweenComponent now creates tweens on Awake instead than on Start

## v1.0.020 ##
#### Changes ####
  * Now HOTweenManager creates the tweens **during Awake** instead than during Start, so if you use **HOTween.GetTweensById** during a Start method you will find them all

## v1.0.010 ##
#### New features ####
  * Implemented button that allows to duplicate full tweens and single property tweens (next to the delete - x - button).
  * Added full undo options.

## v1.0.002 ##
#### Bugfixes ####
  * Fixed "Cannot implicitly convert type `Holoville.HOTween.Tweener' to `Tweener'" error.

## v1.0.001 ##
Fully compatible with tweens created with previous versions of the Visual Editor.
#### New features ####
  * Implemented **HOTweenComponent**, a component which can be directly dragged on a GameObject to tween it or its Components.
  * Added Icons for HOTweenComponent and HOTweenManager.
  * Now HOTweenManager and HOTweenComponent are not shown as available targets to tween.
  * Various optimizations.