///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright(c) 2016, Sidney Fernandez                                                                                                                                                                                                              //
// All rights reserved.                                                                                                                                                                                                                      //
//                                                                                                                                                                                                                                           //
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:                                                                                            //
//                                                                                                                                                                                                                                           //
// 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.                                                                                                           //
//                                                                                                                                                                                                                                           //
// 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.                             //
//                                                                                                                                                                                                                                           //
// 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.                                            //
//                                                                                                                                                                                                                                           //
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A                             //
// PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,                     //
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)   //
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                                                                                                                    //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public static class HelperExtensions
{
    /// <summary>
    /// turns a planar vector2 into a local vector3
    /// </summary>
    /// <param name="Frame">Frame of reference</param>
    /// <param name="PlanarVector">Vector to be translated</param>
    /// <returns></returns>
    public static Vector3 PlanarToLocal(this Transform Frame, Vector2 PlanarVector)
    {
        //Debug.Log("frame forward: " + Frame.forward + " planar y: " + PlanarVector.y);
        return Frame.forward * PlanarVector.y + Frame.right * PlanarVector.x;
    }

    /// <summary>
    /// turns a planar vector2 into a local forward vector
    /// </summary>
    /// <param name="Frame">Frame of reference</param>
    /// <param name="PlanarVector">Vector to be translated</param>
    /// <returns></returns>
    public static Vector3 PlanarToLocalForward(this Transform Frame, Vector2 PlanarVector)
    {
        //Debug.Log("frame forward: " + Frame.forward + " planar y: " + PlanarVector.y);
        return Frame.forward * PlanarVector.y;
    }

    /// <summary>
    /// Keep a collider cenetered around a position and extending to the floor
    /// </summary>
    /// <param name="col"></param>
    /// <param name="pos">local position to center under</param>
    public static void AutoCalibrateColider(this CapsuleCollider col, Vector3 pos)
    {
        col.height = pos.y;
        col.center = new Vector3(0, -pos.y * 0.5f, 0);
    }

    public static Vector3 ValwiseMultiply(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }
    

    
}
