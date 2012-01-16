// 
// PlugVector3Path.cs
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

using UnityEngine;
using System;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins.Core;

namespace Holoville.HOTween.Plugins
{
	/// <summary>
	/// Plugin for the tweening of Vector3 objects along a Vector3 path.
	/// </summary>
	public class PlugVector3Path : ABSTweenPlugin
	{
		// ENUMS //////////////////////////////////////////////////
		
		private		enum OrientType 
		{
			None,
			ToPath,
			LookAtTransform,
			LookAtPosition
		}
		
		// VARS ///////////////////////////////////////////////////
		
		static internal	Type[]			validPropTypes = { typeof(Vector3) };
		static internal	Type[]			validValueTypes = { typeof(Vector3[]) };
		
		internal	Path				path; // Internal so that HOTween OnDrawGizmo can find it and draw the paths.
		internal	float				pathPerc = 0; // Stores the current percentage of the path, so that HOTween's OnDrawGizmo can show its velocity.
		
		private		Vector3				typedStartVal;
		private		Vector3[]			points;
		private		bool				isClosedPath = false;
		private		OrientType			orientType = OrientType.None;
		private		Vector3				orientation; // Used to get correct axis for orientation.
		private		float				lookAheadVal = 0.0001f;
		
		// REFERENCES /////////////////////////////////////////////
		
		private		Vector3				lookPos;
		private		Transform			lookTrans;
		private		Transform			orientTrans;
		
		// GETS/SETS //////////////////////////////////////////////
		
		/// <summary>
		/// Gets the untyped start value,
		/// sets both the untyped and the typed start value.
		/// </summary>
		override protected	object		startVal {
			get { return _startVal; }
			set { _startVal = typedStartVal = (Vector3) value; }
		}
		
		/// <summary>
		/// Gets the untyped end value,
		/// sets both the untyped and the typed end value.
		/// </summary>
		override protected	object		endVal {
			get { return _endVal; }
			set {
				_endVal = value;
				Vector3[] ps = (Vector3[])value;
				points = new Vector3[ps.Length];
				Array.Copy( ps, points, ps.Length );
			}
		}
		
		
		// ***********************************************************************************
		// CONSTRUCTOR
		// ***********************************************************************************
		
		/// <summary>
		/// Creates a new instance of this plugin using the main ease type and an absolute path.
		/// </summary>
		/// <param name="p_path">
		/// The <see cref="Vector3"/> path to tween through.
		/// </param>
		public PlugVector3Path( Vector3[] p_path ) : base( p_path, false ) {}
		/// <summary>
		/// Creates a new instance of this plugin using an absolute path.
		/// </summary>
		/// <param name="p_path">
		/// The <see cref="Vector3"/> path to tween through.
		/// </param>
		/// <param name="p_easeType">
		/// The <see cref="EaseType"/> to use.
		/// </param>
		public PlugVector3Path( Vector3[] p_path, EaseType p_easeType ) : base( p_path, p_easeType, false ) {}
		/// <summary>
		/// Creates a new instance of this plugin using the main ease type.
		/// </summary>
		/// <param name="p_path">
		/// The <see cref="Vector3"/> path to tween through.
		/// </param>
		/// <param name="p_isRelative">
		/// If <c>true</c>, the path is considered relative to the starting value of the property, instead than absolute.
		/// </param>
		public PlugVector3Path( Vector3[] p_path, bool p_isRelative ) : base( p_path, p_isRelative ) {}
		/// <summary>
		/// Creates a new instance of this plugin.
		/// </summary>
		/// <param name="p_path">
		/// The <see cref="Vector3"/> path to tween through.
		/// </param>
		/// <param name="p_easeType">
		/// The <see cref="EaseType"/> to use.
		/// </param>
		/// <param name="p_isRelative">
		/// If <c>true</c>, the path is considered relative to the starting value of the property, instead than absolute.
		/// </param>
		public PlugVector3Path( Vector3[] p_path, EaseType p_easeType, bool p_isRelative ) : base( p_path, p_easeType, p_isRelative ) {}
		
		// ===================================================================================
		// PARAMETERS ------------------------------------------------------------------------
		
		/// <summary>
		/// Parameter > Smoothly closes the path, so that it can be used for cycling loops.
		/// </summary>
		/// <returns>
		/// A <see cref="PlugVector3Path"/>
		/// </returns>
		public PlugVector3Path ClosePath() { return ClosePath( true ); }
		/// <summary>
		/// Parameter > Choose whether to smoothly close the path, so that it can be used for cycling loops.
		/// </summary>
		/// <param name="p_close">
		/// Set to <c>true</c> to close the path.
		/// </param>
		public PlugVector3Path ClosePath( bool p_close )
		{
			isClosedPath = p_close;
			return this;
		}
		
		/// <summary>
		/// Parameter > If the tween target is a <see cref="Transform"/>, orients the tween target to the path.
		/// </summary>
		/// <returns>
		/// A <see cref="PlugVector3Path"/>
		/// </returns>
		public PlugVector3Path OrientToPath() { return OrientToPath( true ); }
		/// <summary>
		/// Parameter > Choose whether to orient the tween target to the path (only if it's a <see cref="Transform"/>).
		/// </summary>
		/// <param name="p_orient">
		/// Set to <c>true</c> to orient the tween target to the path.
		/// </param>
		public PlugVector3Path OrientToPath( bool p_orient )
		{
			if ( p_orient )		orientType = OrientType.ToPath;
			return this;
		}
		
		/// <summary>
		/// Parameter > If the tween target is a <see cref="Transform"/>, sets the tween so that the target will always look at the given transform.
		/// </summary>
		/// <param name="p_transform">
		/// The <see cref="Transform"/> to look at.
		/// </param>
		public PlugVector3Path LookAt( Transform p_transform )
		{
			if ( p_transform != null ) {
				orientType = OrientType.LookAtTransform;
				lookTrans = p_transform;
			}
			return this;
		}
		/// <summary>
		/// Parameter > If the tween target is a <see cref="Transform"/>, sets the tween so that the target will always look at the given position.
		/// </summary>
		/// <param name="p_position">
		/// The <see cref="Vector3"/> to look at.
		/// </param>
		public PlugVector3Path LookAt( Vector3 p_position )
		{
			orientType = OrientType.LookAtPosition;
			lookPos = p_position;
			lookTrans = null;
			return this;
		}
		
		// ===================================================================================
		// PRIVATE METHODS -------------------------------------------------------------------
		
		/// <summary>
		/// Recreate the path if it was relative,
		/// otherwise simply add the correct starting and ending point so the path can be reached from the property's actual position.
		/// </summary>
		override protected void SetChangeVal()
		{
			if ( orientType != OrientType.None ) {
				// Store orient transform and orientation.
				if ( orientTrans == null ) {
					if ( tweenObj.target is Transform )
						orientTrans = tweenObj.target as Transform;
					else
						orientTrans = null;
				}
				if ( orientTrans != null && !orientTrans.Equals( null ) ) {
					orientation = orientTrans.rotation.eulerAngles;
				}
			}
			
			// Create path.
			Vector3[] pts;
			int indMod = 1;
			int pAdd = ( isClosedPath ? 1 : 0 );
			if ( isRelative ) {
				pts = new Vector3[points.Length + 2 + pAdd]; // Path length is the same (plus control points).
				Vector3 diff = points[0] - typedStartVal;
				for ( int i = 0; i < points.Length; ++i )
					pts[i + indMod] = points[i] - diff;
			} else {
				Vector3 currVal = (Vector3)GetValue();
				if ( points[0].Equals( currVal ) ) {
					pts = new Vector3[points.Length + 2 + pAdd]; // Path length is the same (plus control points).
				} else {
					pts = new Vector3[points.Length + 3 + pAdd]; // Path needs additional point for current value as starting point (plus control points).
					pts[1] = currVal;
					indMod = 2;
				}
				for ( int i = 0; i < points.Length; ++i )
					pts[i + indMod] = points[i];
			}
			
			if ( isClosedPath ) {
				// Close path.
				pts[pts.Length - 2] = pts[1];
			}
			
			// Add control points.
			if ( isClosedPath ) {
				pts[0] = pts[pts.Length - 3];
				pts[pts.Length - 1] = pts[2];
			} else {
				pts[0] = pts[1];
				Vector3 lastP = pts[pts.Length - 2];
				Vector3 diffV = lastP - pts[pts.Length - 3];
				pts[pts.Length - 1] = lastP + diffV;
			}
			
			// Create the path.
			path = new Path( pts );
		}
		
		/// <summary>
		/// Updates the tween.
		/// </summary>
		/// <param name="p_totElapsed">
		/// The total elapsed time since startup.
		/// </param>
		override protected internal void Update ( float p_totElapsed )
		{
			pathPerc = ease( p_totElapsed, 0, 1, tweenObj.duration );
			// Clamp value because path has limited range of 0-1.
			if ( pathPerc > 1 ) pathPerc = 1; else if ( pathPerc < 0 ) pathPerc = 0;
			
			Vector3 v = path.GetPoint( pathPerc );
			SetValue( v );
			
			if ( orientType != OrientType.None && orientTrans != null && !orientTrans.Equals( null ) ) {
				switch ( orientType ) {
				case OrientType.LookAtPosition:
					orientTrans.LookAt( lookPos, Vector3.up );
					break;
				case OrientType.LookAtTransform:
					if ( orientTrans != null && !orientTrans.Equals( null ) ) {
						orientTrans.LookAt( lookTrans.position, Vector3.up );
					}
					break;
				case OrientType.ToPath:
					// FIXME world up doesn't work correctly.
					
//					// Orient transform to path.
//					Vector3 orientV;
//					float lookAhead = pathT + lookAheadVal;
//					if ( lookAhead > 1 ) {
//						// Find virtual lookAhead using backwards coordinate.
//						Vector3 prevV = path.Interpolate( pathT - lookAheadVal );
//						Vector3 diffV = v - prevV;
//						orientV = v + diffV;
//					} else {
//						orientV = path.Interpolate( lookAhead );
//					}
//					orientTrans.LookAt( orientV, Vector3.up );
					
					
					
//					// Orient transform to path.
//					// Calculate tangent.
//					Vector3 tangent;
//					float lookAhead = pathT + lookAheadVal;
//					if ( lookAhead > 1 ) {
//						// Find virtual lookAhead using backwards coordinate.
//						Vector3 prevV = path.Interpolate( pathT - lookAheadVal );
//						tangent = prevV - v;
//					} else {
//						tangent = v - path.Interpolate( lookAhead );
//					}
//					tangent.Normalize();
//					// Calculate binormal.
//					Vector3 binormal = Vector3.Cross( tangent, Vector3.up );
//					binormal.Normalize();
//					// Calculate normal.
//					Vector3 normal = Vector3.Cross( binormal, tangent );
//					normal.Normalize();
//					// Set rotation.
//					Matrix4x4 m = new Matrix4x4();
//					m.SetColumn( 0, tangent );
//					m.SetColumn( 1, normal );
//					m.SetColumn( 2, binormal );
//					orientTrans.rotation = Utils.MatrixToQuaternion( m );
					
					
					
//					// Orient transform to path.
//					Vector3 orientV;
//					Vector3 forward;
//					float lookAhead = pathT + lookAheadVal;
//					if ( lookAhead > 1 ) {
//						// Find virtual lookAhead using backwards coordinate.
//						Vector3 prevV = path.Interpolate( pathT - lookAheadVal ); // FIXME
//						Vector3 diffV = v - prevV;
//						orientV = v + diffV;
//						forward = Vector3.Normalize( orientV - prevV );
//					} else {
//						orientV = path.Interpolate( lookAhead );
//						forward = Vector3.Normalize( orientV - v );
//					}
//					orientTrans.rotation = Rot( Vector3.forward, forward, Vector3.up );
					
					
					
//					// Orient transform to path.
//					Vector3 orientV;
//					Vector3 forward;
//					float lookAhead = pathT + lookAheadVal;
//					if ( lookAhead > 1 ) {
//						// Find virtual lookAhead using backwards coordinate.
//						Vector3 prevV = path.Interpolate( pathT - lookAheadVal ); // FIXME
//						Vector3 diffV = v - prevV;
//						orientV = v + diffV;
//						forward = orientV - prevV;
//					} else {
//						orientV = path.Interpolate( lookAhead );
//						forward = orientV - v;
//					}
//					orientTrans.LookAt( orientV, Vector3.up );
					
					
					
//					// Orient transform to path.
//					Vector3 prevP;
//					Vector3 startP = v;
//					Vector3 lookAt;
//					float aheadPerc = pathPerc + lookAheadVal;
//					float behindPerc;
//					if ( aheadPerc > 1 ) {
//						aheadPerc = 1;
//						behindPerc = 1 - lookAheadVal * 2;
//						startP = path.GetPoint( 1 - lookAheadVal );
//					} else {
//						behindPerc = pathPerc - lookAheadVal;
//						if ( behindPerc < 0 ) {
//							behindPerc = 0;
//							aheadPerc = lookAheadVal * 2;
//							startP = path.GetPoint( lookAheadVal );
//						}
//					}
////					lookAt = path.Interpolate( aheadPerc );
////					bool invertUp = ( lookAt.x < startP.x && lookAt.y > startP.y );
////					Vector3 up = ( invertUp ? -Vector3.up : Vector3.up );
////					orientTrans.LookAt( lookAt, up );
//					
//					prevP = path.GetPoint( behindPerc );
//					lookAt = path.GetPoint( aheadPerc );
//					float angX = XAngleBetween( startP, prevP, lookAt );
//					float angY = YAngleBetween( startP, prevP, lookAt );
//					float angZ = ZAngleBetween( startP, prevP, lookAt );
//					Debug.Log( ">>>> ANG: " + angX + " / " + angY + " / " + angZ + " ////// " + Vector3.Angle( startP, lookAt ) );
//					bool invertUp =	( angX > 0 && ( angY < 0 && angZ < 0 ) )
//									|| angX < 0 && angY < 0 && angZ < 0
//									|| angY > 3.14f && angX > 0 && angZ > 0;
//					Vector3 up = ( invertUp ? -Vector3.up : Vector3.up );
//					orientTrans.LookAt( lookAt, Vector3.up );
					
					
					
//					// Orient transform to path.
//					Vector3 prevP;
//					Vector3 p = v;
//					Vector3 nextP;
//					float nextT = pathPerc + lookAheadVal;
//					if ( nextT > 1 ) {
//						nextP = p;
//						p = path.GetPoint( pathPerc - lookAheadVal );
//						prevP = path.GetPoint( pathPerc - lookAheadVal * 2 );
//					} else {
//						float prevT = pathPerc - lookAheadVal;
//						if ( prevT < 0 ) {
//							prevP = p;
//							p = path.GetPoint( pathPerc + lookAheadVal );
//							nextP = path.GetPoint( pathPerc + lookAheadVal * 2 );
//						} else {
//							prevP = path.GetPoint( prevT );
//							nextP = path.GetPoint( nextT );
//						}
//					}
//					Vector3 tangent = nextP - p;
//					tangent.Normalize();
//					Vector3 tangent2 = p - prevP;
//					tangent2.Normalize();
//					Vector3 normal = Vector3.Cross( tangent, tangent2 );
//					normal.Normalize();
//					Vector3 binormal = Vector3.Cross( tangent, normal );
//					binormal.Normalize();
//					// Addition - normal with given up vector.
//					Vector3 binormal2 = Vector3.Cross( tangent, Vector3.up );
//					binormal2.Normalize();
//					Vector3 normal2 = Vector3.Cross( binormal2, tangent );
//					normal2.Normalize();
//					// Orient.
//					float dot = Vector3.Dot( binormal, binormal2 );
//					Vector3 up = Vector3.up + binormal2 - binormal;
//					bool invertUp = ( binormal2.z < 0 && ( Mathf.Round( Vector3.Angle( normal, normal2 ) ) == 90 ) );
////					up = ( invertUp ? up * -1 : up );
//					orientTrans.LookAt( nextP, up );
//					Debug.Log( ">>> " + ( invertUp ? "INVERT! " : "" ) + Vector3.Angle( normal, normal2 ) + " / " + Vector3.Angle( binormal, binormal2 ) );
////					Debug.Log( "    >>> " + ( invertUp ? "INVERT! " : "" ) + Vector3.Angle( binormal, tangent2 ) + " / " + Vector3.Angle( binormal2, tangent ) );
					
					
					
					// Orient transform to path.
					Vector3 prevP;
					Vector3 p = v;
					Vector3 nextP;
					float nextT = pathPerc + lookAheadVal;
					if ( nextT > 1 ) {
						nextP = p;
						p = path.GetPoint( pathPerc - lookAheadVal );
						prevP = path.GetPoint( pathPerc - lookAheadVal * 2 );
					} else {
						float prevT = pathPerc - lookAheadVal;
						if ( prevT < 0 ) {
							prevP = p;
							p = path.GetPoint( pathPerc + lookAheadVal );
							nextP = path.GetPoint( pathPerc + lookAheadVal * 2 );
						} else {
							prevP = path.GetPoint( prevT );
							nextP = path.GetPoint( nextT );
						}
					}
					Vector3 tangent = nextP - p;
					tangent.Normalize();
					Vector3 binormal = Vector3.Cross( tangent, Vector3.up );
					binormal.Normalize();
					Vector3 normal = Vector3.Cross( binormal, tangent );
					normal.Normalize();
					// Set rotation.
					Matrix4x4 m = new Matrix4x4();
					m.SetColumn( 0, tangent );
					m.SetColumn( 1, normal );
					m.SetColumn( 2, binormal );
					orientTrans.rotation = Utils.MatrixToQuaternion( m );
					
					
					
					break;
				}
			}
		}
	}
}

