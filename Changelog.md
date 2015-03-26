## v1.3.380 ##
#### Bugfixes ####
  * HOTween won't consider boolean values as valid animateable objects anymore
  * Added more safe checks on some operations

## v1.3.360 ##
#### Changes ####
  * HOTween.Kill(target) will now also kill all Sequences that have at least one nested tween with the given target
#### Bugfixes ####
  * Fixed empty Sequences causing errors with some operations and in HOTween Inspector

## v1.3.355 ##
#### New Features ####
  * Added Tweener.GetPathLength method
#### Bugfixes ####
  * WaitForCompletion and WaitForRewind now break also if the tween is destroyed

## v1.3.327 ##
#### Bugfixes ####
  * Fixed Color tweens being broken in a previous update

## v1.3.326 ##
#### Bugfixes ####
  * Fixed Vector2/4 tweens being broken in previous version

## v1.3.320 ##
#### New Features ####
  * Added Is2D Method Parameter to PlugVector3Path
#### Bugfixes ####
  * Fixed empty Sequences callback bug
  * Fixes for PlugVector3Path in case of Z axis lock
  * If a callback of a Sequence kills its own Sequence, it is correctly managed

## v1.3.100 ##
#### Bugfixes ####
  * RestartIncremental now works correctly with Quaternions and Rewinds
  * Fixed a bug caused by Unity, where TimeScaleIndependentUpdate tweens created in the first frame didn't work correctly with delays

## v1.3.080 ##
#### New Features ####
  * TweenVar now supports AnimationCurves for ease
  * Alpha version of RestartIncremental (still buggy)

## v1.3.060 ##
#### New Features ####
  * Now allows automatic tweening of uint values

## v1.3.055 ##
#### New Features ####
  * No GC is allocated when tweening position or rotation of a Transform

## v1.3.035 ##
#### New Features ####
  * SpeedBased tweens now allow ease
#### Bugfixes ####
  * Fixed ResetAndChangeParms not working correctly when OverwriteManager is enabled
  * Fixed bug where playing an initially paused and delayed FROM tween, didn't immediately set the target position to its FROM value
  * Fixed bug with IsTweening and Sequences
## v1.3.000 ##
#### Changes ####
  * HOTween and HOTweenMicro DLLs now have the same name (so serialization with the Visual Editor doesn't encounter errors), and Unity's Inspector will tell you which one you're using when you select the DLL in the Project Window
#### New Features ####
  * Made the runtime Inspector more readable

## v1.2.031 ##
#### Bugfixes ####
  * Fixed bug where GetAllTweens, GetAllPausedTweens and GetAllPlayingTweens methods threw an error when no tween existed

## v1.2.030 ##
#### New Features ####
  * Added overloads to HOTween.To/From that accept easeType and delay parameters inline
  * Added GetAllTweens, GetAllPausedTweens and GetAllPlayingTweens static HOTween methods

## v1.2.010 ##
#### New Features ####
  * Added PixelPerfect Method parameter
#### Bugfixes ####
  * Removed Andrew Kingdom's mod since it broke constant speed on curved paths

## v1.1.900 ##
#### Changes ####
  * Since Google Code doesn't allow file uploads anymore, the most recent downloads will only be available on HOTween's website
#### Bugfixes ####
  * Fixed bug where using PartialPath the tween would stop at the first waypoint

## v1.1.890 ##
#### Bugfixes ####
  * Fixed flickers with first loop when using FixedUpdate
  * OnComplete's TweenEvent now still reports the correct tween target instead than null
  * Fixed relative zero-duration tweens not working correctly

## v1.1.882 ##
#### New Features ####
  * Added PlugColor32 (automatically used in case of Color32 values)
  * Added optional forcePlay parameter to all Reverse methods
  * OverwriteManager now ignores paused tweens
#### Bugfixes ####
  * Fixed bug where GoToAndPlay didn't play if the tween was at the same position where it was sent via the goto

## v1.1.860 ##
#### New Features ####
  * Various memory allocation optimizations
  * Implemented Andrew Kingdom's mod (OnPluginUpdated callback, waypointIndex exposed in PlugVector3Path)
  * Added string id and int id overload to IsTweening static method
  * Added KeepDisabled/KeepEnabled method parameters
#### Changes ####
  * OnPlay is now called also when starting
  * OnPause is now called also when tween is complete or rewinded

## v1.1.780 ##
#### Bugfixes ####
  * Fixed bug where OnComplete callback wasn't called when forcing a tween to complete

## v1.1.761 ##
#### New features ####
  * Added WaitForRewind coroutine
  * Added SendMessage overload to OnStepComplete
  * Added option to EnableOverwriteManager that disables overwrite warnings
  * Added Clear method to Sequences, so they can be reused
  * Error/warning lines are now reported correctly by Unity
  * Added alpha version of HOTween.Punch/Shake
#### Bugfixes ####
  * Fixed bug where mySequence.isEmpty always returned true

## v1.1.724 ##
#### Bugfixes ####
  * Fixed bug in linear path movement

## v1.1.722 ##
#### New features ####
  * **Added straight/linear path movement**, to complement movement on curved paths. There's now an optional **PathType** parameter when calling a PlugVector3Path method, which allows to set the path as linear (while default is curved).
  * Optimized all path logics, so that tweening an object's localPosition while moving its parent will re-adapt the path correctly.
#### Bugfixes ####
  * Fixed bug when calling **GetPointOnPath** from a Tweener.

## v1.1.600 ##
#### New features ####
  * Unity AnimationCurves can now be assigned to easing (instead than the regular ease curves)

## v1.1.500 ##
#### New Features ####
  * External plugins can now be created and used for tweens, by inheriting from Holoville.HOTween.Plugins.Core.ABSTweenPlugin

## v1.1.430 ##
#### New Features ####
  * Added **AppendCallback** and **InsertCallback** methods to Sequences

## v1.1.420 ##
#### New Features ####
  * Added HOTween.GetTweensByIntId method
  * OnPluginOverwritten callback can now be used with ApplyCallback method
#### Bugfixes ####
  * Fixed bug when killing a tween during a callback different from OnComplete

## v1.1.402 ##
#### New Features ####
  * Added **OnPluginOverwritten callback** to Tweeners
#### Optimizations ####
  * Now tweens are updated in growing order, which leads to better behaviour of various things, and correct overwriting when 2 overwriting tweens start at the same exact time
  * Added more low-level optimizations based on Dmitriy's advice
#### Bugfixes ####
  * Fixed tweens not being correctly killed and removed from OverwriteManager when using HOTween.Kill method

## v1.1.371 ##
#### Optimizations ####
  * Additional low-level optimizations by Dmitriy Yukhanov
#### Bugfixes ####
  * 0 durations tweens are now working also when inside Sequences
  * Fixed bug with Complete method not working correctly when used on Quaternion properties with a Vector3 endValue

## v1.1.360 ##
#### New features ####
  * Added overload to OnComplete and ApplyCallbacks, which implements a **SendMessage** behaviour
  * Low-level optimizations by Dmitriy Yukhanov

## v1.1.340 ##
#### New features ####
  * Now the overshoot of Back eases and the amplitude and period of Elastic eases can be modified when choosing the Ease TweenParm.

## v1.1.335 ##
#### New features ####
  * Greatly optimized partial paths
  * **HOTween Inspector:** now "completed but not killed" tweens are displayed as a separate group (while previously they appeared inside the running tweens)
#### Bugfixes ####
  * Fixed floating point imprecision bug when calculating total elapsed loops
  * Fixed **path lockRotationAxis** bug
  * Fixed bug where **incremental loops** would behave correctly only after two cycles, when set to infinite

## v1.1.330 ##
#### New features ####
  * Added **HOTween.GetTweenersByTarget** method, that returns all the Tweeners (not Sequences, since Sequences have no target) whose target is the given one.

## v1.1.320 ##
#### New features ####
  * Added **ResetAndChangeParms** method to Tweeners, which allows to change a Tweener (while keeping its target) without creating a new one

## v1.1.311 ##
#### Bugfixes ####
  * Fixed **OverwriteManager throwing error** when more than one tween was overwritten at the same time

## v1.1.310 ##
#### New features ####
  * Added **ApplyCallback** method to IHOTweenComponent and ABSTweenComponent

## v1.1.301 ##
#### New features ####
  * Added **HOTween.GetTweensById** static method
  * Added tween ID to HOTween Inspector debug information
  * Implemented **lockRotationAxis** parameter for **PlugVector3Path OrientToPath** method-parameter
  * Added **LockPosition** method-parameter to **PlugVector3Path**
#### Bugfixes ####
  * OnStart callbacks are now called in the correct order even when inside nested sequences

## v1.1.202 ##
#### Bugfixes ####
  * Fixed bug introduced in v1.1.201, where animating a Transform property not included in the new speed tweens wasn't working

## v1.1.201 ##
#### New features ####
  * Tweens that concern Transforms (position/localPosition/rotation/localRotation/localScale) are now faster on all platforms, and WAY faster on iOS/Micro.

## v1.1.102 ##
#### New features ####
  * Added partial path methods (myTweener.UsePartialPath, myTweener.ResetPath).
  * Tweening on a path is slightly faster than before.

## v1.1.007 ##
#### New features ####
  * Implemented **HOTweenInspector**. Now, when in the Editor, clicking on HOTween's instance will show a series of debug informations in Unity's Inspector.

## v1.1.006 ##
#### Bugfixes ####
  * Fixed PlugVector3Path bug (determining if starting position was identical to first waypoint could give incorrect results, due to floating point imprecision).

## v1.1.005 ##
#### Bugfixes ####
  * Fixed bug where PlugQuaternion wasn't using "shortest" available rotation.
  * **HOTweenMicro only:** fixed HOTweenMicro not being compiled with correct symbols (MICRO compilation symbol was missing - bug introduced when moving from MonoDevelop to Visual Studio), and thus not working on iOS with max stripping level.

## v1.1.004 ##
#### Bugfixes ####
  * Fixed 0 duration tweens not completing correctly (and thus not dispatching OnComplete event).

## v1.1.003 ##
#### Bugfixes ####
  * Fixed PlugRect bug (y value was animated incorrectly).

## v1.1.002 ##
#### Bugfixes ####
  * Fixed OverwriteManager bug (identical plugins inside the same Sequence were sometimes overwritten if calling GoTo).

## v1.1.001 ##
#### New features ####
  * Tweens can now have 0 duration (meaning they will be completed immediately, as soon as they start).

## v0.9.008 ##
#### Bugfixes ####
  * "From" tweens now jump to starting position immediately, even if delayed.

## v0.9.007 ##
#### New features ####
  * Sequences can now be created without parameters.

## v0.9.006 ##
#### Bugfixes ####
  * Fixed bug which could sometimes happen when getting values on iOS (mainly a monotouch bug, but it's fixed now).

## v0.9.005 ##
#### Bugfixes ####
  * Fixed PlugInt bug introduced in previous release.

## v0.9.004 ##
#### New features ####
  * Added Init parameter which allows to activate the OverwriteManager, which will otherwise stay completely deactivated (and will ignore any HOTween.EnableOverwriteManager calls).
#### Optimizations ####
  * Various code optimizations and performance improvements by stfx.
#### Bugfixes ####
  * Fixed bug in **PlugVector3Path**, where going to the end of a non-closed path would set the eventual transform orientation incorrectly.

## v0.9.003 ##
#### Bugfixes ####
  * Fixed bug in HOTween.PlayBackwards/PlayForward methods.

## v0.9.002 ##
#### Optimizations ####
  * Various optimizations (mainly to PlugVector3Path system) thanks to **stfx**.

#### Bugfixes ####
  * Fixed bug with OverwriteManager erroneously overwriting tweens in case Restart was called.

## v0.9.001 ##
#### New features ####
  * Implemented **OverwriteManager** (you have to call `HOTween.EnableOverwriteManager()` to enable it).
  * Added the **isSequenced** property to Tweeners and Sequences, which returns TRUE if a Tweener/Sequence is part of another Sequence.

## v0.8.142 ##
#### New features ####
  * Added **OnRewinded** callback.

#### Bugfixes ####
  * Fixed bug where **OnUpdate** wasn't called when calling **Rewind**.

## v0.8.141 ##
#### New features ####
  * Added **renameInstanceToCountTweens** parameter to HOTween.Init() (it prevents HOTween to rename its gameObject in order to show currently running tweens, even while in the Editor).
  * Added **PlayForward** and **PlayBackwards** methods to HOTween, IHOTweenComponent, ABSTweenComponent, Tweener and Sequence.

## v0.8.140 ##
#### New features ####
  * Added permanent HOTween mode, via **HOTween.Init(true)**. This will set HOTween not to be destroyed when all tweens are killed, thus reducing memory allocation and garbage collection.
  * Now HOTween gameObject is renamed to show total number of tweens only while inside the Editor.

## v0.8.132 ##
#### Bugfixes ####
  * Fixed PlugVector3Path OrientToPath bug introduced by Tweener.GetPointOnPath.

## v0.8.131 ##
#### Bugfixes ####
  * Fixed Sequence Append/Prepend not working correctly with speed-based tweens.

## v0.8.130 ##
#### New features ####
  * Added **WaitForCompletion** coroutine to Tweeners and Sequences. Can be used inside a coroutine to pause it until the tween is complete. Example:
    * yield return StartCoroutine( myTweenComponent.WaitForCompletion() );

## v0.8.125 ##
#### Bugfixes ####
  * Fixed bug with **LoopType.Incremental** adding an additional increment when the loop completed.

## v0.8.124 ##
#### Bugfixes ####
  * Should have definitely fixed iOS error when tweening ints without directly using PlugInt.

## v0.8.123 ##
#### New features ####
  * Added **GetPointOnPath** method to **Tweener**.

#### Bugfixes ####
  * Possibly fixed iOS error when tweening ints without directly using PlugInt (though I still didn't have a feedback of its verification).

## v0.8.121 ##
#### New features ####
  * Added **enabled** property for Tweeners and Sequences (and IHOTweenComponent and ABSTweenComponent).
  * Added **TweenVar**, a class you can instantiate to manually control a float value as if it was being tweened.

#### Bugfixes ####
  * **OnUpdate** callbacks now work even for nested Tweeners and Sequences.

## v0.8.112 ##
#### Bugfixes ####
  * Removed leftover Debug.Log message from **Sequence**.

## v0.8.111 ##
#### Bugfixes ####
  * Fixed bug where appending an interval at the start of a Sequence didn't work.

## v0.8.110 ##
#### New Features ####
  * Added **Incremental** LoopType also to **Sequences**. Please note that this is an experimental option: it works perfectly with simple Sequences, but you should check that your animation works as expected with more complex ones.

## v0.8.102 ##
#### Bugfixes ####
  * Fixed a NullReferenceException which could happen when killing a tween during an update.

## v0.8.101 ##
#### New Features ####
  * Added **Incremental** LoopType (as of now works only with Tweeners and not with Sequences). It creates a loop that, instead than rewinding or restarting at each cycle, increments the values each time (like 0 to 50, 50 to 100, 100 to 150, and so on). It works with all ease types and plugin types (even PlugVector3Path and strings).

## v0.8.019 ##
#### New Features ####
  * Added `IntId()` method parameter to TweenParms and SequenceParms. It works like `Id()`, but wants an int instead of a string, which makes it faster when you perform grouped ID operations, like `HOTween.Pause( myId )`.
  * Added `intId` property to IHOTweenComponent and ABSTweenComponent.

## v0.8.017 ##
#### Bugfixes ####
  * Removed leftover Debug.Log message from **PlugVector3Path**.

## v0.8.016 ##
#### New Features ####
  * Added **PlugRect** plugin (thanks to Romain Giraud), which is now used by default with Rect properties.
  * Added lookAhead option in **PlugVector3Path** **OrientToPath** parameter.

## v0.8.015 ##
#### New Features ####
  * Added **HOTween.From**.
  * Now **PlugString** checks for null values and replaces them with "".

## v0.8.011 ##
#### New Features ####
More parameters can now be changed while playing, including:
  * **IHOTweenComponent**[_Tweener or Sequence_].timeScale, .loops, .loopType.
  * **Tweener**.easeType.

## v0.8.007 ##
#### Changes ####
  * Sequences are now automatically paused at startup (you will need to call mySequence.Play() to start them).
  * **ConstantSpeed** parameter removed from **PlugVector3Path**: now paths use constant speed by default.
#### Optimizations ####
  * Optimized drawing of path gizmos.

## v0.8.006 ##
#### New Features ####
  * Added **ConstantSpeed** parameter to **PlugVector3Path**, which animates with constant speed along the path.
  * Added **SpeedBased** parameter to **TweenParms**, to allow speed-based instead than time-based tweens (when using this parameter, duration is interpreted as speedxseconds).
  * String properties can now be animated using defaults (without the need to add PlugString).

## v0.8.005 ##
#### Bugfixes ####
  * Fixed iOS compatibility.

## v0.8.004 ##
#### Bugfixes ####
  * Corrected bug where default loopType wasn't changed when changing HOTween.defLoopType value.

## v0.8.002 ##
#### Bugfixes ####
  * Fixed PlugVector3Path OrientToPath orienting on incorrect axis.
  * Fixed PlugVector3Path OrientToPath flipping orientation on certain conditions issue.

## v0.8.001 ##
First Google Code release
#### Changes ####
  * HOTween main namespace is now Holoville.HOTween instead than Holoville.Tweening.

## v0.7.301 ##
#### Bugfixes ####
  * Fixed delay bug (if delay was longer than duration, the tween would not start).

## v0.7.300 ##
#### Changes ####
  * **Warning!** EaseType enum values now start with an uppercase letter (due to the fact that EaseType is now a true enum, and relies on another class to store the ease functions). This means for example EaseType.EaseOutQuad instead than EaseType.easeOutQuad.
#### Bugfixes ####
  * Changing HOTween's timeScale doesn't affect eventual tween delays anymore.

## v0.7.202 ##
First release.