﻿#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Runtime.InteropServices;

namespace Flare.Graphics.GL4
{
    public class VBO<T> : IDisposable
        where T : struct
    {
        #region Properties
        /// <summary>
        /// The ID of the vertex buffer object.
        /// </summary>
        public uint vboID { get; private set; }

        /// <summary>
        /// The type of the buffer.
        /// </summary>
        public BufferTarget BufferTarget { get; private set; }

        /// <summary>
        /// The size (in floats) of the type of data in the buffer.  Size * 4 to get bytes.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The type of data that is stored in the buffer (either int or float).
        /// </summary>
        public VertexAttribPointerType PointerType { get; private set; }
        
        /// <summary>
        /// The length of data that is stored in the buffer.
        /// </summary>
        public int Count { get; private set; }
        #endregion

        #region Constructor and Destructor
        /// <summary>
        /// Creates a buffer object of type T with a specified length.
        /// This allows the array T[] to be larger than the actual size necessary to buffer.
        /// Useful for reusing resources and avoiding unnecessary GC action.
        /// </summary>
        /// <param name="Data">An array of data of type T (which must be a struct) that will be buffered to the GPU.</param>
        /// <param name="Length">The length of the valid data in the data array.</param>
        /// <param name="Target">Specifies the target buffer object.</param>
        /// <param name="Hint">Specifies the expected usage of the data store.</param>
        public VBO(T[] Data, int Length, BufferTarget Target = GL4.BufferTarget.ArrayBuffer, BufferUsageHint Hint = BufferUsageHint.StaticDraw)
        {
            Length = System.Math.Max(0, System.Math.Min(Length, Data.Length));

            vboID = GL.CreateVBO<T>(BufferTarget = Target, Data, Hint, Length);

            this.Size = (Data is int[] ? 1 : (Data is Vector2[] ? 2 : (Data is Vector3[] ? 3 : (Data is Vector4[] ? 4 : 0))));
            this.PointerType = (Data is int[] ? VertexAttribPointerType.Int : VertexAttribPointerType.Float);
            this.Count = Length;
        }

        /// <summary>
        /// Creates a buffer object of type T with a specified length.
        /// This allows the array T[] to be larger than the actual size necessary to buffer.
        /// Useful for reusing resources and avoiding unnecessary GC action.
        /// </summary>
        /// <param name="Data">An array of data of type T (which must be a struct) that will be buffered to the GPU.</param>
        /// <param name="Length">The length of the valid data in the data array.</param>
        /// <param name="Target">Specifies the target buffer object.</param>
        /// <param name="Hint">Specifies the expected usage of the data store.</param>
        public VBO(T[] Data, int Position, int Length, BufferTarget Target = GL4.BufferTarget.ArrayBuffer, BufferUsageHint Hint = BufferUsageHint.StaticDraw)
        {
            Length = System.Math.Max(0, System.Math.Min(Length, Data.Length));

            vboID = GL.CreateVBO<T>(BufferTarget = Target, Data, Hint, Position, Length);

            this.Size = (Data is int[] ? 1 : (Data is Vector2[] ? 2 : (Data is Vector3[] ? 3 : (Data is Vector4[] ? 4 : 0))));
            this.PointerType = (Data is int[] ? VertexAttribPointerType.Int : VertexAttribPointerType.Float);
            this.Count = Length;
        }

        /// <summary>
        /// Creates a buffer object of type T.
        /// </summary>
        /// <param name="Data">Specifies a pointer to data that will be copied into the data store for initialization.</param>
        /// <param name="Target">Specifies the target buffer object.</param>
        /// <param name="Hint">Specifies the expected usage of the data store.</param>
        public VBO(T[] Data, BufferTarget Target = GL4.BufferTarget.ArrayBuffer, BufferUsageHint Hint = BufferUsageHint.StaticDraw)
        {
            vboID = GL.CreateVBO<T>(BufferTarget = Target, Data, Hint);

            Size = (Data is int[] ? 1 : (Data is Vector2[] ? 2 : (Data is Vector3[] ? 3 : (Data is Vector4[] ? 4 : 0))));
            PointerType = (Data is int[] ? VertexAttribPointerType.Int : VertexAttribPointerType.Float);
            Count = Data.Length;
        }

        /// <summary>
        /// Creates a static-read array buffer of type T.
        /// </summary>
        /// <param name="Data">Specifies a pointer to data that will be copied into the data store for initialization.</param>
        public VBO(T[] Data)
            : this(Data, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)
        {
        }

        /// <summary>
        /// Check to ensure that the VBO was disposed of properly.
        /// </summary>
        ~VBO()
        {
            Dispose(false);
        }
    
        #endregion

        #region IDisposable Support

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                if (vboID != 0)
                {
                    GL_Cleanup.AddVBO(this.vboID);
                    vboID = 0;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
