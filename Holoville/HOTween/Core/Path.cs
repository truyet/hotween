// 
// Path.cs
//  
// Author: Daniele Giardini
// 
// Copyright (c) 2012 Daniele Giardini - Holoville - http://www.holoville.com
// Based on Andeeee's CRSpline (http://forum.unity3d.com/threads/32954-Waypoints-and-constant-variable-speed-problems?p=213942&viewfull=1#post213942)
// Contains code ported from Stephen Vincent and David Forsey's Fast and accurate parametric curve length computation, Journal of graphics tools, 6(4):29-40, 2001 (http://jgt.akpeters.com/papers/VincentForsey01/CurveLength.html).
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

namespace Holoville.HOTween.Core
{
	/// <summary>
	/// Used to manage movement on a Cardinal spline (of Catmull-Rom type).
	/// Based on Andeeee's CRSpline (http://forum.unity3d.com/threads/32954-Waypoints-and-constant-variable-speed-problems),
	/// and on Stephen Vincent and David Forsey's Fast and accurate parametric curve length computation,
	/// Journal of graphics tools, 6(4):29-40, 2001 (http://jgt.akpeters.com/papers/VincentForsey01/CurveLength.html).
	/// </summary>
	internal class Path
	{
		// VARS ///////////////////////////////////////////////////
		
		internal		float			defArcLengthPerc;
		internal		Vector3[]		path;
		
		
		// ***********************************************************************************
		// CONSTRUCTOR
		// ***********************************************************************************
		
		/// <summary>
		/// Creates a new <see cref="Path"/> based on the given array of <see cref="Vector3"/> points.
		/// </summary>
		/// <param name="p_path">
		/// The <see cref="Vector3"/> array used to create the path.
		/// </param>
		public Path( params Vector3[] p_path )
		{
			path = new Vector3[p_path.Length];
			Array.Copy( p_path, path, path.Length );
		}
		
		// ===================================================================================
		// METHODS ---------------------------------------------------------------------------
		
		/// <summary>
		/// Gets the point on the curve at the given time position.
		/// </summary>
		public Vector3 GetPoint( float t )
		{
			int numSections = path.Length - 3;
			int tSec = Mathf.FloorToInt( t * numSections );
			int currPt = numSections - 1;
			if ( currPt > tSec )		currPt = tSec;
			float u = t * numSections - currPt;
			
			Vector3 a = path[currPt];
			Vector3 b = path[currPt + 1];
			Vector3 c = path[currPt + 2];
			Vector3 d = path[currPt + 3];
			
			return .5f * (
				( -a + 3f * b - 3f * c + d ) * ( u * u * u )
				+ ( 2f * a - 5f * b + 4f * c - d ) * ( u * u )
				+ ( -a + c ) * u
				+ 2f * b
			);
		}
		
		/// <summary>
		/// Gets the velocity at the given time position.
		/// </summary>
		public Vector3 Velocity( float t )
		{
			int numSections = path.Length - 3;
			int tSec = (int)Mathf.Floor( t * numSections );
			int currPt = numSections - 1;
			if ( currPt > tSec )		currPt = tSec;
			float u = t * numSections - currPt;
			
			Vector3 a = path[currPt];
			Vector3 b = path[currPt + 1];
			Vector3 c = path[currPt + 2];
			Vector3 d = path[currPt + 3];
			
			return 1.5f * ( -a + 3f * b - 3f * c + d ) * ( u * u )
				+ ( 2f * a -5f * b + 4f * c - d ) * u
				+ .5f * c - .5f * a;
		}
		
		/// <summary>
		/// Returns the path length between the given points in time,
		/// approximating with the given subdivisions.
		/// Port from Stephen Vincent and David Forsey's Fast and accurate parametric curve length computation,
		/// Journal of graphics tools, 6(4):29-40, 2001
		/// (http://jgt.akpeters.com/papers/VincentForsey01/CurveLength.html).
		/// </summary>
		public float GetArcLength( float p_minT, float p_maxT, int p_subdivisions )
		{
			if ( ( p_subdivisions & 0x01 ) == 0 ) ++p_subdivisions;
			
			float len = 0;
			float[] ts = new float[p_subdivisions];
			Vector3[] ps = new Vector3[p_subdivisions];
			
			for ( int i = 0; i < p_subdivisions; ++i ) {
				ts[i] = p_minT + ( p_maxT - p_minT ) * ( (float)i / ( p_subdivisions - 1 ) );
				ps[i] = GetPoint( ts[i] );
			}
			
			for ( int i = 0; i < p_subdivisions - 1; i += 2 ) {
				len += GetSectionLength( ts[i], ts[i+1], ts[i+2], ps[i], ps[i+1], ps[i+2] );
			}
			
			return len;
		}
		
		/// <summary>
		/// Draws the full path.
		/// </summary>
		public void GizmoDraw() { GizmoDraw( -1, false ); }
		/// <summary>
		/// Draws the full path, and if <c>t</c> is not -1 also draws the velocity at <c>t</c>.
		/// </summary>
		/// <param name="t">
		/// The point where to calculate velocity and eventual additional trigonometry.
		/// </param>
		/// <param name="p_drawTrig">
		/// If <c>true</c> also draws the normal, tangent, and binormal of t.
		/// </param>
		public void GizmoDraw( float t, bool p_drawTrig )
		{
			Gizmos.color = new Color( 0.6f,0.6f,0.6f,0.6f );
			
			float pm;
			Vector3 currPt;
			Vector3 prevPt = GetPoint( 0 );
			int subdivisions = 200;
			for ( int i = 1; i <= subdivisions; ++i ) {
				pm = i / (float)subdivisions;
				currPt = GetPoint( pm );
				Gizmos.DrawLine( currPt, prevPt );
				prevPt = currPt;
			}
			
			if ( t != -1 ) {
				Vector3 pos = GetPoint( t );
				Gizmos.color = Color.blue;
				Gizmos.DrawLine( pos, pos + Velocity( t ) );
				if ( p_drawTrig ) {
					Vector3 prevP;
					Vector3 p = pos;
					Vector3 nextP;
					float nextT = t + 0.0001f;
					if ( nextT > 1 ) {
						nextP = pos;
						p = GetPoint( t - 0.0001f );
						prevP = GetPoint( t - 0.0002f );
					} else {
						float prevT = t - 0.0001f;
						if ( prevT < 0 ) {
							prevP = pos;
							p = GetPoint( t + 0.0001f );
							nextP = GetPoint( t + 0.0002f );
						} else {
							prevP = GetPoint( prevT );
							nextP = GetPoint( nextT );
						}
					}
					Vector3 tangent = nextP - p;
					tangent.Normalize();
					Vector3 tangent2 = p - prevP;
					tangent2.Normalize();
					Vector3 normal = Vector3.Cross( tangent, tangent2 );
					normal.Normalize();
					Vector3 binormal = Vector3.Cross( tangent, normal );
					binormal.Normalize();
					// Draw normal.
					Gizmos.color = Color.black;
					Gizmos.DrawLine( pos, pos + tangent );
					Gizmos.color = Color.blue;
					Gizmos.DrawLine( pos, pos + normal );
					Gizmos.color = Color.red;
					Gizmos.DrawLine( pos, pos + binormal );
				}
			}
		}
		
		// ===================================================================================
		// INTERNAL METHODS ------------------------------------------------------------------
		
		/// <summary>
		/// Returns the percentage for each arc (1 being the full length of the whole curve).
		/// Used to move with constant speed along the path.
		/// </summary>
		internal float[] GetArcLengthsPercentages()
		{
			float[] arcLens = GetArcLengths( 10 );
			float[] percs = new float[arcLens.Length];
			float fullLen = 0;
			for ( int i = 0; i < arcLens.Length; ++i )	fullLen += arcLens[i];
			float defArcLen = fullLen / arcLens.Length;
			defArcLengthPerc = 1f / arcLens.Length;
			for ( int i = 0; i < percs.Length; ++i ) {
				percs[i] = ( arcLens[i] * defArcLengthPerc ) / defArcLen;
			}
			return percs;
		}
		
		// ===================================================================================
		// PRIVATE METHODS -------------------------------------------------------------------
		
		/// <summary>
		/// Returns an array with the lengths of each arc section (point to next point) of the curve,
		/// approximating with the given subdivisions.
		/// </summary>
		private float[] GetArcLengths( int p_subdivisions )
		{
			float[] lens = new float[path.Length - 3];
			float arcDuration = 1f / lens.Length;
			float prevT = 0;
			for ( int i = 0; i < lens.Length; ++i ) {
				lens[i] = GetArcLength( prevT, prevT + arcDuration, p_subdivisions );
				prevT += arcDuration;
			}
			return lens;
		}
		
		/// <summary>
		/// Port from Stephen Vincent and David Forsey's Fast and accurate parametric curve length computation,
		/// Journal of graphics tools, 6(4):29-40, 2001
		/// (http://jgt.akpeters.com/papers/VincentForsey01/CurveLength.html).
		/// </summary>
		private float GetSectionLength( float t0, float t1, float t2, Vector3 p0, Vector3 p1, Vector3 p2 )
		{
			float kEpsilon = 0.00001f;
			float kEpsilon2 = 0.000001f;
			float kMaxArc = 1.05f;
			float kLenRatio = 1.2f;
			
			float d1, d2, len1, len2, da, db;
			
			d1 = Vector3.Distance( p0, p2 );
			da = Vector3.Distance( p0, p1 );
			db = Vector3.Distance( p1, p2 );
			d2 = da + db;
			
			if ( d2 < kEpsilon ) {
				return ( d2 + ( d2 - d1 ) / 3 );
			} else if ( d1 < kEpsilon || d2 / d1 > kMaxArc || da < kEpsilon2 || db / da > kLenRatio || db < kEpsilon2 || da / db > kLenRatio ) {
				//	We're in a region of high curvature. Recurse.
				//
				//	Lengths are tested against kEpsilon and kEpsilon2 just
				//	to prevent divison-by-zero/overflow.
				//
				//	However kEpsilon2 should be less than half of kEpsilon 
				//	otherwise we'll get unnecessary recursion.
				//
				//	The value of kMaxArc implicitly refers to the maximum
				//	angle that can be subtended by the circular arc that 
				//	approximates the curve between t and prev_t.
				//	The relationship is : kMaxArc = ( 1 - ø*ø/24 ) /
				//	( 1 - ø*ø/6 ).
				//	Rearranging gives ø = sqrt ( 24 * ( kMaxArc - 1 ) / 
				//	( 4 * kMaxArc - 1 ) )
				//
				//	kLenRatio : when the lengths of da and db become too
				//	dissimilar the curve probably ( not necessarily ) 
				//	can't be approximated by a circular arc here.
				//	Recurse again : a value of 1.1 is a little high in 
				//	that it won't accurately detect a certain pathological 
				//	case of cusp mentioned in the documentation : on the 
				//	other hand too low a value results in unnecessary
				//	recursion.
				Vector3 midP;
				float midT = ( t0 + t1 ) * 0.5f;
				midP = GetPoint( midT );
				len1 = GetSectionLength( t0, midT, t1, p0, midP, p1 );
				midT = ( t1 + t2 ) * 0.5f;
				midP = GetPoint( midT );
				len2 = GetSectionLength( t1, midT, t2, p1, midP, p2 );
				return ( len1 + len2 );
			} else {
				return ( d2 + ( d2 - d1 ) / 3 );
			}
		}
	}
}

