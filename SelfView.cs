using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using UIKit;
using CoreImage;
using CoreVideo;
using CoreMedia;
using AVFoundation;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using ObjCRuntime;
using Foundation;

namespace VidiOS
{
    public partial class SelfView : UIView
    {
    	CaptureSession captureSession;
		Dictionary<int,Face> faces = new Dictionary<int,Face> ();

		public event EventHandler<FacesEventArgs> FacesDetected;
		public event EventHandler<FacesEventArgs> FacesRemoved;

		[Export ("layerClass")]
		public static Class LayerClass () => new Class (typeof (AVCaptureVideoPreviewLayer));		
		public new AVCaptureVideoPreviewLayer Layer => (AVCaptureVideoPreviewLayer)base.Layer;

		public SelfView (IntPtr handle) : base (handle)
        {
        }

		public SelfView ()
		{
			// Called when created from code.
			Initialize ();
		}

		public override void AwakeFromNib ()
		{
			// Called when created from XIB or storyboard.
			base.AwakeFromNib ();
			Initialize ();
		}

		void Initialize ()
		{
			captureSession = CaptureSession.Create (this);
			if (captureSession == null) {
				//FIXME: This means the device doesn't have a camera..
				BackgroundColor = UIColor.Gray;
				return;
			}

			Layer.MasksToBounds = true;
			captureSession.SetPreviewLayer (Layer);
			Layer.Connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
			Layer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
		}

		public void Start ()
		{
			captureSession?.Start ();
		}

		public void Stop ()
		{
			captureSession?.Stop ();
		}

		void OnFacesDetected (AVMetadataObject [] objs)
		{
			var newFaces = new List<Face> (objs.Length);
			var removedFaces = new List<Face> (faces.Count);
			removedFaces.AddRange (faces.Values);

			for (var i = 0; i < objs.Length; i++) {
				var face = (AVMetadataFaceObject)objs [i];
				var id = (int)face.FaceID;
				var bounds = Layer.MapToLayerCoordinates (face.Bounds);

				Face model;
				if (!faces.TryGetValue (id, out model)) {
					model = new Face (id, bounds);
					newFaces.Add (model);
					faces.Add (id, model);
				} else {
					// We're not on the main thread here, so we need to
					//  marshal this back because things could be bound to this property..
					BeginInvokeOnMainThread (() => model.Bounds = bounds);

					// Remove this Face from the removedFaces list because we detected it
					removedFaces.Remove (model);
				}
			}

			// Bookeeping
			foreach (var face in removedFaces)
				faces.Remove (face.Id);

			// Raise events
			if (removedFaces.Any ()) {
				var array = removedFaces.ToArray ();
				BeginInvokeOnMainThread (() => FacesRemoved?.Invoke (this, new FacesEventArgs (array)));
			}
			if (newFaces.Any ()) {
				var array = newFaces.ToArray ();
				BeginInvokeOnMainThread (() => FacesDetected?.Invoke (this, new FacesEventArgs (array)));
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing && captureSession != null) {
				captureSession.Dispose ();
				captureSession = null;
			}
			base.Dispose (disposing);
		}

		class CaptureSession : AVCaptureMetadataOutputObjectsDelegate, IDisposable {

			SelfView parent;

			DispatchQueue queue;
			AVCaptureSession session;

			public bool IsStarted => session != null && session.Running;

			private CaptureSession (SelfView parent, AVCaptureDeviceInput input, AVCaptureMetadataOutput output)
			{
				this.parent = parent;
				this.queue = new DispatchQueue ("myQueue");
				this.session = new AVCaptureSession {
					SessionPreset = AVCaptureSession.PresetMedium
				};

				session.AddInput (input);

				output.SetDelegate (this, queue);
				session.AddOutput (output);		
			}

			public void SetPreviewLayer (AVCaptureVideoPreviewLayer previewLayer)
			{
				previewLayer.Session = session;
			}

			public void Start ()
			{
				session.StartRunning ();
			}

			public void Stop ()
			{
				session.StopRunning ();
			}

			public static CaptureSession Create (SelfView parent)
			{
				// create a device input and attach it to the session
				var captureDevice = AVCaptureDevice.DevicesWithMediaType (AVMediaType.Video).FirstOrDefault (d => d.Position == AVCaptureDevicePosition.Front);
				if (captureDevice == null)
					return null;
	
				var input = AVCaptureDeviceInput.FromDevice (captureDevice);
				if (input == null)
					return null;

				var output = new AVCaptureMetadataOutput ();
				var cs = new CaptureSession (parent, input, output);

				// This must be set after the output is added to the sesssion
				output.MetadataObjectTypes = AVMetadataObjectType.Face;

				return cs;
			}

			public override void DidOutputMetadataObjects (AVCaptureMetadataOutput captureOutput, AVMetadataObject [] metadataObjects, AVCaptureConnection connection)
			{
				parent.OnFacesDetected (metadataObjects);
			}

			protected override void Dispose (bool disposing)
			{
				try {
					if (session.Running)
						session.StopRunning ();
					session = null;
					base.Dispose (disposing);
				} catch {
				}
			}
		}
    }
}