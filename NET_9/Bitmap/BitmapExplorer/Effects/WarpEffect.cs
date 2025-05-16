using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

using D2D = C1.Util.DX.Direct2D;
using DXGI = C1.Util.DX.DXGI;
using C1.Util.DX;

namespace BitmapExplorer
{
    [D2D.CustomEffect("Adds the Warp effect", "Distort", "C1")]
    [D2D.CustomEffectInput("Source")]
    public class WarpEffect : CallbackBase, D2D.CustomEffect, D2D.DrawTransform
    {
        //----------------------------------------------------------------------
        #region ** constants

        const int TesselationLevelsX = 200;
        const int TesselationLevelsY = 120;

        // {DAEC286F-66E3-4B07-AACE-DE38A21E8A6C}
        static readonly Guid GUID_WarpVertexShader = new Guid(0xdaec286f, 0x66e3, 0x4b07, 0xaa, 0xce, 0xde, 0x38, 0xa2, 0x1e, 0x8a, 0x6c);

        // {CE40DBCB-515D-41C4-9089-DC7582888897}
        static readonly Guid GUID_WarpVertexBuffer = new Guid(0xce40dbcb, 0x515d, 0x41c4, 0x90, 0x89, 0xdc, 0x75, 0x82, 0x88, 0x88, 0x97);

        #endregion

        //----------------------------------------------------------------------
        #region ** ConstantBuffer

        [StructLayout(LayoutKind.Sequential)]
        struct ConstantBuffer
        {
            public Size2F Size;
            public Point2F StartPos;
            public Point2F EndPos;
            public float Distance;
        }

        /// <summary>
        /// Constants used to set properties for Warp custom effect.
        /// </summary>
        enum WarpProperties
        {
            Size,
            StartPos,
            EndPos,
            Distance
        }

        #endregion

        //----------------------------------------------------------------------
        #region ** fields

        ConstantBuffer _constBuffer;
        Point2F _warpStart;
        Point2F _warpEnd;

        D2D.DrawInformation _drawInfo;
        RectL _inputRect;
        byte[] _shaderBuffer;
        D2D.VertexBuffer _vertexBuffer;
        int _numVertices;
        D2D.Effect _effect;

        #endregion

        //----------------------------------------------------------------------
        #region ** ctor/Dispose

        /// <summary>
        /// Initializes a new instance of <see cref="WarpEffect"/> class.
        /// </summary>
        public WarpEffect()
        {
            _constBuffer = new ConstantBuffer
            {
                Size = Size2F.Empty,
                StartPos = Point2F.Empty,
                EndPos = Point2F.Empty,
                Distance = 0f
            };
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DXUtil.Dispose(ref _effect);
                DXUtil.Dispose(ref _vertexBuffer);
                DXUtil.Dispose(ref _drawInfo);
            }
            base.Dispose(disposing);
        }

        #endregion

        //----------------------------------------------------------------------
        #region ** API for public usage

        /// <summary>
        /// Gets or sets the effect objects.
        /// </summary>
        public D2D.Effect Effect
        {
            get { return _effect; }
            set
            {
                value.CustomEffect = null;
                _effect = value;
            }
        }

        /// <summary>
        /// Set the normalized [0, 1] positions of the start and end points.
        /// </summary>
        public void SetNormPositions(Point2F start, Point2F end)
        {
            _warpStart = start;
            _warpEnd = end;
            UpdateConstants();
        }

        #endregion

        //----------------------------------------------------------------------
        #region ** binding to the constant buffer

        /// <summary>
        /// Gets the size of the image, in pixels.
        /// </summary>
        [D2D.PropertyBinding(D2D.BindingType.Vector2, (int)WarpProperties.Size, "(0, 0)", "(1000000, 1000000)", "(0, 0)")]
        public Size2F Size
        {
            get { return _constBuffer.Size; }
        }

        /// <summary>
        /// Gets the offset of the start point, in pixels.
        /// </summary>
        [D2D.PropertyBinding(D2D.BindingType.Vector2, (int)WarpProperties.StartPos, "(0, 0)", "(1000000, 1000000)", "(0, 0)")]
        public Point2F StartPosition
        {
            get { return _constBuffer.StartPos; }
        }

        /// <summary>
        /// Gets the offset of the end point, in pixels.
        /// </summary>
        [D2D.PropertyBinding(D2D.BindingType.Vector2, (int)WarpProperties.EndPos, "(0, 0)", "(1000000, 1000000)", "(0, 0)")]
        public Point2F EndPosition
        {
            get { return _constBuffer.EndPos; }
        }

        /// <summary>
        /// Gets the pre-calculated value of the affected distance, in pixels.
        /// </summary>
        [D2D.PropertyBinding(D2D.BindingType.Float, (int)WarpProperties.Distance, "0", "1000000", "0")]
        public float Distance
        {
            get { return _constBuffer.Distance; }
        }

        void UpdateConstants()
        {
            // exit if size is not initialized yet
            if (_drawInfo == null || _constBuffer.Size.IsEmpty)
            {
                return;
            }

            // pre-calculate some values
            var sz = _constBuffer.Size;
            var sp = new Point2F(_warpStart.X * sz.Width, _warpStart.Y * sz.Height);
            var ep = new Point2F(_warpEnd.X * sz.Width, _warpEnd.Y * sz.Height);

            _constBuffer.StartPos = sp;
            _constBuffer.EndPos = ep;
            _constBuffer.Distance = sp.Distance(ep) * 1.5f;

            _drawInfo.SetVertexConstantBuffer(ref _constBuffer);
        }

        #endregion

        //----------------------------------------------------------------------
        #region ** CustomEffect

        /// <summary>	
        /// Creates any resources used repeatedly during subsequent rendering calls.
        /// </summary>	
        public void Initialize(D2D.EffectContext effectContext, D2D.TransformGraph transformGraph)
        {
            if (_shaderBuffer == null)
            {
                Assembly asm = typeof(WarpEffect).Assembly;
                using (Stream stream = asm.GetManifestResourceStream("BitmapExplorer.Resources.WarpEffect.cso"))
                {
                    int length = (int)stream.Length;
                    _shaderBuffer = new byte[length];
                    stream.Read(_shaderBuffer, 0, length);
                }
            }
            effectContext.LoadVertexShader(GUID_WarpVertexShader, _shaderBuffer);

            _vertexBuffer = effectContext.FindVertexBuffer(GUID_WarpVertexBuffer);
            if (_vertexBuffer == null)
            {
                using (var meshStream = GenerateMesh())
                {
                    var props = new D2D.VertexBufferProperties(1, D2D.VertexUsage.Static, meshStream);
                    var desc = new D2D.InputElement("MESH_POSITION", 0, DXGI.Format.R32G32_Float, 0, 0);
                    var custProps = new D2D.CustomVertexBufferProperties(_shaderBuffer, new D2D.InputElement[] { desc }, DXUtil.SizeOf<Vector2>());
                    _vertexBuffer = D2D.VertexBuffer.Create(effectContext, GUID_WarpVertexShader, props, custProps);
                }
            }

            transformGraph.SetSingleTransformNode(this);
        }

        DataStream GenerateMesh()
        {
            float offsetX = 1f / TesselationLevelsX;
            float offsetY = 1f / TesselationLevelsY;
            _numVertices = TesselationLevelsX * TesselationLevelsY * 6;
            var stream = new DataStream(DXUtil.SizeOf<Vector2>() * _numVertices, false, true);
            for (int i = 0; i < TesselationLevelsY; i++)
                for (int j = TesselationLevelsX - 1; j >= 0; j--)
                {
                    float x = offsetX * j;
                    float y = offsetY * i;
                    stream.Write(new Vector2(x, y));
                    stream.Write(new Vector2(x, y + offsetY));
                    stream.Write(new Vector2(x + offsetX, y));
                    stream.Write(new Vector2(x + offsetX, y));
                    stream.Write(new Vector2(x, y + offsetY));
                    stream.Write(new Vector2(x + offsetX, y + offsetY));
                }
            return stream;
        }

        /// <summary>	
        /// Prepares an effect for the rendering process.	
        /// </summary>	
        public void PrepareForRender(D2D.ChangeType changeType)
        {
            UpdateConstants();
        }

        /// <summary>	
        /// The renderer calls this method to provide the effect implementation with a way to specify its transform graph and transform graph changes. 
        /// It is executed when: 1) When the effect is first initialized. 2) If the number of inputs to the effect changes.
        /// </summary>	
        public int SetGraph(D2D.TransformGraph transformGraph)
        {
            return HResult.NotImplemented.Code;
        }

        #endregion

        //----------------------------------------------------------------------
        #region ** DrawTransform

        /// <summary>	
        /// Sets the GPU render information for the transform.
        /// </summary>	
        public void SetDrawInfo(D2D.DrawInformation drawInfo)
        {
            _drawInfo = drawInfo;
            var range = new D2D.VertexRange { StartVertex = 0, VertexCount = _numVertices };
            drawInfo.SetVertexProcessing(_vertexBuffer, D2D.VertexOptions.UseDepthBuffer, null, range, GUID_WarpVertexShader);
        }

        /// <summary>	
        /// Performs the inverse mapping to MapOutputRectToInputRects.
        /// </summary>	
        public RectL MapInputRectsToOutputRect(RectL[] inputRects, RectL[] inputOpaqueSubRects, out RectL outputOpaqueSubRect)
        {
            RectL rc = inputRects[0];
            _inputRect = rc;
            var newSize = new Size2F(rc.Width, rc.Height);
            if (_constBuffer.Size != newSize)
            {
                _constBuffer.Size = newSize;
                UpdateConstants();
            }
            outputOpaqueSubRect = RectL.Empty;
            return rc;
        }

        /// <summary>	
        /// Allows a transform to state how it would map a rectangle requested on its output to a set of sample rectangles on its input.
        /// </summary>	
        public void MapOutputRectToInputRects(RectL outputRect, RectL[] inputRects)
        {
            inputRects[0] = _inputRect;
        }

        /// <summary>
        /// Sets the input rectangles for this rendering pass into the transform.
        /// </summary>
        public RectL MapInvalidRect(int inputIndex, RectL invalidInputRect)
        {
            return _inputRect;
        }

        /// <summary>
        /// Gets the number of inputs to the transform node.
        /// </summary>
        public int GetInputCount()
        {
            return 1;
        }

        #endregion
    }
}
