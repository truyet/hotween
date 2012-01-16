// 
// OverwriteManager.cs
//  
// Author: Daniele Giardini
// 
// Copyright (c) 2012 Daniele Giardini - Holoville - http://www.holoville.com
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins.Core;

namespace Holoville.HOTween.Core
{
	/// <summary>
	/// Manager used to store targets and properties being tweened,
	/// and to manage eventual overwrites.
	/// TODO HOTOverwriteManager is not used for now, because it would conflict with Sequence. I'm keeping it for later, eventually.
	/// </summary>
	static internal class OverwriteManager
	{
		// VARS ///////////////////////////////////////////////////
		
		static	private		Dictionary<object,List<HOTweenData>>		dcTargetToDatas;
		
		
		// ===================================================================================
		// METHODS ---------------------------------------------------------------------------
		
		/// <summary>
		/// Manages adding a new target and property to tween to HOTween,
		/// managing eventual overwrite and clear of tween plugins that control the same property
		/// (for a plugin to be overwritten, it must be of the same type of the one that's already tweening the property).
		/// Called by <see cref="ABSTweenPlugin"/> when a plugin is started.
		/// Returns <c>true</c> if the property didn't overwrite anything else, otherwise <c>false</c>.
		/// </summary>
		/// <returns>
		/// A value of <c>true</c> if the property didn't overwrite anything else, otherwise <c>false</c>.
		/// </returns>
		static internal bool AddTween( Tweener p_tweenObj, ABSTweenPlugin p_plugin, object p_target, string p_propName )
		{
			if ( dcTargetToDatas == null )						dcTargetToDatas = new Dictionary<object, List<HOTweenData>>();
			if ( !dcTargetToDatas.ContainsKey( p_target ) ) {
				dcTargetToDatas.Add( p_target, new List<HOTweenData>() );
				dcTargetToDatas[p_target].Add( new HOTweenData( p_tweenObj, p_plugin, p_propName ) );
				return true;
			}
			
			bool overwritten = false;
			List<HOTweenData> datas = dcTargetToDatas[p_target];
			HOTweenData data;
			for ( int i = datas.Count - 1; i > -1; --i ) {
				data = datas[i];
				if ( data.propName == p_propName && data.plugin.GetType() == p_plugin.GetType() ) {
					// Overwrite.
					overwritten = true;
					// data.tweenObj.KillPlugin( data.plugin );
					dcTargetToDatas[p_target].RemoveAt( i );
					break;
				}
			}
			
			dcTargetToDatas[p_target].Add( new HOTweenData( p_tweenObj, p_plugin, p_propName ) );
			return overwritten;
		}
		
		/// <summary>
		/// Purges the given <see cref="Tweener"/> from the overwrite list.
		/// Called by HOTween each time a tween is removed.
		/// </summary>
		static internal void RemoveTween( Tweener p_tweenObj, object p_target )
		{
			if ( dcTargetToDatas == null )		return;
			
			if ( p_target.Equals( null ) ) {
				// TODO Target has become null. Go through all lists and remove null targets.
				
				return;
			}
			
			List<HOTweenData> datas = dcTargetToDatas[p_target];
			for ( int i = datas.Count - 1; i > -1; --i ) {
				if ( datas[i].tweenObj == p_tweenObj )		datas.RemoveAt( i );
			}
		}
		
		// ===================================================================================
		// PRIVATE METHODS -------------------------------------------------------------------
		
		static private void PurgeNullTargets()
		{
			// TODO (since the target is the key, how can I use a null key?)
		}
		
		
		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// ||| INTERNAL CLASSES ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		
		private class HOTweenData
		{
			// VARS ///////////////////////////////////////////////////
			
			public		Tweener				tweenObj;
			public		ABSTweenPlugin		plugin;
			public		string				propName;
			
			
			// ***********************************************************************************
			// CONSTRUCTOR
			// ***********************************************************************************
			
			public HOTweenData( Tweener p_tweenObj, ABSTweenPlugin p_plugin, string p_propName )
			{
				tweenObj = p_tweenObj;
				plugin = p_plugin;
				propName = p_propName;
			}
		}
	}
}

